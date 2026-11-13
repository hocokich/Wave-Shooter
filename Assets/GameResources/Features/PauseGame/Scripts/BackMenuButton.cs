namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using WaveShooter.Features.SwitchScenes;

    /// <summary>
    /// Кнопка возвращающая в главное меню во время паузы.
    /// </summary>
    public class BackMenuButton : SwitchSceneButton
    {
        protected override void OnClick()
        {
            Time.timeScale = 1f;
            base.OnClick();
        }
    }
}