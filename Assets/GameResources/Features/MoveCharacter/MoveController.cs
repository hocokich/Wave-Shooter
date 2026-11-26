namespace WaveShooter.Features.MoveCharacter
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контроллер движения персонажа.
    /// </summary>
    public class MoveController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Персонаж начал двигаться.
        /// </summary>
        public event Action onMoveStart = delegate { };

        /// <summary>
        /// Персонаж перестал двигаться.
        /// </summary>
        public event Action onMoveStop = delegate { };

        /// <summary>
        /// Состояние спринта изменилось. true — бег, false — ходьба.
        /// </summary>
        public event Action<bool> onSprintChanged = delegate { };

        /// <summary>
        /// Текущая скорость.
        /// </summary>
        public float CurrentSpeed { get; private set; }

        [Header("Настройки скорости")]
        [SerializeField] protected float walkSpeed = 2f;
        [SerializeField] protected float sprintSpeed = 5f;

        protected Vector2 moveVector = Vector2.zero;
        protected Transform characterBody;
        protected Coroutine moveRoutine;
        protected bool isSprinting;

        protected override void OnInited()
        {
            CurrentSpeed = walkSpeed;
            isSprinting = false;
            characterBody = ConcreteFacade.transform;
        }
        protected override void OnDisposed()
        {
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
            moveVector = Vector2.zero;
        }

        /// <summary>
        /// Вкл/Выкл спринт.
        /// Вызывается из InputActionSprint
        /// </summary>
        public void SetSprint(bool sprinting)
        {
            if (isSprinting == sprinting) return;

            isSprinting = sprinting;
            CurrentSpeed = isSprinting ? sprintSpeed : walkSpeed;
            onSprintChanged.Invoke(isSprinting);

            CurrentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        }

        /// <summary>
        /// Текущее направление движения.
        /// Вызывается из InputActionMove.
        /// </summary>
        public void SetMoveDirection(Vector2 vector)
        {
            bool wasMoving = moveVector.sqrMagnitude > 0.01f;
            moveVector = vector;
            bool isMoving = moveVector.sqrMagnitude > 0.01f;

            if (isMoving && !wasMoving)
                onMoveStart.Invoke();
            else if (!isMoving && wasMoving)
                onMoveStop.Invoke();

            if (isMoving && moveRoutine == null)
                moveRoutine = StartCoroutine(MoveRoutine());
        }

        protected IEnumerator MoveRoutine()
        {
            while (moveVector.sqrMagnitude > 0.01f)
            {
                Vector3 forward = characterBody.forward;
                Vector3 right = characterBody.right;

                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                Vector3 direction = (forward * moveVector.y + right * moveVector.x).normalized;
                direction *= CurrentSpeed;

                ChangeSpeed(direction);
                
                yield return new WaitForFixedUpdate(); 
            }
            moveRoutine = null;
        }
        protected void ChangeSpeed(Vector3 direction)
        {
            transform.position += direction * Time.fixedDeltaTime;
        }
    }
}