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
        public event Action onDie = delegate { };
        [SerializeField] private HealthModel healthModel = default;
        public HealthModel Model => healthModel;

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

            healthModel.ApplyDamage(amount);
            
            if (healthModel.CurrentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Вылечить игрока (аптечка).
        /// </summary>
        public void Heal(int amount)
        {
            healthModel?.ApplyHeal(amount);
        }

        private void Die()
        {
            onDie.Invoke();
        }
    }
}