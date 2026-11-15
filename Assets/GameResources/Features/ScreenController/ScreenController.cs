namespace WaveShooter.Features.ScreenSpawner
{
    using UnityEngine;
    using WaveShooter.Features.PauseGame;
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.SaveSystem;

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
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private TimerCountdown timerCountdown;
        [SerializeField] private SaveGameController saveController;

        private GameObject currentPauseMenu;
        private GameObject currentGameOver;

        private void Start()
        {
            if (hudPrefab != null)
                Instantiate(hudPrefab, viewParent);

            if (pauseManager != null)
                pauseManager.onIsPausedChange += HandlePause;

            if (timerCountdown != null)
                timerCountdown.onTimerIsDone += HandleGameOver;
        }

        private void OnDestroy()
        {
            if (pauseManager != null) pauseManager.onIsPausedChange -= HandlePause;
            if (timerCountdown != null) timerCountdown.onTimerIsDone -= HandleGameOver;
        }

        private void HandlePause(bool isPaused)
        {
            if (currentGameOver != null) return; 

            if (isPaused)
            {
                if (currentPauseMenu == null)
                {
                    currentPauseMenu = Instantiate(pauseMenuPrefab, viewParent);
                    var pauseView = currentPauseMenu.GetComponentInChildren<TogglePauseMenu>(true);
                    if (pauseView != null)
                    {
                        pauseView.Initialize(pauseManager, saveController);
                    }
                }
            }
            else
            {
                if (currentPauseMenu != null)
                {
                    Destroy(currentPauseMenu);
                    currentPauseMenu = null;
                }
            }
        }


        private void HandleGameOver()
        {
            if (currentGameOver == null)
            {
                if (currentPauseMenu != null)
                {
                    Destroy(currentPauseMenu);
                    currentPauseMenu = null;
                }

                currentGameOver = Instantiate(gameOverPrefab, viewParent);
                
                if (pauseManager != null)
                {
                    pauseManager.SetPause(true);
                    pauseManager.DisablePauseInput();
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
}