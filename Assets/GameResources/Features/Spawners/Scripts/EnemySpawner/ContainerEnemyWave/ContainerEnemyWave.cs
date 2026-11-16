namespace WaveShooter.Features.Spawner.EnemySpawner.ContainerEnemyWave
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Контейнер модели волн врагов.
    /// </summary>
    [CreateAssetMenu(fileName = "New Container Enemy Wave", menuName = "WaveShooter/Features/ContainerEnemyWave")]
    public class ContainerEnemyWave : ScriptableObject
    {
        /// <summary>
        /// Контейнер инициализирован
        /// </summary>
        public event Action onContainerInited = delegate { };

        /// <summary>
        /// Контейнер почищен
        /// </summary>
        public event Action onContainerDisposed = delegate { };

        /// <summary>
        /// Экземпляр контроллера
        /// </summary>
        /// <value></value>
        public ControllerEnemyWave ControllerInstance
        {
            get => controllerInstance;
            set
            {
                if (value == controllerInstance)
                {
                    controllerInstance = value;
                    return;
                }

                if (controllerInstance != null)
                {
                    onContainerDisposed();
                    controllerInstance = null;
                }

                controllerInstance = value;

                if (controllerInstance != null)
                {
                    onContainerInited();
                }
            }
        }

        [SerializeField] protected ControllerEnemyWave controllerInstance = default;
    }
}