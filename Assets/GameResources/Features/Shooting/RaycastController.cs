namespace WaveShooter.Features.Shooting
{
    using WaveShooter.Features.Facade;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Контроллер луча персонажа.
    /// Строит луч из камеры через курсор или палец, рисует его
    /// и отдаёт компонент заданного типа с объекта, в который луч попал.
    /// </summary>
    public class RaycastController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Камера
        /// </summary>
        public virtual Camera CameraView => cameraView;

        [SerializeField] protected Camera cameraView = default;
        [SerializeField, Min(0)] protected float rayDistance = 100.0f;

        protected virtual void Awake()
        {
            if (cameraView == null)
            {
                cameraView = Camera.main;
            }
        }

#if UNITY_EDITOR
        protected virtual void Update() => DrawRay();
#endif

        /// <summary>
        /// Луч из камеры через текущий указатель.
        /// </summary>
        public virtual bool TryGetRay(out Ray ray)
        {
            ray = default;

            if (cameraView == null)
            {
                return false;
            }

            if (!TryReadPointer(out Vector2 screenPosition))
            {
                return false;
            }

            ray = cameraView.ScreenPointToRay(screenPosition);
            return true;
        }

        /// <summary>
        /// Компонент типа T на объекте, в который попал луч. Поиска в родителях нет.
        /// </summary>
        public virtual bool TryHit<T>(out T target) where T : Component
        {
            target = default;

            if (!TryGetRay(out Ray ray))
            {
                return false;
            }

            if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
            {
                return false;
            }

            return hit.collider.TryGetComponent(out target);
        }

        /// <summary>
        /// Линия луча в окне Scene.
        /// </summary>
        protected virtual void DrawRay()
        {
            if (!TryGetRay(out Ray ray))
            {
                return;
            }

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.white);
        }

        /// <summary>
        /// Экранная позиция текущего указателя (мышь / палец / перо).
        /// </summary>
        protected virtual bool TryReadPointer(out Vector2 screenPosition)
        {
            screenPosition = default;

            Pointer pointer = Pointer.current;
            if (pointer == null)
            {
                return false;

            }

            screenPosition = pointer.position.ReadValue();
            return true;
        }
    }
}
