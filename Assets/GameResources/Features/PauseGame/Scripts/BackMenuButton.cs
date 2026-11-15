namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using WaveShooter.Features.SaveSystem;
    using WaveShooter.Features.SwitchScenes;

    /// <summary>
    /// Кнопка возвращающая в главное меню во время паузы.
    /// </summary>
    public sealed class BackMenuButton : SwitchSceneButton
    {
        private SaveGameController saveController;

        /// <summary>
        /// Вызывается из TogglePauseMenu
        /// </summary>
        /// <param name="controller"></param>
        public void Initialize(SaveGameController controller)
        {
            saveController = controller;
        }

        protected override void OnClick()
        {
            saveController?.SaveGame();
            Time.timeScale = 1f;
            base.OnClick();
        }
    }
}