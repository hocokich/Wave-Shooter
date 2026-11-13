namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.MoveCharacter;

    /// <summary>
    /// Модуль передвижения движения.
    /// </summary>
    public class InputActionMove : InputActionModule
    {
        protected MoveController moveController = default;

        protected override void OnInited()
        {
            base.OnInited();

            moveController = ConcreteFacade.GetModuleFacade<MoveController>();
            action = ConcreteFacade
                .GetModuleFacade<InputSystemModule>()?
                .InputActions.Gameplay.Move;

            if (action == null || moveController == null)
            {
                Debug.LogError("action или moveController = null");
                return;
            }

            action.performed += OnAction;
            action.canceled  += OnCanceled;

            if (isActiveAndEnabled)
                action.Enable();
        }

        protected override void OnDisposed()
        {
            if (action != null)
            {
                action.Disable();
                action.performed -= OnAction;
                action.canceled  -= OnCanceled;
                action = null;
            }

            base.OnDisposed();
        }

        protected override void OnAction(InputAction.CallbackContext context)
        {
            moveController.SetMoveDirection(context.ReadValue<Vector2>());
        }

        protected virtual void OnCanceled(InputAction.CallbackContext context)
        {
            moveController.SetMoveDirection(Vector2.zero);
        }
    }
}