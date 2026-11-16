namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.SaveSystem;

    /// <summary>
    /// Вкл/выкл меню паузы.
    /// </summary>
    public sealed class TogglePauseMenu : ProviderCharacter
    {
        /// <summary>
        /// Вызывается из ScreenController при создании окна.
        /// </summary>
        public void Initialize(PauseManager pauseManager, SaveGameController saveController)
        {
            var resumeButton = GetComponentInChildren<ResumeButton>(true);
            resumeButton.Initialize(pauseManager);
        }

        protected override void OnFacadeInited() { }
        protected override void OnFacadeDisposed() { }
    }
}