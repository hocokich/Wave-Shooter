namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;
    using WaveShooter.Features.UIElements;

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