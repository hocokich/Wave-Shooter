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
        [SerializeField] protected EnemyHealth enemyHealth = default;

        protected void Start()
        {
            if(timerCountdown == null)
                timerCountdown = FindAnyObjectByType<TimerCountdown>();
            
            if(enemyHealth == null)
            {
                enemyHealth = GetComponent<EnemyHealth>();
                enemyHealth.onDied += IncreaseSeconds;  
            }
        }

        protected virtual void OnEnable()
        {
            if(enemyHealth != null)
            {
                enemyHealth.onDied += IncreaseSeconds;
            }
        }
        protected virtual void OnDisable()
        {
            if(enemyHealth != null)
            {
                enemyHealth.onDied -= IncreaseSeconds;
            }
        }
        protected virtual void IncreaseSeconds()
        {
            timerCountdown.Seconds += seconds;
            onIncreseSeconds.Invoke();
        }
    }
}
