namespace WaveShooter.Features.Health
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контроллер здоровья игрока.
    /// </summary>
    public sealed class HealthController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Получил урон.
        /// </summary>
        public event Action onDamaged = delegate { };

        /// <summary>
        /// Вылечился.
        /// </summary>
        public event Action onHealed = delegate { };

        /// <summary>
        /// Игрок умер.
        /// </summary>
        public event Action onDied = delegate { };

        /// <summary>
        /// Дата здоровья.
        /// </summary>
        public HealthModel Model => healthModel;

        [SerializeField] private HealthModel healthModel = default;

        protected override void OnInited()
        {
            if (healthModel == null)
            {
                healthModel = ConcreteFacade.GetDataModule<HealthModel>();
            }
        }
        protected override void OnDisposed()
        {
            // Очистка, если нужна
        }

        /// <summary>
        /// Нанести урон игроку (например, при столкновении с врагом).
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (healthModel == null) return;

            int currentHealth = healthModel.CurrentHealth - amount;
            healthModel.CurrentHealth = Mathf.Max(0, currentHealth);
            
            if (healthModel.CurrentHealth <= 0)
            {
                Die();
                return;
            }

            onDamaged.Invoke();
        }

        /// <summary>
        /// Вылечить игрока
        /// </summary>
        public void Heal(int amount)
        {
            if (healthModel.CurrentHealth <= 0) return;

            int currentHealth = healthModel.CurrentHealth + amount;
            healthModel.CurrentHealth = Mathf.Min(currentHealth, healthModel.MaxHealth);

            onHealed.Invoke();
        }

        private void Die()
        {
            onDied.Invoke();
        }
    }
}