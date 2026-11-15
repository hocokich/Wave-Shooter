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
        /// Изменение хп.
        /// </summary>/
        public event Action<int, int> onHealthChanged = delegate { };

        /// <summary>
        /// Умер.
        /// </summary>
        public event Action onDied = delegate { };

        /// <summary>
        /// Текущее хп.
        /// </summary>
        public int CurrentHealth { get; private set; }

        /// <summary>
        /// Максимальное кол-во здоровья.
        /// </summary>
        public int MaxHealth => maxHealth;

        [SerializeField, Range(1, 100)] protected int maxHealth = 100;
        [SerializeField, Range(0, 100)] protected int currentHealth = 100;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        /// <summary>
        /// Получение урона.
        /// </summary>
        /// <param name="amount"></param>
        public virtual void TakeDamage(int amount)
        {
            currentHealth = CurrentHealth - amount;
            CurrentHealth = Mathf.Max(0, currentHealth);

            if (CurrentHealth <= 0)
            {
                Die();
                return;
            }

            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }

        /// <summary>
        /// Лечение.
        /// </summary>
        /// <param name="amount"></param>
        public virtual void Heal(int amount)
        {
            currentHealth = CurrentHealth + amount;
            CurrentHealth = Mathf.Min(currentHealth, maxHealth);

            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }
        protected virtual void Die()
        {
            onDied.Invoke();

            if (TryGetComponent(out EnemySpawnItem spawnItem))
            {
                spawnItem.Complete();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}