namespace Unitskills.Features.Lesson6.Extenstions
{
    using Unitskills.Features.Lesson5;
    using Unitskills.Features.Lesson5.Container;
    using UnityEngine;

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
