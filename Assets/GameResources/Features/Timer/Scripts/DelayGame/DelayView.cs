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

            delayGame.onDelaySecondsChange += UpdateView;
            delayGame.onDelayEnd += DisableView;

            UpdateView(delayGame.DelaySeconds);
        }
        private void OnEnable()
        {
            if(delayGame == null) return;

            delayGame.onDelaySecondsChange += UpdateView;
            delayGame.onDelayEnd += DisableView;

            UpdateView(delayGame.DelaySeconds);
        }
        private void OnDisable()
        {
            if(delayGame == null) return;

            delayGame.onDelaySecondsChange -= UpdateView;
            delayGame.onDelayEnd -= DisableView;
            delayGame = null;
        }
        
        private void UpdateView(int sec) => textView.text = $"{sec}";
        private void DisableView() => gameObject.SetActive(false);
    }
}