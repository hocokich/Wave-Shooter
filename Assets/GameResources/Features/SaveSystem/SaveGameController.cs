namespace WaveShooter.Features.SaveSystem
{
    using WaveShooter.Features.HealthCharacter;
    using WaveShooter.Features.ShootingCharacter;

    /// <summary>
    /// Контроллер сохранений.
    /// </summary>
    public sealed class SaveGameController : SaveSystemController
    {
        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();
            SaveGame();
        }
        protected override void OnFacadeDisposed()
        {
            SaveGame();
            base.OnFacadeDisposed();
        }

        /// <summary>
        /// Сохранить игру.
        /// </summary>
        private void SaveGame()
        {
            var ammo = container.FacadeInstance.GetDataModule<AmmoModel>();
            var health = container.FacadeInstance.GetDataModule<HealthModel>();
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