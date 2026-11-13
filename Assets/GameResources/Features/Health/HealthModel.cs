namespace WaveShooter.Features.Health
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Модель здоровья игрока.
    /// </summary>
    [CreateAssetMenu(fileName = "New Health Model", menuName = "WaveShooter/Features/HealthModel")]
    public class HealthModel : DataModuleFacade
    {
        /// <summary>
        /// Здоровье изменилось (текущее, максимальное).
        /// </summary>
        public event Action<int, int> onHealthChanged = delegate { };
        
        /// <summary>
        /// Игрок умер.
        /// </summary>
        public event Action onDied = delegate { };

        /// <summary>
        /// Максимум здоровья.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Текущее здоровье.
        /// </summary>
        public int CurrentHealth { get; private set; }

        [SerializeField] protected int maxHealth = 100;

        public override void OnModuleInited()
        {
            CurrentHealth = maxHealth;
            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }

        public void ApplyDamage(int amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Max(0, CurrentHealth);

            onHealthChanged.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0)
            {
                onDied.Invoke();
            }
        }

        public void ApplyHeal(int amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);

            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }
    }
}