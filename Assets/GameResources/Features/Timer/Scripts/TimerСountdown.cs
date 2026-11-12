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
        /// Событие что таймер уменьшился на 1 с.
        /// </summary>
        public event Action<float> onTimeDecrese = delegate {};

        /// <summary>
        /// С какого времени начинается отсчет.
        /// </summary>
        public float Seconds => seconds;
        
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