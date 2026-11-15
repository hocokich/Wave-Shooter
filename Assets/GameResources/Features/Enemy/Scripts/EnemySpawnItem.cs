namespace WaveShooter.Features.Enemy
{
    using UnityEngine;
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Префаб врага.
    /// </summary>
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemySpawnItem : SpawnItem
    {
        public override void Launch()
        {
            cooldown = float.MaxValue;

            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth.Heal(enemyHealth.MaxHealth);
            
            base.Launch();
        }
    }
}