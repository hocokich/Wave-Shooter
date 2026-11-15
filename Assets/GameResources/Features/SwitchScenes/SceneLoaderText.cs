namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Отображение загрузки через Text.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class SceneLoaderText : AbstractSceneLoader
    {
        [SerializeField] private Text progressValue = default;

        private void Awake()
        {
            progressValue = GetComponent<Text>();
        }

        protected override void SetProgressPercent(int value)
        {
            if (progressValue == null) return;

            progressValue.text  = $"{value} %";
            progressValue.color = Color.Lerp(Color.red, Color.green, value / 100f);
        }
    }
}