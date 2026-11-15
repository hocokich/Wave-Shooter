namespace WaveShooter.Features.Spawner.EnemySpawner
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Модель волн врагов.
    /// </summary>
    [CreateAssetMenu(fileName = "New EnemyWaveModel", menuName = "WaveShooter/Features/EnemyWaveModel")]
    public class ModelEnemyWave : ScriptableObject
    {
        /// <summary>
        /// Кол-во противников поменялось.
        /// </summary>
        public event Action<int> onEnemiesCountChanged = delegate { };

        /// <summary>
        /// Кол-во убитых врагов.
        /// </summary>
        public event Action<int> onKilledChanged = delegate { };

        /// <summary>
        /// Максимум врагов.
        /// </summary>
        public int MaxEnemies => totalEnemies;

        /// <summary>
        /// Текущее кол-во врагов.
        /// </summary>
        public int CurrentEnemies 
        {
            get => currentEnemies;
            set
            {
                if (currentEnemies != value)
                {
                    currentEnemies = value;
                    onEnemiesCountChanged.Invoke(currentEnemies);
                }
            }
        }

        /// <summary>
        /// Кол-во убитых врагов
        /// </summary>
        public int KilledEnemies { get; private set; }

        /// <summary>
        /// Флаг режима.
        /// </summary>
        public bool IsInfinite => isInfinite;

        [SerializeField, Range(1, 1000)] protected int totalEnemies = 1000;
        [SerializeField] protected int currentEnemies = 0;
        [SerializeField] protected bool isInfinite = false;
        
        public void ResetModel()
        {
            CurrentEnemies = totalEnemies;
            KilledEnemies = 0;
            onKilledChanged.Invoke(KilledEnemies);
        }

        public void AddKill()
        {
            KilledEnemies++;
            onKilledChanged.Invoke(KilledEnemies);
        }
    }
}