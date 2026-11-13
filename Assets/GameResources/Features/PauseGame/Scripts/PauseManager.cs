namespace WaveShooter.Features.PauseGame
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Менеджер паузы, использующий контейнер для доступа к инпуту.
    /// </summary>
    public sealed class PauseManager : ProviderCharacter
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

        protected override void OnDestroy()
        {
            base.OnDestroy(); // Отписываемся от событий контейнера
            
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.performed -= OnPausePressed;
                inputActions.UI.Pause.performed -= OnPausePressed;
            }
        }

        protected override void OnFacadeInited()
        {
            // Берем InputSystemModule прямо из фасада внутри контейнера
            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();
            
            if (inputSys == null || inputSys.InputActions == null)
            {
                Debug.LogError("PauseManager: Не удалось получить InputActions из контейнера.");
                return;
            }

            inputActions = inputSys.InputActions;

            // Подписываемся на обе карты
            inputActions.Gameplay.Pause.performed += OnPausePressed;
            inputActions.UI.Pause.performed += OnPausePressed;
        }
        protected override void OnFacadeDisposed()
        {
            // Отписываемся, когда контейнер уничтожается (например, при смене сцены)
            if (inputActions != null)
            {
                inputActions.Gameplay.Pause.performed -= OnPausePressed;
                inputActions.UI.Pause.performed -= OnPausePressed;
                inputActions = null;
            }
        }

        /// <summary>
        /// Вкл/Выкл. паузу.
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused; 

            if (IsPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                inputActions.Gameplay.Disable();
                inputActions.UI.Enable();
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                inputActions.UI.Disable();
                inputActions.Gameplay.Enable();
            }
        }

        private void OnPausePressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            TogglePause();
        }
    }
}