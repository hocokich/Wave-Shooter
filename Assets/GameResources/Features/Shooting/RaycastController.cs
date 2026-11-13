namespace WaveShooter.Features.Shooting
{
    using WaveShooter.Features.Facade;
    using UnityEngine;

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

            ray = cameraView.ScreenPointToRay(ReadPointer());
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
        /// Палец, если он сейчас на экране, иначе курсор.
        /// </summary>
        protected virtual Vector3 ReadPointer()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                return UnityEngine.Input.GetTouch(0).position;
            }

            return UnityEngine.Input.mousePosition;
        }
    }
}
