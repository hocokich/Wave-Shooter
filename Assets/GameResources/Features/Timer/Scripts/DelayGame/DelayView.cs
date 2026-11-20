namespace WaveShooter.Features.Timer.DelayGame
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Вью задержки.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class DelayView : MonoBehaviour
    {
        private DelayGameController delayGame = default;
        private Text textView = default;

        private void Awake()
        {
            delayGame = FindAnyObjectByType<DelayGameController>();
            textView = GetComponent<Text>();

            if (delayGame == null)
            {
                textView.enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            if (delayGame == null) return;

            delayGame.onDelaySecondsChange += UpdateView;
            delayGame.onDelayEnd += DisableView;

            if (delayGame.IsDelayActive)
                UpdateView(delayGame.DelaySeconds);
            else
                DisableView();
        }

        private void OnDisable()
        {
            if (delayGame == null) return;

            delayGame.onDelaySecondsChange -= UpdateView;
            delayGame.onDelayEnd -= DisableView;
        }

        private void UpdateView(int sec)
        {
            textView.enabled = true;
            textView.text = $"{sec}";
        }

        private void DisableView()
        {
            textView.enabled = false;
        }
    }
}