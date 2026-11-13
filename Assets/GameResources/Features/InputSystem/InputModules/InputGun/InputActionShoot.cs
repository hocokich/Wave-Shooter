namespace WaveShooter.Features.Input.InputModules
{
    using UnityEngine.InputSystem;

    /// <summary>
    /// Модуль события выстрела.
    /// </summary>
    public class InputActionShoot : InputActionGunModule
    {
        protected override InputAction GetInputAction(InputSystemModule inputSystemModule)
        {
            return inputSystemModule.InputActions.Gameplay.Shoot;
        }

        protected override void OnAction(InputAction.CallbackContext context)
        {
            gunController.TryShoot();
        }
    }
}