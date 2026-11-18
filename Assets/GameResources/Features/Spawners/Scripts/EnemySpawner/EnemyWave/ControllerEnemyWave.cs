namespace WaveShooter.Features.Spawner.EnemySpawner.EnemyWave
{
    using UnityEngine;

    /// <summary>
    /// Управляет логикой лимита врагов.
    /// </summary>
    public class ControllerEnemyWave : MonoBehaviour
    {
        /// <summary>
        /// Модель волн врагов.
        /// </summary>
        public ModelEnemyWave Model => waveModel;

        [SerializeField] protected ModelEnemyWave waveModel = default;

        protected virtual void Awake()
        {
            if (waveModel == null) return;

            // Свежий старт: все счётчики нули (значения из ассета).
            // Загруженное сохранение уже содержит непустые значения — его не трогаем.
            bool isFreshState =
                waveModel.ActiveEnemies == 0 &&
                waveModel.KilledEnemies == 0 &&
                waveModel.CurrentEnemies == 0;

            if (isFreshState)
            {
                waveModel.ResetModel();
            }
            else
            {
                // Пере-испускаем события, чтобы HUD подхватил загруженные значения.
                waveModel.SetActiveEnemies(waveModel.ActiveEnemies);
                waveModel.SetKilledEnemies(waveModel.KilledEnemies);
            }
        }

        /// <summary>
        /// Проверяет, можно ли заспавнить врага, и если да — вычитает его из лимита.
        /// </summary>
        public bool TryConsumeEnemy()
        {
            if (waveModel == null) return false;
            if (waveModel.IsInfinite) return true;
            if (waveModel.CurrentEnemies <= 0) return false;

            waveModel.CurrentEnemies--;
            return true;
        }

        /// <summary>
        /// Вызывается, когда врага убивают.
        /// </summary>
        public void RegisterKill()
        {
            if (waveModel == null) return;

            waveModel.AddKill();
            waveModel.RemoveActiveEnemy();
        }

        /// <summary>
        /// Вызывается при успешном спавне врага.
        /// </summary>
        public void RegisterSpawn()
        {
            if (waveModel == null) return;
            
            waveModel.AddActiveEnemy();
        }
    }
}