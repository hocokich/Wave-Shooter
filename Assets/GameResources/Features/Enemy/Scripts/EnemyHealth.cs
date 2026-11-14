namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Здоровье врага.
    /// </summary>
    public class EnemyHealth : MonoBehaviour
    {
        /// <summary>
        /// Текущее хп.
        /// </summary>
        public int CurrentHealth { get; private set; }

        /// <summary>
        /// Изменение хп.
        /// </summary>/
        public event Action<int, int> onHealthChanged = delegate { };

        /// <summary>
        /// Умер.
        /// </summary>
        public event Action onDied = delegate { };

        [SerializeField, Range(1, 100)] protected int maxHealth = 100;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(int amount)
        {
            int currentHealth = CurrentHealth - amount;
            CurrentHealth = Mathf.Max(0, currentHealth);

            if (CurrentHealth <= 0)
            {
                Die();
                return;
            }

            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }
        protected virtual void Die()
        {
            onDied.Invoke();
            Destroy(gameObject);
        }
    }
}