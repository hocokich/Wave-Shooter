namespace WaveShooter.Features.Spawners.Physics
{
    using UnityEngine;
    using WaveShooter.Features.Enemy;

    /// <summary>
    /// Пуля ломается при попадании по врагу.
    /// </summary>
    [RequireComponent(typeof(Bullet))]
    public class DestroyBullet : AbstractGenericTrigger<EnemyHealth>
    {
        private int damage = default;
        private Bullet bullet = default;
        
        private void Awake()
        {
            bullet = GetComponent<Bullet>();

            damage = bullet.Damage;
        }
        protected override void OnTriggerEnterSuccessfully(EnemyHealth enemyHealth)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
