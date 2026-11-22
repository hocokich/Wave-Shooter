namespace WaveShooter.Features.GameState
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Контроллер состояний.
    /// Применяет к сцене настройки, соответствующие состоянию.
    /// </summary>
    public sealed class GameStateController : ProviderCharacter
    {
        [SerializeField] private GameStateModel stateModel = default;

        private InputActions inputActions;

        protected override void Awake()
        {
            stateModel.ResetState();
            base.Awake();
        }
        
        protected override void OnFacadeInited()
        {
            stateModel.SetState(GameState.DelayGameplay);

            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();

            if (inputSys == null || inputSys.InputActions == null)
            {
                Debug.LogError("GameStateApplier: не удалось получить InputActions.");
                return;
            }

            inputActions = inputSys.InputActions;
            stateModel.onStateChanged += Apply;
            Apply(stateModel.CurrentState);
        }
        protected override void OnFacadeDisposed()
        {
            if (stateModel != null)
                stateModel.onStateChanged -= Apply;
        }

        private void Apply(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    ApplyGameplay();
                    break;

                case GameState.Pause:
                case GameState.GameOver:
                    ApplyMenuLike();
                    break;

                default:
                    ApplyBlocked();
                    break;
            }
        }
        private void ApplyGameplay()
        {
            Time.timeScale = 1f;
            SetCursor(false);
            inputActions?.UI.Disable();
            inputActions?.Gameplay.Enable();
        }
        private void ApplyMenuLike()
        {
            Time.timeScale = 0f;
            SetCursor(true);
            inputActions?.Gameplay.Disable();
            inputActions?.UI.Enable();
        }
        private void ApplyBlocked()
        {
            Time.timeScale = 0f;
            SetCursor(true);
            inputActions?.Gameplay.Disable();
            inputActions?.UI.Disable();
        }
        private static void SetCursor(bool visible)
        {
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = visible;
        }
    }
}