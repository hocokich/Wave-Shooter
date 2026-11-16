namespace WaveShooter.Features.Timer
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;
    using WaveShooter.Features.PauseGame;

    /// <summary>
    /// Активирует экран конца игровой сессии.
    /// </summary>
    public class GameOverScreen : ProviderCharacter
    {
        public void Initialize(PauseController pauseController)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Container != null && Container.FacadeInstance != null)
            {
                var inputSys = Container.FacadeInstance.GetModuleFacade<InputSystemModule>();
                if (inputSys != null && inputSys.InputActions != null)
                {
                    inputSys.InputActions.Disable();
                }
            }
            
            if (pauseController != null)
            {
                pauseController.DisablePauseInput();
            }
        }

        protected override void OnFacadeInited() { }
        protected override void OnFacadeDisposed() { }
    }
}