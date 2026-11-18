namespace WaveShooter.Features.Spawners
{
    using UnityEngine;
    using WaveShooter.Features.ShootingCharacter;
    using WaveShooter.Features.Spawners.Projectiles;

    /// <summary>Спаунер снарядов игрока по выстрелу.</summary>
    [RequireComponent(typeof(PoolProjectiles))]
    public class SpawnerProjectiles : AbstractSpawner
    {
        [SerializeField] protected PoolProjectiles itemPool = default;
        [SerializeField, Min(0)] protected float spawnDistance = 11.0f;

        protected GunController gun;

        protected override AbstractPool Pool => itemPool;

        protected override void OnFacadeInited()
        {
            if (itemPool == null) itemPool = GetComponent<PoolProjectiles>();

            if (container?.FacadeInstance == null) return;

            gun = container.FacadeInstance.GetModuleFacade<GunController>();
            if (gun != null) gun.onGunShooted += OnGunShooted;
        }

        protected override void OnFacadeDisposed()
        {
            if (gun != null)
            {
                gun.onGunShooted -= OnGunShooted;
                gun = null;
            }

            base.OnFacadeDisposed();
        }

        protected virtual void OnGunShooted()
        {
            if (!isActiveAndEnabled || container?.FacadeInstance == null) return;

            var raycast = container.FacadeInstance.GetModuleFacade<RaycastController>();
            if (raycast == null || raycast.CameraView == null || !raycast.TryGetRay(out Ray ray))
                return;

            Vector3 position = ray.GetPoint(spawnDistance);
            Quaternion rotation = raycast.CameraView.transform.rotation;

            SpawnItem item = SpawnFromPool(position, rotation);

            // Импульс применяем ПОСЛЕ Launch (Launch сбрасывает скорость).
            if (item != null && item.TryGetComponent(out ProjectileImpulse impulse))
                impulse.Apply(raycast.CameraView);
        }
    }
}