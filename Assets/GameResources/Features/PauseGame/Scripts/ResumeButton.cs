namespace WaveShooter.Features.PauseGame
{
    using WaveShooter.Features.UIElements.Buttons;

    /// <summary>
    /// Кнопка, которая делает то же самое, что и нажатие Esc.
    /// </summary>
    public sealed class ResumeButton : AbstractButtonView
    {
        private PauseController pauseController;

        /// <summary>
        /// Вызывается из TogglePauseMenu
        /// </summary>
        /// <param name="manager"></param>
        public void Initialize(PauseController manager)
        {
            pauseController = manager;
        }

        protected override void OnClick()
        {
            if (pauseController != null)
            {
                pauseController.TogglePause(); 
            }
        }
    }
}