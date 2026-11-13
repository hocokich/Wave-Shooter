namespace WaveShooter.Features.MoveCharacter
{
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контроллер движения персонажа.
    /// </summary>
    public class MoveController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Направление движения.
        /// </summary>
        public Vector2 MoveDirection => moveDirection;

        [SerializeField] protected float speed = 5f;
        [SerializeField] protected Rigidbody body = default;

        protected Vector2 moveDirection = Vector2.zero;

        /// <summary>
        /// Задать направление движения.
        /// </summary>
        public void SetMoveDirection(Vector2 direction)
        {
            moveDirection = Vector2.ClampMagnitude(direction, 1f);
        }

        protected override void OnInited()
        {
            if (body == null)
                body = GetComponent<Rigidbody>();

            if (body == null)
                Debug.LogError("MoveController: Rigidbody не найден");
        }

        protected override void OnDisposed()
        {
            moveDirection = Vector2.zero;
        }

        protected virtual void FixedUpdate()
        {
            if (body == null) return;

            Vector3 delta = new Vector3(moveDirection.x, 0f, moveDirection.y) * (speed * Time.fixedDeltaTime);

            body.MovePosition(body.position + delta);
        }
    }
}