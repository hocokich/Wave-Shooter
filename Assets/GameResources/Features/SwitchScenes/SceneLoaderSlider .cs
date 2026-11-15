namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Отображение загрузки через Slider.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public sealed class SceneLoaderSlider : AbstractSceneLoader
    {
        [Header("Опционально: если нужно красить слайдер")]
        [SerializeField] private Image fillImage = default;

        private Slider progressSlider = default;

        private void Awake()
        {
            progressSlider = GetComponent<Slider>();
        }

        protected override void SetProgressPercent(int value)
        {
            if (progressSlider == null) return;

            progressSlider.value = value / 100f;

            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, value / 100f);
            }
        }
    }
}