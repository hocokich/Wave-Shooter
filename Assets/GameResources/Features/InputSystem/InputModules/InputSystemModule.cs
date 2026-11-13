namespace WaveShooter.Features.Input.InputModules
{
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Модуль инициализации инпут системы.
    /// </summary>
    public class InputSystemModule : ModuleFacade
    {
        /// <summary>
        /// Система инпута.
        /// </summary>
        public InputActions InputActions { get; private set;}

        /// <summary>
        /// Присваем InputActions.
        /// </summary>
        public override void OnModuleInited()
        {
            InputActions = new InputActions();

            if (isActiveAndEnabled)
                InputActions.Gameplay.Enable();
        }

        /// <summary>
        /// Затираем InputActions.
        /// </summary>
        public override void OnModuleDisposed()
        {
            if(InputActions != null)
            {
                InputActions.Gameplay.Disable();
                InputActions.Dispose();
                InputActions = null;
            }
        }

        protected virtual void OnEnable() => InputActions?.Gameplay.Enable();
        protected virtual void OnDisable() => InputActions?.Gameplay.Disable();
        protected virtual void OnDestroy()
        {
            InputActions?.Dispose();
            InputActions = null;
        }
    }
}