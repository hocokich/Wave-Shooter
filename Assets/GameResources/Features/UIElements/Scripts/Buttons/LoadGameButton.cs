namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;
    using WaveShooter.Features.SaveSystem;

    /// <summary>
    /// Кнопка загрузки сохраненной игры.
    /// </summary>
    public class LoadGameButton : SwitchSceneButton 
    {
        protected override void Awake()
        {
            base.Awake();
            
            if (!SaveManager.HasSave())
            {
                btn.interactable = false;
            }
        }
        protected override void OnClick() 
        {
            PlayerPrefs.SetInt("LoadGame", 1);
            base.OnClick();
        }
    }
}