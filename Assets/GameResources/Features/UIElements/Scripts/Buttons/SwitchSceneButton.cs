namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;
    using WaveShooter.Features.SwitchScenes;

    /// <summary>
    /// Кнопка переключения сцен.
    /// </summary>
    public class SwitchSceneButton : AbstractButtonView
    {
        [SerializeField] protected string newSceneName = "Game";

        protected override void OnClick()
        {
            Time.timeScale = 1f;
            SceneFlow.Load(newSceneName);
        }
    }
}