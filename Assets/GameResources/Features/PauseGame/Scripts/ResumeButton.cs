namespace WaveShooter.Features.PauseGame
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Кнопка, которая делает то же самое, что и нажатие Esc (снимает/ставит паузу).
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class ResumeButton : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager = default;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }
        private void OnDisable()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnButtonClicked);
            }
        }

        private void OnButtonClicked()
        {
            if (pauseManager != null)
            {
                pauseManager.TogglePause(); 
            }

        }
    }
}