namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.GameState;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Обрабатывает ввод паузы.
    /// </summary>
    public sealed class PauseController : ProviderCharacter
    {
        [SerializeField] private GameStateModel stateModel = default;

        private InputActions inputActions;

        protected override void OnFacadeInited()
        {
            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();

            if (inputSys == null || inputSys.InputActions == null)
            {
                Debug.LogError("PauseController: не удалось получить InputActions.");
                return;
            }

            inputActions = inputSys.InputActions;
            inputActions.Gameplay.Pause.performed += OnPausePressed;
            inputActions.UI.Pause.performed += OnPausePressed;
        }
        protected override void OnFacadeDisposed() => Unsubscribe();

        protected override void OnDestroy()
        {
            Unsubscribe();
            base.OnDestroy();
        }
        private void Unsubscribe()
        {
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.performed -= OnPausePressed;
                inputActions.UI.Pause.performed -= OnPausePressed;
                inputActions = null;
            }
        }

        /// <summary>
        /// Обрабатывает запрос паузы с учетом текущего состояния.
        /// </summary>
        public void TogglePause()
        {
            switch (stateModel.CurrentState)
            {
                case GameState.Gameplay:
                    stateModel.SetState(GameState.Pause);
                    break;
                case GameState.Pause:
                    stateModel.SetState(GameState.DelayGameplay);
                    break;
            }
        }

        private void OnPausePressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            TogglePause();
        }
    }
}