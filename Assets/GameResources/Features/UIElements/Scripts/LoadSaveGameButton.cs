namespace WaveShooter.Features.UIElements
{
    using UnityEngine;
    using WaveShooter.Features.SaveSystem;
    using WaveShooter.Features.SwitchScenes;

    /// <summary>
    /// Кнопка загрузки сохраненной игры.
    /// </summary>
    public class LoadSaveGameButton : SwitchSceneButton 
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