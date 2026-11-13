namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Spawners;
    using WaveShooter.Features.Spawners.Physics;

    /// <summary>
    /// Здоровье врага.
    /// </summary>
    public class EnemyHealth : AbstractGenericTrigger<Bullet>
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

        [SerializeField] protected int maxHealth = 100;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        protected override void OnTriggerEnterSuccessfully(Bullet bullet)
        {
            CurrentHealth -= bullet.Damage;
            CurrentHealth = Mathf.Max(0, CurrentHealth);

            onHealthChanged.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0)
            {
                EnemyDie();
                base.OnTriggerEnterSuccessfully(bullet); 
            }
        }
        protected virtual void EnemyDie()
        {
            onDied.Invoke();
            Destroy(gameObject);
        }
    }
}