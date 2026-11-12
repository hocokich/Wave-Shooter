namespace WaveShooter.Features.Timer
{
    using UnityEngine;

    /// <summary>
    /// Активирует экран конца игровой сессии.
    /// </summary>
    public class ActivateEndScreen : MonoBehaviour
    {
        [SerializeField] protected TimerCountdown timerCountdown = default;
        [SerializeField] protected Canvas endScreen = default;

        protected virtual void Start()
        {
            if(timerCountdown == null)
            {
                timerCountdown = GetComponent<TimerCountdown>();
                timerCountdown.onTimerIsDone += OnTimerIsDone;
            }
        }
        protected virtual void OnEnable()
        {
            if(timerCountdown != null)
                timerCountdown.onTimerIsDone += OnTimerIsDone;
        }
        protected virtual void OnDisable()
        {
            if(timerCountdown != null)
                timerCountdown.onTimerIsDone -= OnTimerIsDone;
        }

        protected virtual void OnTimerIsDone() => endScreen.gameObject.SetActive(true);
    }
}