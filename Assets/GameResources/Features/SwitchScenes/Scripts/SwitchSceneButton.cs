namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Кнопка переключения сцен.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SwitchSceneButton : MonoBehaviour
    {
        [SerializeField] protected string newSceneName = "Game";
        protected Button btn;


        protected virtual void Awake()
        {
            if(btn == null)
                btn = GetComponent<Button>();
        }

        protected virtual void OnEnable()  => btn.onClick.AddListener(OnClick);
        protected virtual void OnDisable() => btn.onClick.RemoveListener(OnClick);

        protected virtual void OnClick()
        {
            SceneFlow.Load(newSceneName);
        }
    }
}