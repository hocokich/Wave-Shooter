namespace WaveShooter.Features.Spawner.EnemySpawner
{
    using UnityEngine;
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Пул с ограничением на максимальное количество созданных объектов.
    /// </summary>
    public class SpawnEnemyPool : SpawnItemPool
    {
        /// <summary>
        /// Есть ли свободный объект в пуле или возможность создать новый (если не достигнут лимит).
        /// </summary>
        public bool HasFreeObject => maxCount == 0 || ready.Count > 0 || totalCreated < maxCount;

        [SerializeField, Min(0)] protected int maxCount = 5;

        protected int totalCreated = 0;

        /// <summary>
        /// Переопределяем создание. Если лимит достигнут - возвращаем null.
        /// </summary>
        protected override SpawnItem Create()
        {
            if (maxCount > 0 && totalCreated >= maxCount)
            {
                return null;
            }

            SpawnItem item = base.Create();
            
            if (item != null)
            {
                totalCreated++;
            }

            return item;
        }
    }
}