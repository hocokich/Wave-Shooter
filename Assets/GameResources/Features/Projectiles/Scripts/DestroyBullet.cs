namespace WaveShooter.Features.Spawners.Physics
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Enemy;

    /// <summary>
    /// Пуля ломается при попадании по врагу.
    /// </summary>
    [RequireComponent(typeof(Bullet))]
    public sealed class DestroyBullet : AbstractGenericTrigger<EnemyHealth>
    {
        /// <summary>
        /// Взрыв
        /// </summary>
        public event Action OnExpolde = delegate { };

        private Bullet bullet = default;
        private int damage = default;
        
        private void Awake()
        {
            bullet = GetComponent<Bullet>();
            damage = bullet.Damage;
        }

        protected override void OnTriggerEnterSuccessfully(EnemyHealth enemyHealth)
        {
            OnExpolde.Invoke();

            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
