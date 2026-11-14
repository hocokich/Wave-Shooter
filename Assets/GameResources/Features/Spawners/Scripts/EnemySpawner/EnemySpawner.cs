namespace WaveShooter.Features.Spawner.EnemySpawner
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Enemy;
    using WaveShooter.Features.Facade;
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Спавнер врага.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] protected SpawnEnemyPool itemPool = default;
        [SerializeField] protected EnemyWaveController waveController = default;

        [SerializeField] protected float spawnInterval = 1.5f;
        [SerializeField] protected Transform spawnPoint = default;

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

            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
            Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

            SpawnItem enemy = itemPool.Get(position, rotation);

            if (enemy != null)
            {
                enemy.SetCooldownHandler(ReleaseEnemy);

                if (enemy.TryGetComponent(out EnemyMovement movement))
                {
                    movement.Initialize(playerTarget);
                }

                enemy.Launch(); 
            }
        }

        protected virtual void ReleaseEnemy(SpawnItem item)
        {
            itemPool.Release(item);
        }
    }
}