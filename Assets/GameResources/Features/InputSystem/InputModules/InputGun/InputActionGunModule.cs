namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.Shooting;

    /// <summary>
    /// Модуль InputAction связанный с оружием.
    /// </summary>
    public abstract class InputActionGunModule : InputActionModule
    {
        protected GunController gunController = default;

        protected override void OnInited()
        {
            base.OnInited();

            action = GetInputAction(ConcreteFacade.GetModuleFacade<InputSystemModule>());
            gunController = ConcreteFacade.GetModuleFacade<GunController>();

            if(action == null || gunController == null)
            {
                Debug.LogError("action или gunController = null");
                return;
            }

            action.performed += OnAction;

            if (isActiveAndEnabled)
                action.Enable();
        }
        protected override void OnDisposed()
        {
            if(action != null)
            {
                action.Disable();
                action.performed -= OnAction;
                action = null;
            }
            
            base.OnDisposed();
        }

        protected abstract InputAction GetInputAction(InputSystemModule inputSystemModule);
    }
}