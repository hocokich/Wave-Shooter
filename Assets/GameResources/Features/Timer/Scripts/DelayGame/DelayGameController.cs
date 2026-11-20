namespace WaveShooter.Features.Timer.DelayGame
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.PauseGame;

    /// <summary>
    /// Контроллер задержки начала игры.
    /// </summary>
    [RequireComponent(typeof(TimerCountdown))]
    public sealed class DelayGameController : ProviderCharacter
    {
        /// <summary>
        /// Время задержки изменилось.
        /// </summary>
        public event Action<int> onDelaySecondsChange = delegate { };

        /// <summary>
        /// Задержка кончилась.
        /// </summary>
        public event Action onDelayEnd = delegate { };

        /// <summary>
        /// Время задержки.
        /// </summary>
        public int DelaySeconds => delaySeconds;

        [SerializeField, Range(0, 10)] private int delaySeconds = 3;
        [SerializeField] private PauseController pauseController = default;

        private TimerCountdown timer = default;

        private Coroutine coroutine = default;

        protected override void Awake()
        {
            base.Awake();

            timer = GetComponent<TimerCountdown>();
            timer.enabled = false;
        }
        private void OnEnable()
        {
            StopDelay();
            LaunchDelay();
        }
        private void OnDisable() => StopDelay();
        
        protected override void OnFacadeInited() => pauseController.SetPause(true);

        /// <summary>
        /// Запуск задержки.
        /// </summary>
        public void LaunchDelay() => coroutine = StartCoroutine(Delay());

        /// <summary>
        /// Стопаем задержку.
        /// </summary>
        public void StopDelay()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator Delay()
        {
            float elapsed = 0f;

            while (delaySeconds > 0) 
            {
                elapsed += Time.unscaledDeltaTime; 

                if (elapsed >= 1f)
                {
                    elapsed -= 1f;
                    delaySeconds--;
                    if (delaySeconds < 0) delaySeconds = 0; 

                    onDelaySecondsChange.Invoke(delaySeconds);
                }
                yield return null;
            }
            pauseController.SetPause(false);
            yield return new WaitForSeconds(0.5f);
            
            onDelayEnd.Invoke();
            timer.enabled = true;
        }
    }
}