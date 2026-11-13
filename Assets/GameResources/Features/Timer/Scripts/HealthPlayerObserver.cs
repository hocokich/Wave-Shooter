namespace WaveShooter.Features.Timer
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Health;

    /// <summary>
    /// Следит за здоровьем игрока.
    /// </summary>
    public sealed class HealthPlayerObserver : ProviderCharacter
    {
        TimerCountdown timerCountdown = default;
        HealthController healthController = default;
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
                healthController.onDie += ZeroTime;
            }
        }
        protected override void OnFacadeDisposed()
        {
            if(healthController != null)
            {
                healthController.onDie -= ZeroTime;
                healthController = null;
            }

            base.OnFacadeDisposed();
        }


        private void ZeroTime()
        {
            timerCountdown.Seconds = 0;
        } 
    }
}