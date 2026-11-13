namespace WaveShooter.Features.Spawners.Extenstions
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Shooting;

    /// <summary>
    /// Утилитный класс.
    /// Информация ControllerRaycast из контейнера.
    /// </summary>
    public static class ContainerRayInfo
    {
        /// <summary>
        /// Возвращает направление и поворот рейкаста.
        /// </summary>
        public static void GetRayDirAndRot(this ContainerCharacter container, out Vector3 direction, out Quaternion rotation)
        {
            RaycastController raycast = container.FacadeInstance.GetModuleFacade<RaycastController>();
            raycast.TryGetRay(out Ray ray);

            direction = ray.direction;
            rotation = Quaternion.LookRotation(ray.direction);
        }
    }
}
