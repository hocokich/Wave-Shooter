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
            if (waveModel != null)
                waveModel.ResetModel();
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
            if (waveModel != null)
            {
                waveModel.AddKill();
            }
        }

        /// <summary>
        /// Вызывается при успешном спавне врага.
        /// </summary>
        public void RegisterSpawn()
        {
            if (waveModel != null)
            {
                waveModel.AddActiveEnemy();
            }
        }
    }
}