namespace WaveShooter.Features.Spawner.EnemySpawner
{
    using UnityEngine;
    using WaveShooter.Features.Spawners;

    /// <summary>Пул врагов с жёстким лимитом на количество созданных экземпляров.</summary>
    public class PoolEnemy : AbstractPool
    {
        [SerializeField, Min(0)] protected int maxCount = 5;

        protected int totalCreated = 0;

        public override bool HasFreeObject =>
            maxCount == 0 || ready.Count > 0 || totalCreated < maxCount;

        protected override SpawnItem Create()
        {
            if (maxCount > 0 && totalCreated >= maxCount) return null;

            SpawnItem item = base.Create();
            if (item != null) totalCreated++;
            return item;
        }
    }
}