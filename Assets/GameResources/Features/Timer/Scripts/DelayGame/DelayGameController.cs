namespace WaveShooter.Features.Timer.DelayGame
{
    using System;
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.GameState;

    /// <summary>
    /// Обратный отсчёт до запуска state = Gameplay.
    /// </summary>
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
        [SerializeField] private GameStateModel stateModel = default;

        private Coroutine coroutine;
        private bool isDelayActive;

        protected override void OnFacadeInited()
        {
            stateModel.onStateChanged += OnStateChanged;
            OnStateChanged(stateModel.CurrentState);
        }
        protected override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        protected override void OnFacadeDisposed()
        {
            if (stateModel != null)
                stateModel.onStateChanged -= OnStateChanged;
            StopDelay();
        }

        private void OnDisable() => StopDelay();

        private void OnStateChanged(GameState state)
        {
            if (state == GameState.DelayGameplay)
            {
                StopDelay();
                coroutine = StartCoroutine(Delay());
            }
            else
            {
                StopDelay();
            }
        }

        private void StopDelay()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            isDelayActive = false;
        }

        private IEnumerator Delay()
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
                    remaining = Mathf.Max(0, remaining - 1);
                    onDelaySecondsChange.Invoke(remaining);
                }
                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.5f);

            isDelayActive = false;
            coroutine = null;

            onDelayEnd.Invoke();

            if (stateModel.CurrentState == GameState.DelayGameplay)
                stateModel.SetState(GameState.Gameplay);
        }
    }
}