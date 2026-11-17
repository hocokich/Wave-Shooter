namespace WaveShooter.Features.SaveSystem
{
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Абстрактный контроллер системы сохранения.
    /// </summary>
    public abstract class AbstractSaveSystemController : ProviderCharacter
    {
        protected TimerCountdown timerCountdown;
        protected ControllerEnemyWave waveController;

        protected override void OnFacadeInited()
        {
            timerCountdown = FindAnyObjectByType<TimerCountdown>();
            waveController = FindAnyObjectByType<ControllerEnemyWave>();
        }
    }
}