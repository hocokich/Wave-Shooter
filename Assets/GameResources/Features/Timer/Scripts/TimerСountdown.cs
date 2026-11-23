namespace WaveShooter.Features.Timer
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.GameState;

    /// <summary>
    /// Таймер обратного отсчета.
    /// </summary>
    public sealed class TimerCountdown : ProviderCharacter
    {
        /// <summary>
        /// Таймер запустился.
        /// </summary>
        public event Action onTimerStart = delegate {};

        /// <summary>
        /// Таймер истек.
        /// </summary>
        public event Action onTimerDone = delegate {};

        /// <summary>
        /// Таймер тикает.
        /// </summary>
        public event Action<float> onTimeChanged = delegate {};

        /// <summary>
        /// Оставшееся время.
        /// </summary>
        public float Seconds
        {
            get => seconds;
            set
            {
                if(seconds != value)
                {
                    seconds = value;
                    onTimeChanged.Invoke(seconds);
                }
            }
        }

        [SerializeField] private GameStateModel stateModel = default;
        
        [SerializeField] private float seconds = 60f;

        private Coroutine coroutine;
        private bool hasStarted;

        protected override void OnFacadeInited()
        {
            stateModel.onStateChanged += OnStateChanged;
            OnStateChanged(stateModel.CurrentState);
        }
        protected override void OnFacadeDisposed()
        {
            if (stateModel != null)
                stateModel.onStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            if (state == GameState.Gameplay && !hasStarted)
            {
                hasStarted = true;
                onTimerStart.Invoke();
                coroutine = StartCoroutine(Countdown());
            }
        }

        private IEnumerator Countdown()
        {
            float elapsed = 0f;

            while (seconds > 0)
            {
                elapsed += Time.deltaTime;

                if (elapsed >= 1f)
                {
                    elapsed -= 1f;
                    seconds--;
                    if (seconds < 0) seconds = 0;

                    onTimeChanged.Invoke(seconds);
                }
                yield return null;
            }
            onTimerDone.Invoke();
        }
    }
}