namespace WaveShooter.Features.ScreenSpawner
{
    using UnityEngine;
    using WaveShooter.Features.PauseGame;
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.SaveSystem;
    using WaveShooter.Features.UIElements;

    /// <summary>
    /// Контроллер экранов.
    /// </summary>
    public class ScreenController : MonoBehaviour
    {
        [Header("Контейнер для окон")]
        [SerializeField] private Transform viewParent = default;

        [Header("Префабы окон")]
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pauseMenuPrefab;
        [SerializeField] private GameObject gameOverPrefab;

        [Header("Контроллеры сцены")]
        [SerializeField] private PauseController pauseController;
        [SerializeField] private TimerCountdown timerCountdown;
        [SerializeField] private SaveGameController saveController;

        private GameObject currentPauseMenu;
        private GameObject currentGameOver;

        private void Start()
        {
            if (hudPrefab != null)
                Instantiate(hudPrefab, viewParent);

            if (pauseController != null)
                pauseController.onIsPausedChange += HandlePause;

            if (timerCountdown != null)
                timerCountdown.onTimerIsDone += HandleGameOver;
        }

        private void OnDestroy()
        {
            if (pauseController != null) pauseController.onIsPausedChange -= HandlePause;
            if (timerCountdown != null) timerCountdown.onTimerIsDone -= HandleGameOver;
        }

        private void HandlePause(bool isPaused)
        {
            if (currentGameOver != null) return; 

            if (isPaused)
            {
                currentPauseMenu = Instantiate(pauseMenuPrefab, viewParent);
                if (currentPauseMenu == null) return;

                var pauseView = currentPauseMenu.GetComponentInChildren<TogglePauseMenu>(true);
                pauseView.Initialize(pauseController);

                var saveGameButton = currentPauseMenu.GetComponentInChildren<SaveGameButton>(true);
                saveGameButton.Initialize(saveController);
            }
            else
            {
                Destroy(currentPauseMenu);
                currentPauseMenu = null;
            }
        }


        private void HandleGameOver()
        {
            if (currentGameOver != null) return;

            if (currentPauseMenu != null)
            {
                Destroy(currentPauseMenu);
                currentPauseMenu = null;
            }

            currentGameOver = Instantiate(gameOverPrefab, viewParent);
            
            if (pauseController != null)
            {
                pauseController.SetPause(true);
                pauseController.DisablePauseInput();
            }
            else
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}