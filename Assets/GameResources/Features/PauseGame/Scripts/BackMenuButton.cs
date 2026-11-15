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
        [SerializeField] private SaveGameController saveController;

        protected override void Awake()
        {
            base.Awake();
            saveController = FindAnyObjectByType<SaveGameController>();
        }

        protected override void OnClick()
        {
            saveController?.SaveGame();
            Time.timeScale = 1f;
            base.OnClick();
        }
    }
}