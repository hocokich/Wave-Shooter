namespace WaveShooter.Features.Spawners
{
    using UnityEngine;
    using WaveShooter.Features.ShootingCharacter;
    using WaveShooter.Features.Spawners.Projectiles;

    /// <summary>
    /// Каждый выстрел создаёт новый экземпляр. По кулдауну экземпляр удаляется.
    /// </summary>
    public class SpawnerProjectiles : AbstractCharacterSpawner
    {
        protected override void OnFacadeInited()
        {
            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted += OnGunShooted;
        }
        protected override void OnFacadeDisposed()
        {
            if (container.FacadeInstance == null) return;
            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted -= OnGunShooted;
        }

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

        protected virtual void OnGunShooted()
        {
            RaycastController raycast = container.FacadeInstance.GetModuleFacade<RaycastController>();

            if (!isActiveAndEnabled || raycast == null || raycast.CameraView == null || !raycast.TryGetRay(out Ray ray))
            {
                return;
            }

            Vector3 position = ray.GetPoint(spawnDistance);
            Quaternion rotation = raycast.CameraView.transform.rotation;
            SpawnItem item = Spawn(position, rotation);

            if (item != null && item.TryGetComponent(out ProjectileImpulse impulse))
            {
                impulse.Apply(raycast.CameraView);
            }
        }
        
    }
}
