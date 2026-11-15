namespace WaveShooter.Features.Spawners
{
    using UnityEngine;

    /// <summary>
    /// Каждый выстрел создаёт новый экземпляр. По кулдауну экземпляр удаляется.
    /// </summary>
    public class ConcreteSpawnerSimple : AbstractSpawner
    {
        [SerializeField] protected SpawnItem itemPrefab = default;

        protected override SpawnItem Spawn(Vector3 position, Quaternion rotation)
        {
            if (itemPrefab == null)
            {
                Debug.LogError("Префаб спауна не задан");
                return null;
            }

            SpawnItem item = Instantiate(itemPrefab, position, rotation);
            item.SetCooldownHandler(DestroyItem);
            item.Launch();
            return item;
        }

        protected virtual void DestroyItem(SpawnItem item) => Destroy(item.gameObject);
    }
}
