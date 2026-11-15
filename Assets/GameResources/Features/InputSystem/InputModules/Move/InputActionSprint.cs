namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine.InputSystem;
    using WaveShooter.Features.MoveCharacter;

    /// <summary>
    /// Инупут бега
    /// </summary>
    public class InputActionSprint : InputActionModule
    {
        private MoveController moveController;

        protected override void OnInited()
        {
            base.OnInited();

            moveController = ConcreteFacade.GetModuleFacade<MoveController>();
            var inputSys = ConcreteFacade.GetModuleFacade<InputSystemModule>();

            if (moveController == null || inputSys == null) return;

            action = inputSys.InputActions.Gameplay.Sprint;

            if (action != null)
            {
                action.started += OnActionStarted;
                action.canceled += OnActionCanceled;

                if (isActiveAndEnabled)
                    action.Enable();
            }
        }

        protected override void OnDisposed()
        {
            if (action != null)
            {
                action.Disable();
                action.started -= OnActionStarted;
                action.canceled -= OnActionCanceled;
                action = null;
            }

            base.OnDisposed();
        }

        protected void OnActionStarted(InputAction.CallbackContext context)
        {
            moveController.SetSprint(true);
        }
        protected void OnActionCanceled(InputAction.CallbackContext context)
        {
            moveController.SetSprint(false);
        }
        protected override void OnAction(InputAction.CallbackContext context) { }
    }
}