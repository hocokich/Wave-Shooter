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
        
        [SerializeField] protected float seconds = 60f;
        protected Coroutine coroutine = default;

        protected virtual void Start()
        {
            coroutine = StartCoroutine(Countdown());
        }

        protected IEnumerator Countdown()
        {
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
            onTimerIsDone.Invoke();
        }
    }
}