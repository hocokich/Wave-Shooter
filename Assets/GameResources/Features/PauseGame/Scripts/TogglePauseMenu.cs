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
            var resumeButtons = GetComponentsInChildren<ResumeButton>(true);
            foreach (var btn in resumeButtons)
            {
                btn.Initialize(pauseManager);
            }

            var backButtons = GetComponentsInChildren<BackMenuButton>(true);
            foreach (var btn in backButtons)
            {
                btn.Initialize(saveController);
            }
        }

        protected override void OnFacadeInited() { }
        protected override void OnFacadeDisposed() { }
    }
}