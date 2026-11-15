namespace WaveShooter.Features.Spawner.EnemySpawner.EnemyWave
{
    using UnityEngine.UI;
    using WaveShooter.Features.Spawner.EnemySpawner.ContainerEnemyWave;

    /// <summary>
    /// Вью убитых врагов.
    /// </summary>
    public sealed class KillsView : ProviderEnemyWave
    {
        private Text text = default;
        private ModelEnemyWave waveModel = default;

        protected override void Awake()
        {
            text = GetComponent<Text>();
            base.Awake();
        }
        protected override void OnWaveInited()
        {
            waveModel = Container.ControllerInstance?.Model;

            if (waveModel != null)
            {
                waveModel.onKilledChanged += UpdateView;
                UpdateView(waveModel.KilledEnemies);
            }
        }
        protected override void OnWaveDisposed()
        {
            if (waveModel != null)
            {
                waveModel.onKilledChanged -= UpdateView;
                waveModel = null;
            }
            text = null;
        }

        private void OnEnable()
        {
            if (waveModel != null)
            {
                waveModel.onKilledChanged += UpdateView;
                UpdateView(waveModel.KilledEnemies);
            }
        }
        private void OnDisable()
        {
            if (waveModel != null)
            {
                waveModel.onKilledChanged -= UpdateView;
            }
        }

        private void UpdateView(int killed)
        {
            if (text == null ) return;
                text.text = $"Убийств: {killed}";
        }
    }
}