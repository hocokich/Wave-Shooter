namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Класс переключения сцен.
    /// </summary>
    public class SwitchScene : MonoBehaviour
    {
        [SerializeField] protected string newSceneName = "Game";
        [SerializeField] protected Button btn;

        protected virtual void OnEnable()  => btn.onClick.AddListener(Loading);
        protected virtual void OnDisable() => btn.onClick.RemoveListener(Loading);

        protected virtual void Loading() => SceneFlow.Load(newSceneName);
    }
}