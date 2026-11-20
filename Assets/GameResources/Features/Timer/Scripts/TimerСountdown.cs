namespace WaveShooter.Features.Timer
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Таймер обратного отсчета.
    /// </summary>
    public sealed class TimerCountdown : ProviderCharacter
    {
        /// <summary>
        /// Таймер запустился.
        /// </summary>
        public event Action onTimerStart = delegate {};

        /// <summary>
        /// Таймер закончился.
        /// </summary>
        public event Action onTimerDone = delegate {};

        /// <summary>
        /// Событие изменения таймера.
        /// </summary>
        public event Action<float> onTimeChanged = delegate {};

        /// <summary>
        /// Текущее время.
        /// </summary>
        public float Seconds
        {
            get => seconds;
            set
            {
                if(seconds != value)
                {
                    seconds = value;
                    onTimeChanged.Invoke(seconds);
                }
            }
        }
        
        [SerializeField] private float seconds = 60f;
        private Coroutine coroutine = default;

        private IEnumerator Countdown()
        {
            onTimerStart.Invoke();

            float elapsed = 0f;

            while (seconds > 0) 
            {
                elapsed += Time.deltaTime; 

                if (elapsed >= 1f)
                {
                    elapsed -= 1f;
                    seconds--;

                    if (seconds < 0) seconds = 0; 

                    onTimeChanged.Invoke(seconds);
                }
                yield return null;
            }
            onTimerDone.Invoke();
        }

        protected override void OnFacadeInited()
        {
            onTimerStart.Invoke();
            coroutine = StartCoroutine(Countdown());
        }
    }
}