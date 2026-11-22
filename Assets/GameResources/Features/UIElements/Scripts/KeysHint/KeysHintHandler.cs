namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Вкл/выкл подсказок для управления.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class KeysHintHandler : ProviderCharacter
    {
        private InputActions inputActions = default;
        private Text hintText = default;

        protected override void OnFacadeInited()
        {
            hintText = GetComponent<Text>();

            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();

            if (inputSys == null || inputSys.InputActions == null)
            {
                Debug.LogError("Не удалось получить InputActions.");
                return;
            }

            inputActions = inputSys.InputActions;
            inputActions.Gameplay.Hints.performed += OnHintsPressed;
        }
        protected override void OnFacadeDisposed()
        {
            if (inputActions != null)
            {
                inputActions.Gameplay.Hints.performed -= OnHintsPressed;
                inputActions = null;
            }

            if (hintText != null) hintText = null;
        }

        /// <summary>
        /// Обрабатывает запрос подсказок с учетом текущего состояния.
        /// </summary>
        public void ToggleHints()
        {
            hintText.enabled = !hintText.enabled;
        }
        
        private void OnHintsPressed(InputAction.CallbackContext context)
        {
            ToggleHints();
        }
    }
}