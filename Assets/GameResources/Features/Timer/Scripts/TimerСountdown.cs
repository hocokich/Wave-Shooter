namespace WaveShooter.Features.Timer
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Таймер обратного отсчета.
    /// </summary>
    public class TimerCountdown : MonoBehaviour
    {
        /// <summary>
        /// Событие что таймер закончился.
        /// </summary>
        public event Action onTimerIsDone = delegate {};

        /// <summary>
        /// Событие что тика таймера.
        /// </summary>
        public event Action<float> onTimeDecrese = delegate {};

        /// <summary>
        /// Событие что таймер увеличился.
        /// </summary>
        public event Action onTimeIncrese = delegate {};

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
                    onTimeIncrese.Invoke();
                }
            }
        }
        
        [SerializeField] protected float seconds = 60f;
        protected Coroutine coroutine = default;

        protected virtual void Start()
        {
            coroutine = StartCoroutine(Countdown());
        }

        protected IEnumerator Countdown()
        {
            float elapsed = 0f;

            while (elapsed < seconds) 
            {
                seconds--;
                onTimeDecrese.Invoke(seconds);
                yield return new WaitForSeconds(1f);
            } 
            onTimerIsDone.Invoke();
        }
    }
}