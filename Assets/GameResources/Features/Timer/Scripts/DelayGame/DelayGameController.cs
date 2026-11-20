namespace WaveShooter.Features.Timer.DelayGame
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.PauseGame;

    /// <summary>
    /// Контроллер задержки начала игры.
    /// </summary>
    [RequireComponent(typeof(TimerCountdown))]
    public sealed class DelayGameController : ProviderCharacter
    {
        /// <summary>
        /// Время задержки изменилось.
        /// </summary>
        public event Action<int> onDelaySecondsChange = delegate { };

        /// <summary>
        /// Задержка кончилась.
        /// </summary>
        public event Action onDelayEnd = delegate { };

        /// <summary>
        /// Время задержки.
        /// </summary>
        public int DelaySeconds => delaySeconds;

        /// <summary>
        /// Задержка сейчас активна.
        /// </summary>
        public bool IsDelayActive => isDelayActive;

        [SerializeField, Range(0, 10)] private int delaySeconds = 3;
        [SerializeField] private PauseController pauseController = default;

        private TimerCountdown timer = default;
        private Coroutine coroutine = default;
        private bool isDelayActive;

        protected override void Awake()
        {
            base.Awake();

            timer = GetComponent<TimerCountdown>();
            timer.enabled = false;
        }

        private void OnEnable()
        {
            if (pauseController != null)
                pauseController.onResumeRequested += OnResumeRequested;
        }

        private void OnDisable()
        {
            if (pauseController != null)
            {
                pauseController.onResumeRequested -= OnResumeRequested;

                if (isDelayActive)
                    pauseController.SetInputBlocked(false);
            }

            StopDelay();
        }

        protected override void OnFacadeInited()
        {
            pauseController.SetInputBlocked(true);
            LaunchDelay(isResume: false);
        }

        /// <summary>
        /// Запуск задержки.
        /// </summary>
        public void LaunchDelay(bool isResume = false)
        {
            StopDelay();
            coroutine = StartCoroutine(Delay(isResume));
        }

        /// <summary>
        /// Остановка задержки.
        /// </summary>
        public void StopDelay()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            isDelayActive = false;
        }

        private void OnResumeRequested()
        {
            if (isDelayActive) return;

            pauseController.SetInputBlocked(true);
            pauseController.SetPause(false);

            LaunchDelay(isResume: true);
        }

        private IEnumerator Delay(bool isResume)
        {
            isDelayActive = true;

            int remaining = Mathf.Max(0, delaySeconds);
            onDelaySecondsChange.Invoke(remaining);

            float elapsed = 0f;

            while (remaining > 0)
            {
                elapsed += Time.unscaledDeltaTime;

                if (elapsed >= 1f)
                {
                    elapsed -= 1f;
                    remaining--;
                    if (remaining < 0) remaining = 0;

                    onDelaySecondsChange.Invoke(remaining);
                }

                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.5f);

            pauseController.SetInputBlocked(false);

            isDelayActive = false;
            coroutine = null;

            onDelayEnd.Invoke();

            if (!isResume)
                timer.enabled = true;
        }
    }
}