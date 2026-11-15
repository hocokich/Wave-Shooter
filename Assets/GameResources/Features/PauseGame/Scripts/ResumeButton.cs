namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using WaveShooter.Features.UIElements;

    /// <summary>
    /// Кнопка, которая делает то же самое, что и нажатие Esc.
    /// </summary>
    public sealed class ResumeButton : AbstractButtonView
    {
        private PauseManager pauseManager;

        /// <summary>
        /// Вызывается из TogglePauseMenu
        /// </summary>
        /// <param name="manager"></param>
        public void Initialize(PauseManager manager)
        {
            pauseManager = manager;
        }

        protected override void OnClick()
        {
            if (pauseManager != null)
            {
                pauseManager.TogglePause(); 
            }
        }
    }
}