namespace WaveShooter.Features.Spawner.EnemySpawner.EnemyWave
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
        /// Кол-во врагов на сцене изменилось.
        /// </summary>
        public event Action<int> onActiveEnemiesChanged = delegate { };

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
        /// Текущее количество врагов на сцене.
        /// </summary>
        public int ActiveEnemies { get; private set; }

        /// <summary>
        /// Флаг режима.
        /// </summary>
        public bool IsInfinite => isInfinite;

        [SerializeField, Range(1, 1000)] protected int totalEnemies = 1000;
        [SerializeField] protected int currentEnemies = 0;
        [SerializeField] protected bool isInfinite = false;
        
        /// <summary>
        /// Ресетим модель.
        /// </summary>
        public void ResetModel()
        {
            CurrentEnemies = totalEnemies;
            KilledEnemies = 0;
            ActiveEnemies = 0;

            onKilledChanged.Invoke(KilledEnemies);
        }

        /// <summary>
        /// Добавляем убитого врага.
        /// </summary>
        public void AddKill()
        {
            KilledEnemies++;
            onKilledChanged.Invoke(KilledEnemies);
        }

        /// <summary>
        /// Добавляем заспавненного врага.
        /// </summary>
        public void AddActiveEnemy()
        {
            ActiveEnemies++;
            onActiveEnemiesChanged.Invoke(ActiveEnemies);
        }

        /// <summary>
        /// Убрать заспавненого врага.
        /// </summary>
        public void RemoveActiveEnemy()
        {
            ActiveEnemies = Mathf.Max(0, ActiveEnemies - 1);
            onActiveEnemiesChanged.Invoke(ActiveEnemies);
        }

        /// <summary>
        /// Установка убитых врагов на сцене.
        /// Нужно при загрузке сохранения.
        /// </summary>
        /// <param name="value"></param>
        public void SetKilledEnemies(int value)
        {
            KilledEnemies = value;
            onKilledChanged.Invoke(KilledEnemies);
        }

        /// <summary>
        /// Установка живых врагов на сцене.
        /// Нужно при загрузке сохранения.
        /// </summary>
        /// <param name="value"></param>
        public void SetActiveEnemies(int value)
        {
            ActiveEnemies = value;
            onActiveEnemiesChanged.Invoke(ActiveEnemies);
        }
    }
}