namespace WaveShooter.Features.PauseGame
{
    using WaveShooter.Features.Container;

    /// <summary>
    /// Вкл/выкл меню паузы.
    /// </summary>
    public sealed class TogglePauseMenu : ProviderCharacter
    {
        /// <summary>
        /// Вызывается из ScreenController при создании окна.
        /// </summary>
        public void Initialize(PauseController pauseController)
        {
            var resumeButton = GetComponentInChildren<ResumeButton>(true);
            resumeButton.Initialize(pauseController);
        }

        protected override void OnFacadeInited() { }
        protected override void OnFacadeDisposed() { }
    }
}