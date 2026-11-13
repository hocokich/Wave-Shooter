namespace WaveShooter.Features.Facade.View
{
    using UnityEngine;
    using UnityEngine.UI;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Health;

    /// <summary>
    /// Вью здоровья игрока.
    /// </summary>
    public class HealthView : ProviderCharacter
    {
        [SerializeField] protected Text textView = default;

        [Header("Настройки цвета")]
        [SerializeField] protected Color fullHealthColor = Color.white;
        [SerializeField] protected Color lowHealthColor = Color.red;

        protected HealthModel healthModel = default;

        protected override void OnFacadeInited()
        {
            if (textView == null)
            {
                textView = GetComponent<Text>();
            }

            healthModel = container.FacadeInstance.GetDataModule<HealthModel>();

            if (healthModel != null)
            {
                healthModel.onHealthChanged += UpdateView;
                UpdateView(healthModel.CurrentHealth, healthModel.MaxHealth);
            }
            else
            {
                Debug.LogError("HealthView: HealthModel не найден в фасаде!");
            }
        }

        protected override void OnFacadeDisposed()
        {
            if (healthModel != null)
            {
                healthModel.onHealthChanged -= UpdateView;
                healthModel = null;
            }
        }

        protected virtual void UpdateView(int currentHealth, int maxHealth)
        {
            if (maxHealth <= 0) return;

            if (textView != null)
            {
                float healthPercent = (float)currentHealth / maxHealth;

                textView.text = currentHealth.ToString();
                textView.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
            }
        }
    }
}