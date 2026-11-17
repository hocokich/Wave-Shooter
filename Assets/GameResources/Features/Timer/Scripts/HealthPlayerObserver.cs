namespace WaveShooter.Features.Timer
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.HealthCharacter;

    /// <summary>
    /// Следит за здоровьем игрока.
    /// Сбрасывает время в 0 если здоровье критическое.
    /// </summary>
    public sealed class HealthPlayerObserver : ProviderCharacter
    {
        private TimerCountdown timerCountdown = default;
        private HealthController healthController = default;

        protected override void Awake()
        {
            base.Awake();

            timerCountdown = GetComponent<TimerCountdown>();
        }
        protected override void OnFacadeInited()
        {
            healthController = Container.FacadeInstance.GetModuleFacade<HealthController>();

            if(healthController != null)
            {
                healthController.onDied += ZeroTime;
            }
        }
        protected override void OnFacadeDisposed()
        {
            if(healthController != null)
            {
                healthController.onDied -= ZeroTime;
                healthController = null;
            }

            base.OnFacadeDisposed();
        }


        private void ZeroTime() => timerCountdown.Seconds = 0;
    }
}