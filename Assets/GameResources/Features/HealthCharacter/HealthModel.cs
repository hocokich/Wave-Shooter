namespace WaveShooter.Features.HealthCharacter
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Модель здоровья игрока.
    /// </summary>
    [CreateAssetMenu(fileName = "New Health Model", menuName = "WaveShooter/Features/Health/HealthModel")]
    public class HealthModel : DataModuleFacade
    {
        /// <summary>
        /// Здоровье изменилось (текущее, максимальное).
        /// </summary>
        public event Action<int, int> onHealthChanged = delegate { };

        /// <summary>
        /// Максимум здоровья.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Текущее здоровье.
        /// </summary>
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if(currentHealth != value)
                {
                    currentHealth = value;
                    onHealthChanged.Invoke(currentHealth, maxHealth);
                }
            }
        }

        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int currentHealth = 100;

        public override void OnModuleInited()
        {
            CurrentHealth = maxHealth;
            onHealthChanged.Invoke(CurrentHealth, maxHealth);
        }
    }
}