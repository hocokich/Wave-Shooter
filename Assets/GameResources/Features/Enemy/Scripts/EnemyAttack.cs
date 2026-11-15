namespace WaveShooter.Features.Enemy
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Spawners.Physics;
    using WaveShooter.Features.HealthCharacter;

    /// <summary>
    /// Атака врага.
    /// </summary>
    public class EnemyAttack : AbstractGenericTrigger<HealthController>
    {
        /// <summary>
        /// Начал атаку (догнал игрока).
        /// </summary>
        public event Action onAttackStarted = delegate { };
        
        /// <summary>
        /// Закончил атаку (игрок отбежал).
        /// </summary>
        public event Action onAttackEnded = delegate { };

        [SerializeField, Range(1, 100)] protected int damage = 5;
        [SerializeField, Min(0.1f)] protected float cooldown = 1f;

        private HealthController playerHealth;
        private Coroutine attackRoutine;

        protected override void OnTriggerEnterSuccessfully(HealthController HealthController)
        {
            playerHealth = HealthController;

            if (attackRoutine == null)
            {
                onAttackStarted.Invoke();
                attackRoutine = StartCoroutine(AttackRoutine());
            }
        }

        protected override void OnTriggerExitSuccessfully(HealthController healthController)
        {
            StopAttack();
            onAttackEnded.Invoke();
        }

        private IEnumerator AttackRoutine()
        {
            while (playerHealth != null && playerHealth.Model.CurrentHealth > 0)
            {
                playerHealth.TakeDamage(damage);

                yield return new WaitForSeconds(cooldown);
            }
            attackRoutine = null;
            playerHealth = null;
        }

        private void StopAttack()
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
            playerHealth = null;
        }
    }
}