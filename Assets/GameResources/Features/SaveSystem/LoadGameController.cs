namespace WaveShooter.Features.SaveSystem
{
    using UnityEngine;
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.Facade;
    using WaveShooter.Features.HealthCharacter;
    using WaveShooter.Features.ShootingCharacter;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Контроллер загруки.
    /// </summary>
    public sealed class GameSessionLoader : MonoBehaviour
    {
        [SerializeField] private CharacterFacade characterFacade;
        [SerializeField] private TimerCountdown timerCountdown;
        [SerializeField] private ControllerEnemyWave waveController;
        

        private void Awake()
        {
            characterFacade = FindAnyObjectByType<CharacterFacade>();
            timerCountdown = FindAnyObjectByType<TimerCountdown>();
            waveController = FindAnyObjectByType<ControllerEnemyWave>();
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("LoadGame", 0) == 1)
            {
                LoadGameData();
                PlayerPrefs.SetInt("LoadGame", 0);
            }
            else
            {
                ResetForNewGame();
            }
        }

        private void LoadGameData()
        {
            var ammo = characterFacade.GetDataModule<AmmoModel>();
            var health = characterFacade.GetDataModule<HealthModel>();
            var wave = waveController.Model;

            timerCountdown.Seconds = SaveManager.GetTime();
            
            health.CurrentHealth = SaveManager.GetHealth(); 
            
            ammo.AmmoMag = SaveManager.GetAmmoMag();
            ammo.AmmoTotal = SaveManager.GetAmmoTotal();
            
            wave.CurrentEnemies = SaveManager.GetEnemiesLeft();
            wave.SetKilledEnemies(SaveManager.GetKilled());
            wave.SetActiveEnemies(SaveManager.GetActive());

            Debug.Log("Игра загружена из сохранения!");
        }

        private void ResetForNewGame()
        {
            var ammo = characterFacade.GetDataModule<AmmoModel>();
            var health = characterFacade.GetDataModule<HealthModel>();
            var wave = waveController.Model;

            ammo.OnModuleInited();
            health.OnModuleInited();
            wave.ResetModel();
            
            Debug.Log("Новая игра. Модели сброшены.");
        }
    }
}