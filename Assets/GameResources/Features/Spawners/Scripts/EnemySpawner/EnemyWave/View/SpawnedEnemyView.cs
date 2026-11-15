namespace WaveShooter.Features.Spawner.EnemySpawner.EnemyWave
{
    using UnityEngine.UI;
    using WaveShooter.Features.Spawner.EnemySpawner.ContainerEnemyWave;

    /// <summary>
    /// Вью отображения врагов на сцене.
    /// </summary>
    public sealed class SpawnedEnemiesView : ProviderEnemyWave
    {
        private Text text = default;
        private ModelEnemyWave waveModel = default;

        protected override void OnWaveInited()
        {
            text = GetComponent<Text>();
            waveModel = Container.ControllerInstance?.Model;

            if (waveModel != null)
            {
                waveModel.onActiveEnemiesChanged += UpdateView;
                UpdateView(waveModel.ActiveEnemies);
            }
        }

        protected override void OnWaveDisposed()
        {
            if (waveModel != null)
            {
                waveModel.onActiveEnemiesChanged -= UpdateView;
                waveModel = null;
            }
        }

        private void UpdateView(int activeEnemies)
        {
            if (text != null)
            {
                text.text = $"Врагов заспавнено: {activeEnemies}"; 
            }
        }
    }
}