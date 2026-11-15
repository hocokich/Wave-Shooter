namespace WaveShooter.Features.SaveSystem
{
    using UnityEngine;
    using WaveShooter.Features.Timer;
    using WaveShooter.Features.Facade;
    using WaveShooter.Features.HealthCharacter;
    using WaveShooter.Features.ShootingCharacter;
    using WaveShooter.Features.Spawner.EnemySpawner.EnemyWave;

    /// <summary>
    /// Контроллер сохранений.
    /// </summary>
    public sealed class SaveGameController : MonoBehaviour
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

        /// <summary>
        /// Сохранить игру.
        /// </summary>
        public void SaveGame()
        {
            var ammo = characterFacade.GetDataModule<AmmoModel>();
            var health = characterFacade.GetDataModule<HealthModel>();
            var wave = waveController.Model;

            SaveManager.SaveGame(
                timerCountdown.Seconds,
                health.CurrentHealth,
                ammo.AmmoMag,
                ammo.AmmoTotal,
                wave.CurrentEnemies,
                wave.KilledEnemies,
                wave.ActiveEnemies
            );
        }
    }
}