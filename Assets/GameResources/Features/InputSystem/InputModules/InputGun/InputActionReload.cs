namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine.InputSystem;

    /// <summary>
    /// Модуль перезарядки.
    /// </summary>
    public class InputActionReload : InputActionGunModule
    {
        protected override InputAction GetInputAction(InputSystemModule inputSystemModule)
        {
            return inputSystemModule.InputActions.Gameplay.Reload;
        }

        protected override void OnAction(InputAction.CallbackContext context)
        {
            gunController.Reload();
        }
    }
}