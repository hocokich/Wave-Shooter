namespace WaveShooter.Features.Timer
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Вью таймера.
    /// </summary>
    public class TimerView : MonoBehaviour
    {
        [SerializeField] protected Text text = default;
        [SerializeField] protected TimerCountdown timerCountdown = default;

        protected float minutes;
        protected float seconds;

        protected virtual void Awake()
        {
            if(text == null)
                text = GetComponent<Text>();

            if(timerCountdown == null)
            timerCountdown = FindAnyObjectByType<TimerCountdown>();

            OnTimeChanged(timerCountdown.Seconds);
        }
        protected virtual void OnEnable() => timerCountdown.onTimeChanged += OnTimeChanged;
        protected virtual void OnDisable() => timerCountdown.onTimeChanged -= OnTimeChanged;

        protected virtual void OnTimeChanged(float sec)
        {
            minutes = Mathf.FloorToInt(sec / 60);
            seconds = Mathf.FloorToInt(sec % 60);

            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}