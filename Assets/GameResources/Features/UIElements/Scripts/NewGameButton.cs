namespace WaveShooter.Features.UIElements
{
    using UnityEngine;
    using WaveShooter.Features.SaveSystem;
    using WaveShooter.Features.SwitchScenes;

    /// <summary>
    /// Кнопка начала новой игры.
    /// </summary>
    public class NewGameButton : SwitchSceneButton {
        protected override void OnClick() {
            SaveManager.DeleteSave();
            PlayerPrefs.SetInt("LoadGame", 0);
            base.OnClick();
        }
    }
}