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
        /// Начала атаку.
        /// </summary>
        public event Action onAttackStarted = delegate { };

        /// <summary>
        /// Закончил атаку.
        /// </summary>
        public event Action onAttackEnded = delegate { };

        [SerializeField, Range(1, 100)] protected int damage = 5;
        [SerializeField, Min(0.1f)] protected float cooldown = 1f;

        private HealthController playerHealth;
        private Coroutine attackRoutine;
        
        private float cooldownTimer = 0f; 

        protected override void OnTriggerEnterSuccessfully(HealthController healthController)
        {
            playerHealth = healthController;

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
                if (cooldownTimer <= 0f)
                {
                    playerHealth.TakeDamage(damage);
                    cooldownTimer = cooldown;
                }
                
                if (cooldownTimer > 0f)
                {
                    cooldownTimer -= Time.deltaTime;
                }

                yield return null; 
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

        private void OnDisable()
        {
            StopAttack();
        }
    }
}