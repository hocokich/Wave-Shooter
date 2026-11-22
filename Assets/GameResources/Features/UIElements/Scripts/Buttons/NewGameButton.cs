namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;
    using WaveShooter.Features.SaveSystem;

    /// <summary>
    /// Кнопка начала новой игры.
    /// </summary>
    public class NewGameButton : SwitchSceneButton 
    {
        protected override void OnClick() 
        {
            SaveManager.DeleteSave();
            PlayerPrefs.SetInt("LoadGame", 0);
            base.OnClick();
        }
    }
}