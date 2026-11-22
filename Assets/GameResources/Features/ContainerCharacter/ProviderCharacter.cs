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

        private bool isFacadeInited;

        protected virtual void Awake()
        {
            container.onContainerInited += HandleContainerInited;
            container.onContainerDisposed += HandleContainerDisposed;

            if (container.FacadeInstance != null)
                HandleContainerInited();
        }

        protected virtual void OnDestroy()
        {
            container.onContainerInited -= HandleContainerInited;
            container.onContainerDisposed -= HandleContainerDisposed;

            if (isFacadeInited)
            {
                isFacadeInited = false;
                OnFacadeDisposed();
            }
        }

        private void HandleContainerInited()
        {
            if (isFacadeInited) return;
            isFacadeInited = true;
            OnFacadeInited();
        }

        private void HandleContainerDisposed()
        {
            if (!isFacadeInited) return;
            isFacadeInited = false;
            OnFacadeDisposed();
        }

        /// <summary>
        /// Фасад убран из контейнера. Прежняя ссылка в этот момент ещё на месте.
        /// </summary>
        protected virtual void OnFacadeDisposed() { }

        protected abstract void OnFacadeInited();
    }
}