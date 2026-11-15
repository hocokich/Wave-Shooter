namespace WaveShooter.Features.Enemy
{
    using System;
    using System.Collections;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Движение врага к игроку по физике.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EnemyAttack))]
    public class EnemyMovement : MonoBehaviour
    {
        /// <summary>
        /// Текущая скорость.
        /// </summary>
        public float CurrentSpeed
        {
            get => currentSpeed;
            set
            {
                if(currentSpeed != value)
                {
                    currentSpeed = value;
                    onSpeedChanged.Invoke(currentSpeed);
                }
            }
        }

        /// <summary>
        /// Минимальная скорость.
        /// </summary>
        public float MinSpeed => minSpeed;

        /// <summary>
        /// Максимальная скорость.
        /// </summary>
        public float MaxSpeed => maxSpeed;

        /// <summary>
        /// Скорость задана.
        /// </summary>
        public event Action<float> onSpeedChanged = delegate { };

        [SerializeField] protected float minSpeed = 1f;
        [SerializeField] protected float maxSpeed = 3.5f;
        [SerializeField] protected float rotationSpeed = 10f;

        protected Rigidbody rb;
        protected Transform playerTarget;
        [SerializeField] protected float currentSpeed;

        protected EnemyAttack enemyAttack;
        protected Coroutine moveRoutine;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            enemyAttack = GetComponent<EnemyAttack>();

            enemyAttack.onAttackStarted += StopMove;
            enemyAttack.onAttackEnded += StartMove;

        }
        protected virtual void OnEnable()
        {
            enemyAttack.onAttackStarted += StopMove;
            enemyAttack.onAttackEnded += StartMove;
        }
        protected virtual void OnDisable()
        {
            playerTarget = null;
            
            enemyAttack.onAttackStarted -= StopMove;
            enemyAttack.onAttackEnded -= StartMove;

            StopMove();
        }

        /// <summary>
        /// Вызывается спавнером при появлении врага.
        /// </summary>
        public virtual void Initialize(Transform targetTransform)
        {
            playerTarget = targetTransform;
            
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            CurrentSpeed = currentSpeed;

            onSpeedChanged.Invoke(currentSpeed);

            StartMove();
        }

        protected virtual void StartMove()
        {
            // Если уже движемся или нет цели — ничего не делаем
            if (moveRoutine != null || playerTarget == null || !gameObject.activeInHierarchy) 
                return;

            moveRoutine = StartCoroutine(MoveRoutine());
        }
        protected virtual void StopMove()
        {
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
            // Останавливаем физику, чтобы враг не скользил
            if (rb != null) rb.linearVelocity = Vector3.zero;
        }
        protected virtual IEnumerator MoveRoutine()
        {
            // Работаем пока есть цель
            while (playerTarget != null)
            {
                Vector3 direction = playerTarget.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    Vector3 moveStep = direction.normalized * (currentSpeed * Time.fixedDeltaTime);
                    rb.MovePosition(rb.position + moveStep);

                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
                }

                // Ждем следующий физический кадр
                yield return new WaitForFixedUpdate();
            }
            
            moveRoutine = null;
        }


    }
}