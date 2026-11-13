namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Health;
    using WaveShooter.Features.Spawners.Physics;

    /// <summary>
    /// Атака врага.
    /// </summary>
    public class EnemyAttack : AbstractGenericTrigger<HealthController>
    {
        [SerializeField, Range(1, 100)] protected int damage = 5;

        protected override void OnTriggerEnterSuccessfully(HealthController HealthController)
        {
            HealthController.TakeDamage(damage);
        }
    }
}