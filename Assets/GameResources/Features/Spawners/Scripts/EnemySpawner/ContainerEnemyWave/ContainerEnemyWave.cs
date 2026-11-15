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
        public event Action onContainerInited = delegate { };
        public event Action onContainerDisposed = delegate { };

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