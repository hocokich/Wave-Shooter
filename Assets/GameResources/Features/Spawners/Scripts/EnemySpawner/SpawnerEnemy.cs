namespace WaveShooter.Features.Spawner.EnemySpawner
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WaveShooter.Features.Enemy;
    using WaveShooter.Features.Facade;
    using WaveShooter.Features.Spawners;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Спаунер врагов по таймеру.
    /// </summary>
    public class SpawnerEnemy : AbstractSpawner
    {
        [SerializeField] protected PoolEnemy itemPool = default;
        [SerializeField] protected ControllerEnemyWave waveController = default;

        [SerializeField] protected float spawnInterval = 1.5f;
        [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] protected Transform playerTarget = default;

        private Coroutine spawnRoutine;
        protected override AbstractPool Pool => itemPool;

        protected override void OnFacadeInited()
        {
            if (playerTarget == null && container?.FacadeInstance != null)
                playerTarget = container.FacadeInstance.transform;

            if (playerTarget == null)
            {
                var facade = FindAnyObjectByType<CharacterFacade>();
                if (facade != null) playerTarget = facade.transform;
            }
            
            int amount = 
                waveController != null && waveController.Model != null
                ? waveController.Model.ActiveEnemies : 0;

            if(amount > 0) InitalSpawn(amount);

            spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        protected virtual void OnDisable()
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        protected virtual void InitalSpawn(int amount)
        {
            int max = itemPool != null ? itemPool.MaxCount : 0;
            if (max > 0 && amount > max)
            {
                Debug.LogWarning(
                    $"SpawnerEnemy: сохранённое число врагов ({amount}) больше лимита пула ({max}). " +
                    $"Будет заспавнено {max}.",
                    this);

                amount = max;
                if (waveController != null && waveController.Model != null)
                    waveController.Model.SetActiveEnemies(amount);
            }

            for (int i = 0; i < amount; i++)
            {
                if (!TrySpawnInitialEnemy()) break;
            }

        }
        protected virtual bool TrySpawnInitialEnemy()
        {
            if (itemPool == null || !itemPool.HasFreeObject) return false;

            var (pos, rot) = GetSpawnPose();
            return SpawnEnemyAt(pos, rot) != null;
        }
        protected virtual (Vector3 position, Quaternion rotation) GetSpawnPose()
        {
            if (spawnPoints != null && spawnPoints.Count > 0)
            {
                int index = Random.Range(0, spawnPoints.Count);
                var point = spawnPoints[index];
                if (point != null) return (point.position, point.rotation);
            }
            return (transform.position, transform.rotation);
        }
        protected virtual SpawnItem SpawnEnemyAt(Vector3 position, Quaternion rotation)
        {
            return SpawnFromPool(position, rotation, item =>
            {
                if (item.TryGetComponent(out EnemyMovement movement))
                    movement.Initialize(playerTarget);
            });
        }
        protected override void ReleaseItem(SpawnItem item)
        {
            if (item == null) return;

            base.ReleaseItem(item);

            if (waveController != null)
                waveController.RegisterKill();
        }
        protected virtual void TrySpawnEnemy()
        {
            if (itemPool == null || !itemPool.HasFreeObject) return;
            if (waveController != null && !waveController.TryConsumeEnemy()) return;

            var (pos, rot) = GetSpawnPose();
            var enemy = SpawnEnemyAt(pos, rot);

            if (enemy != null && waveController != null)
                waveController.RegisterSpawn();

        }
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                TrySpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}