namespace WaveShooter.Features.Spawner.EnemySpawner.ContainerEnemyWave
{
    using UnityEngine;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Установщик связи волны врагов в контейнере.
    /// </summary>
    public class InstallerEnemyWave : MonoBehaviour
    {
        [SerializeField] protected ControllerEnemyWave waveController = default;
        
        [SerializeField] protected ContainerEnemyWave containerEnemyWave = default;

        protected virtual void Awake() => containerEnemyWave.ControllerInstance = waveController;

        protected virtual void OnDestroy() => containerEnemyWave.ControllerInstance = null;
    }
}