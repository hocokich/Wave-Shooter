namespace WaveShooter.Features.Container
{
    using UnityEngine;

    /// <summary>
    /// Провайдер персонажа.
    /// </summary>
    public abstract class ProviderCharacter : MonoBehaviour
    {
        /// <summary>
        /// Свойство для обращения компонентов к контейнеру.
        /// </summary>
        public ContainerCharacter Container => container;

        [SerializeField] protected ContainerCharacter container = default;

        protected virtual void Awake()
        {
            container.onContainerInited += OnFacadeInited;
            container.onContainerDisposed += OnFacadeDisposed;

            if (container.FacadeInstance != null)
            {
                OnFacadeInited();
            }
        }

        protected virtual void OnDestroy()
        {
            container.onContainerInited -= OnFacadeInited;
            container.onContainerDisposed -= OnFacadeDisposed;
        }

        /// <summary>
        /// Фасад убран из контейнера. Прежняя ссылка в этот момент ещё на месте.
        /// </summary>
        protected virtual void OnFacadeDisposed(){}

        protected abstract void OnFacadeInited();
    }
}