namespace WaveShooter.Features.Input.InputModules
{
    using WaveShooter.Features.Facade;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Модуль InputAction события.
    /// </summary>
    public abstract class InputActionModule : GenericModuleFacade<CharacterFacade>
    {
        protected InputAction action;

        protected virtual void OnEnable() => action?.Enable();
        protected virtual void OnDisable() => action?.Disable();

        protected abstract void OnAction(InputAction.CallbackContext context);
    }
}