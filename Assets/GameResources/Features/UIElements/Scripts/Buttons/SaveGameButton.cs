namespace WaveShooter.Features.UIElements.Buttons
{
    using WaveShooter.Features.SaveSystem;

    /// <summary>
    /// Кнопка сохранения игры.
    /// </summary>
    public sealed class SaveGameButton : AbstractButtonView 
    {
        private SaveGameController _saveGameController = default;

        public void Initialize(SaveGameController saveGameController)
        {
            _saveGameController = saveGameController;
        }

        protected override void OnClick() 
        {
            _saveGameController.SaveGame();
            base.OnClick();
        }
    }
}