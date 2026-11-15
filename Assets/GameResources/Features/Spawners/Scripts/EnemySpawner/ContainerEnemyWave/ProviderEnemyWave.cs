namespace WaveShooter.Features.Spawner.EnemySpawner.ContainerEnemyWave
{
    using UnityEngine;

    /// <summary>
    /// Провайдер волны врагов.
    /// </summary>
    public abstract class ProviderEnemyWave : MonoBehaviour
    {
        /// <summary>
        /// Свойство для обращения компонентов к контейнеру.
        /// </summary>
        public ContainerEnemyWave Container => container;

        [SerializeField] protected ContainerEnemyWave container = default;

        protected virtual void Awake()
        {
            if (container == null) return;

            container.onContainerInited += OnWaveInited;
            container.onContainerDisposed += OnWaveDisposed;

            if (container.ControllerInstance != null)
            {
                OnWaveInited();
            }
        }

        protected virtual void OnDestroy()
        {
            if (container == null) return;

            container.onContainerInited -= OnWaveInited;
            container.onContainerDisposed -= OnWaveDisposed;
        }

        /// <summary>
        /// Фасад убран из контейнера. Прежняя ссылка в этот момент ещё на месте.
        /// </summary>
        protected virtual void OnWaveDisposed() { }

        protected abstract void OnWaveInited();
    }
}