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
        /// Пауза изменилась (true — включили, false — выключили).
        /// </summary>
        public event Action<bool> onIsPausedChange = delegate { };

        /// <summary>
        /// Запрос на снятие паузы (Esc / кнопка Resume).
        /// Если есть подписчик — он ОБЯЗАН сам вызвать <see cref="SetPause"/>(false)
        /// (например, после обратного отсчёта). Если подписчиков нет — пауза снимается сразу.
        /// </summary>
        public event Action onResumeRequested;

        public bool IsPaused { get; private set; }
        public bool IsInputBlocked { get; private set; }

        private InputActions inputActions;

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

            // Применяем текущее состояние (на случай, если DelayGame уже вызвал SetInputBlocked до нас).
            ApplyInputState();
        }

        protected override void OnFacadeDisposed()
        {
            Unsubscribe();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Unsubscribe();
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
        /// Переключает паузу.
        /// </summary>
        public void TogglePause()
        {
            if (!IsPaused)
            {
                SetPause(true);
                return;
            }

            // Просят снять паузу — отдаём решение подписчикам (например, отсчёту),
            // иначе снимаем сразу.
            if (onResumeRequested != null)
                onResumeRequested.Invoke();
            else
                SetPause(false);
        }

        /// <summary>
        /// Жестко задаёт состояние паузы.
        /// </summary>
        public void SetPause(bool pauseState)
        {
            if (IsPaused == pauseState) return;

            IsPaused = pauseState;
            onIsPausedChange.Invoke(IsPaused);
            ApplyInputState();
        }

        /// <summary>
        /// Полный блок ввода без показа меню паузы (используется во время обратного отсчёта).
        /// </summary>
        public void SetInputBlocked(bool blocked)
        {
            if (IsInputBlocked == blocked) return;

            IsInputBlocked = blocked;
            ApplyInputState();
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

        private void ApplyInputState()
        {
            if (inputActions == null) return;

            if (IsInputBlocked)
            {
                // Полный блок: время стоит, ввод полностью выключен, меню паузы скрыто.
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inputActions.Gameplay.Disable();
                inputActions.UI.Disable();
                return;
            }

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