namespace WaveShooter.Features.SaveSystem
{
    using UnityEngine;
    using WaveShooter.Features.HealthCharacter;
    using WaveShooter.Features.ShootingCharacter;

    /// <summary>
    /// Контроллер загруки.
    /// </summary>
    public sealed class LoadGameController : AbstractSaveSystemController
    {
        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();
            
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
            var ammo = container.FacadeInstance.GetDataModule<AmmoModel>();
            var health = container.FacadeInstance.GetDataModule<HealthModel>();
            var wave = waveController.Model;

            timerCountdown.Seconds = SaveManager.GetTime();
            
            health.CurrentHealth = SaveManager.GetHealth(); 
            
            ammo.AmmoMag = SaveManager.GetAmmoMag();
            ammo.AmmoTotal = SaveManager.GetAmmoTotal();
            
            wave.CurrentEnemies = SaveManager.GetEnemiesLeft();
            wave.SetKilledEnemies(SaveManager.GetKilled());
            wave.SetActiveEnemies(SaveManager.GetActive());
        }

        private void ResetForNewGame()
        {
            var ammo = container.FacadeInstance.GetDataModule<AmmoModel>();
            var health = container.FacadeInstance.GetDataModule<HealthModel>();
            var wave = waveController.Model;

            ammo.OnModuleInited();
            health.OnModuleInited();
            wave.ResetModel();
        }
    }
}