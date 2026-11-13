namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.CamController;

    /// <summary>
    /// Модуль вращения камеры (мышь / правый стик).
    /// </summary>
    public class InputActionLook : InputActionModule
    {
        private LookController lookController;

        protected override void OnInited()
        {
            base.OnInited();

            lookController = ConcreteFacade.GetModuleFacade<LookController>();
            var inputSys = ConcreteFacade.GetModuleFacade<InputSystemModule>();

            if (inputSys == null || inputSys.InputActions == null || lookController == null)
            {
                Debug.LogError("Не удалось инициализировать InputActionLook");
                return;
            }

            action = inputSys.InputActions.Gameplay.Look;

            if (action != null)
            {
                action.performed += OnAction;
                action.canceled += OnCanceled;

                if (isActiveAndEnabled)
                    action.Enable();
            }
        }

        protected override void OnDisposed()
        {
            if (action != null)
            {
                action.Disable();
                action.performed -= OnAction;
                action.canceled -= OnCanceled;
                action = null;
            }

            base.OnDisposed();
        }

        protected override void OnAction(InputAction.CallbackContext context)
        {
            lookController.SetLookVector(context.ReadValue<Vector2>());
        }

        protected virtual void OnCanceled(InputAction.CallbackContext context)
        {
            lookController.SetLookVector(Vector2.zero);
        }
    }
}