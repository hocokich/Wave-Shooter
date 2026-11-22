namespace WaveShooter.Features.ScreenSpawner
{
    using UnityEngine;
    using WaveShooter.Features.GameState;
    using WaveShooter.Features.PauseGame;
    using WaveShooter.Features.SaveSystem;
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.UIElements.Buttons;

    /// <summary>
    /// Спавнит/удаляет окна в зависимости от состояния игры.
    /// </summary>
    public sealed class ScreenController : MonoBehaviour
    {
        [Header("Контейнер для окон")]
        [SerializeField] private Transform viewParent = default;

        [Header("Префабы окон")]
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pauseMenuPrefab;
        [SerializeField] private GameObject gameOverPrefab;

        [Header("Контроллеры сцены")]
        [SerializeField] private GameStateModel stateModel;
        [SerializeField] private PauseController pauseController;
        [SerializeField] private TimerCountdown timerCountdown;
        [SerializeField] private SaveGameController saveController;

        private GameObject currentPauseMenu;
        private GameObject currentGameOver;

        private void Start()
        {
            if (hudPrefab != null)
                Instantiate(hudPrefab, viewParent);

            stateModel.onStateChanged += HandleStateChanged;
            timerCountdown.onTimerDone += HandleTimerDone;

            HandleStateChanged(stateModel.CurrentState);
        }

        private void OnDestroy()
        {
            if (stateModel != null) stateModel.onStateChanged -= HandleStateChanged;
            if (timerCountdown != null) timerCountdown.onTimerDone -= HandleTimerDone;
        }

        private void HandleTimerDone()
        {
            stateModel.SetState(GameState.GameOver);
        }

        private void HandleStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Pause:
                    SpawnPauseMenu();
                    break;

                case GameState.GameOver:
                    DestroyPauseMenu();
                    SpawnGameOver();
                    break;

                default:
                    DestroyPauseMenu();
                    DestroyGameOver();
                    break;
            }
        }

        private void SpawnPauseMenu()
        {
            if (currentPauseMenu != null || pauseMenuPrefab == null) return;

            currentPauseMenu = Instantiate(pauseMenuPrefab, viewParent);

            var pauseView = currentPauseMenu.GetComponentInChildren<TogglePauseMenu>(true);
            if (pauseView != null) pauseView.Initialize(pauseController);

            var saveButton = currentPauseMenu.GetComponentInChildren<SaveGameButton>(true);
            if (saveButton != null) saveButton.Initialize(saveController);
        }

        private void DestroyPauseMenu()
        {
            if (currentPauseMenu == null) return;
            Destroy(currentPauseMenu);
            currentPauseMenu = null;
        }

        private void SpawnGameOver()
        {
            if (currentGameOver != null || gameOverPrefab == null) return;
            currentGameOver = Instantiate(gameOverPrefab, viewParent);
        }

        private void DestroyGameOver()
        {
            if (currentGameOver == null) return;
            Destroy(currentGameOver);
            currentGameOver = null;
        }
    }
}