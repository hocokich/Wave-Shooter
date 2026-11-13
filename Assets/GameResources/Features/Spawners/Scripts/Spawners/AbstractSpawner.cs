namespace WaveShooter.Features.Spawners
{
    using WaveShooter.Features.Spawners.Projectiles;
    using WaveShooter.Features.Shooting;
    using UnityEngine;

    /// <summary>
    /// Спаунер по выстрелу. Сам ставит кубик на луче фасада на своей дистанции.
    /// </summary>
    public abstract class AbstractSpawner : ProviderCharacter
    {
        [SerializeField, Min(0)] protected float spawnDistance = 11.0f;

        protected override void OnFacadeInited()
        {
            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted += OnGunShooted;
        }

        protected override void OnFacadeDisposed()
        {
            if (container.FacadeInstance == null)
            {
                return;
            }

            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted -= OnGunShooted;
        }

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        /// <summary>
        /// Создать кубик в посчитанной точке.
        /// </summary>
        protected abstract SpawnItem Spawn(Vector3 position, Quaternion rotation);

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
