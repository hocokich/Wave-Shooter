namespace WaveShooter.Features.PauseGame
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Контроллер паузы, использующий контейнер для доступа к инпуту.
    /// </summary>
    public sealed class PauseController : ProviderCharacter
    {
        /// <summary>
        /// Событие что пауза изменилась.
        /// </summary>
        public event Action<bool> onIsPausedChange = delegate { };

        /// <summary>
        /// Пауза да/нет.
        /// </summary>
        public bool IsPaused
        {
            get => isPaused;
            set
            {
                if(isPaused != value)
                {
                    isPaused = value;
                    onIsPausedChange.Invoke(isPaused);
                }
            }
        }
        private InputActions inputActions;
        private bool isPaused = false;



        protected override void OnFacadeInited()
        {
            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();
            
            if (inputSys == null || inputSys.InputActions == null)
            {
                Debug.LogError("PauseManager: Не удалось получить InputActions из контейнера.");
                return;
            }

            inputActions = inputSys.InputActions;

            inputActions.Gameplay.Pause.performed += OnPausePressed;
            inputActions.UI.Pause.performed += OnPausePressed;
        }
        protected override void OnFacadeDisposed()
        {
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.performed -= OnPausePressed;
                inputActions.UI.Pause.performed -= OnPausePressed;
                inputActions = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.performed -= OnPausePressed;
                inputActions.UI.Pause.performed -= OnPausePressed;
            }
        }

        /// <summary>
        /// Переключает паузу.
        /// </summary>
        public void TogglePause()
        {
            SetPause(!IsPaused);
        }

        /// <summary>
        /// Жестко задает состояние паузы.
        /// </summary>
        public void SetPause(bool pauseState)
        {
            if (IsPaused == pauseState) return;

            IsPaused = pauseState;

            if (IsPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (inputActions != null)
                {
                    inputActions.Gameplay.Disable();
                    inputActions.UI.Enable();
                }
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (inputActions != null)
                {
                    inputActions.UI.Disable();
                    inputActions.Gameplay.Enable();
                }
            }
        }

        /// <summary>
        /// Отключает возможность ставить паузу (для Game Over).
        /// </summary>
        public void DisablePauseInput()
        {
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.Disable();
                inputActions.UI.Pause.Disable();
            }
        }

        private void OnPausePressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            TogglePause();
        }
    }
}