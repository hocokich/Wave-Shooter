namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Timer;

    /// <summary>
    /// Увеличивает время
    /// </summary>
    public class IncreseSeconds : MonoBehaviour
    {
        /// <summary>
        /// Время прибавилось.
        /// </summary>
        public event Action onIncreseSeconds = delegate { };

        [SerializeField] protected int seconds = 5;
        [SerializeField] protected TimerCountdown timerCountdown = default;
        [SerializeField] protected DestroyEnemy destroyEnemy = default;

        protected void Start()
        {
            if(timerCountdown == null)
                timerCountdown = FindAnyObjectByType<TimerCountdown>();
            
            if(destroyEnemy == null)
            {
                destroyEnemy = GetComponent<DestroyEnemy>();
                destroyEnemy.onDestroyEnemy += IncreaseSeconds;  
            }
        }

        protected virtual void OnEnable()
        {
            if(destroyEnemy != null)
            {
                destroyEnemy.onDestroyEnemy += IncreaseSeconds;
            }
        }
        protected virtual void OnDisable()
        {
            if(destroyEnemy != null)
            {
                destroyEnemy.onDestroyEnemy -= IncreaseSeconds;
            }
        }
        protected virtual void IncreaseSeconds()
        {
            timerCountdown.Seconds += 5;
            onIncreseSeconds.Invoke();
        }
    }
}
