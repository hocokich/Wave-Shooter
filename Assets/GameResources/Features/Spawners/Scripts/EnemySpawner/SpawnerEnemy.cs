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
    /// Спавнер врага.
    /// </summary>
    public class SpawnerEnemy : MonoBehaviour
    {
        [SerializeField] protected PoolSpawnEnemy itemPool = default;
        [SerializeField] protected ControllerEnemyWave waveController = default;

        [SerializeField] protected float spawnInterval = 1.5f;
        [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();

        [SerializeField] protected Transform playerTarget = default; 

        private Coroutine spawnRoutine;

        protected virtual void Start()
        {
            if (playerTarget == null)
            {
                var facade = FindAnyObjectByType<CharacterFacade>();
                if (facade != null) playerTarget = facade.transform;
            }

            spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        protected virtual void OnDisable()
        {
            if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                TrySpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        /// <summary>
        /// Пробуем заспавнить врага.
        /// </summary>
        public virtual void TrySpawnEnemy()
        {
            if (itemPool == null) return;

            if (!itemPool.HasFreeObject)
            {
                return; 
            }

            if (waveController != null && !waveController.TryConsumeEnemy())
            {
                return;
            }

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

            SpawnItem enemy = itemPool.Get(position, rotation);

            if (enemy != null)
            {
                enemy.SetCooldownHandler(ReleaseEnemy);

                if (enemy.TryGetComponent(out EnemyMovement movement))
                {
                    movement.Initialize(playerTarget);
                }

                enemy.Launch();

                if (waveController != null)
                {
                    waveController.RegisterSpawn();
                }
            }
        }

        protected virtual void ReleaseEnemy(SpawnItem item)
        {
            itemPool.Release(item);
            waveController.RegisterKill();
        }
    }
}