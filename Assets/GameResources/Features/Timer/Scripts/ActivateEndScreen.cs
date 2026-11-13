namespace WaveShooter.Features.Timer
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;
    using WaveShooter.Features.PauseGame;

    /// <summary>
    /// Активирует экран конца игровой сессии.
    /// </summary>
    public class ActivateEndScreen : ProviderCharacter
    {
        [SerializeField] protected Canvas endScreen = default;

        [SerializeField] protected TimerCountdown timerCountdown = default;
        [SerializeField] protected PauseManager pauseManager = default;

        protected virtual void Start()
        {
            if (timerCountdown == null)
            {
                timerCountdown = GetComponent<TimerCountdown>();
                timerCountdown.onTimerIsDone += OnTimerIsDone;
            }

            if (pauseManager == null)
            {
                pauseManager = FindAnyObjectByType<PauseManager>();
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

        protected override void OnFacadeInited()
        {
        }

        protected virtual void OnTimerIsDone()
        {
            endScreen.gameObject.SetActive(true);
            
            Time.timeScale = 0f;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            var inputSys = Container.FacadeInstance?.GetModuleFacade<InputSystemModule>();
            
            if (inputSys != null && inputSys.InputActions != null)
            {
                inputSys.InputActions.Disable(); 
            }
            else
            {
                Debug.LogError("ActivateEndScreen: Не удалось найти InputSystemModule в контейнере.");
            }
        }
    }
}