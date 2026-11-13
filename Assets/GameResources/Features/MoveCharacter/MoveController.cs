namespace WaveShooter.Features.MoveCharacter
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контроллер движения персонажа.
    /// </summary>
    public class MoveController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Текущая скорость
        /// </summary>
        public float CurrentSpeed { get; private set; }

        [Header("Настройки скорости")]
        [SerializeField] protected float walkSpeed = 2f;
        [SerializeField] protected float sprintSpeed = 5f;

        private Vector2 moveVector = Vector2.zero;
        private Coroutine moveRoutine;

        protected override void OnInited()
        {
            CurrentSpeed = walkSpeed;
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
        public void SetSprint(bool isSprinting)
        {
            CurrentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        }

        /// <summary>
        /// 
        /// Вызывается из InputActionMove.
        /// </summary>
        public void SetMoveDirection(Vector2 vector)
        {
            moveVector = vector;

            if (moveVector.sqrMagnitude > 0.01f)
            {
                if (moveRoutine == null) 
                    moveRoutine = StartCoroutine(MoveRoutine());
            }
        }

        protected IEnumerator MoveRoutine()
        {
            while (moveVector.sqrMagnitude > 0.01f)
            {
                Vector3 direction = new Vector3(moveVector.x, 0, moveVector.y) * CurrentSpeed;
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