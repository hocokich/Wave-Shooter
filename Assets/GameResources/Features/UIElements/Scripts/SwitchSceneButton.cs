namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;
    using WaveShooter.Features.UIElements;

    /// <summary>
    /// Кнопка переключения сцен.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SwitchSceneButton : AbstractButtonView
    {
        [SerializeField] protected string newSceneName = "Game";

        protected override void OnClick()
        {
            SceneFlow.Load(newSceneName);
        }
    }
}