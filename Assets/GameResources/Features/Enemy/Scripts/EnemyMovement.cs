namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Движение врага к игроку по физике.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
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

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.isKinematic = true;
            rb.useGravity = false;
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
        }

        protected virtual void FixedUpdate()
        {
            if (playerTarget == null) return;

            Vector3 direction = playerTarget.position - transform.position;
            
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Vector3 moveStep = direction.normalized * (currentSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + moveStep);

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
            }
        }

        protected virtual void OnDisable()
        {
            playerTarget = null;
        }
    }
}