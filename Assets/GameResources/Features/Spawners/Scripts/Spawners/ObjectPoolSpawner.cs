namespace WaveShooter.Features.Spawners
{
    using UnityEngine;

    /// <summary>
    /// Берёт префаб из пула. По кулдауну возвращает его в пул, а не удаляет.
    /// </summary>
    public class ObjectPoolSpawner : AbstractSpawner
    {
        [SerializeField] protected SpawnItemPool itemPool = default;

        protected override void OnFacadeInited()
        {
            if (itemPool == null)
            {
                itemPool = GetComponent<SpawnItemPool>();
            }

            base.OnFacadeInited();
        }

        protected override SpawnItem Spawn(Vector3 position, Quaternion rotation)
        {
            if (itemPool == null)
            {
                Debug.LogError("Пул спауна не задан");
                return null;
            }

            SpawnItem item = itemPool.Get(position, rotation);

            if (item == null)
            {
                return null;
            }

            item.SetCooldownHandler(ReleaseItem);
            item.Launch();
            return item;
        }

        protected virtual void ReleaseItem(SpawnItem item) => itemPool.Release(item);
    }
}
