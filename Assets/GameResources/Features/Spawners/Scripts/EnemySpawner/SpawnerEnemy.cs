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

        /// <summary>
        /// Попытка заспавнить врага.
        /// </summary>
        public virtual void TrySpawnEnemy()
        {
            if (itemPool == null || !itemPool.HasFreeObject) return;
            if (waveController != null && !waveController.TryConsumeEnemy()) return;

            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            if (spawnPoints != null && spawnPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Count);
                Transform randomPoint = spawnPoints[randomIndex];
                if (randomPoint != null)
                {
                    position = randomPoint.position;
                    rotation = randomPoint.rotation;
                }
            }

            SpawnItem enemy = SpawnFromPool(position, rotation, item =>
            {
                if (item.TryGetComponent(out EnemyMovement movement))
                    movement.Initialize(playerTarget);
            });

            if (enemy != null && waveController != null)
                waveController.RegisterSpawn();
        }
        protected override void ReleaseItem(SpawnItem item)
        {
            if (item == null) return;

            base.ReleaseItem(item);

            if (waveController != null)
                waveController.RegisterKill();
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