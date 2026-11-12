namespace WaveShooter.Features.Timer
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Таймер обратного отсчета.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// Событие что таймер закончился.
        /// </summary>
        public event Action TimerIsDone = delegate {};

        /// <summary>
        /// Событие что таймер уменьшился на 1 с.
        /// </summary>
        public event Action TimeDecrese = delegate {};
        
        [SerializeField] protected float seconds = 60f;
        protected Coroutine coroutine;

        protected virtual void Start()
        {
            coroutine = StartCoroutine(EndTimer());
        }

        protected IEnumerator EndTimer()
        {
            float elapsed = 0f;

            while (elapsed < seconds) 
            { 
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / seconds);

                TimeDecrese.Invoke();

                yield return null;
            } 
            TimerIsDone.Invoke();
        }
    }
}