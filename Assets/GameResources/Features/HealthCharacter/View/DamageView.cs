namespace WaveShooter.Features.HealthCharacter
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Вью получения урона.
    /// </summary>
    public sealed class DamageView : ProviderCharacter
    {
        [SerializeField] private float cooldown = 2f;

        [SerializeField, Range(0.01f, 0.5f)] private float alpha = 0.1f;
        private Image imageView = default;

        private HealthController healthModel = default;
        private Coroutine coroutine = default;

        protected override void OnFacadeInited()
        {
            if (imageView == null)
            {
                imageView = GetComponent<Image>();
            }

            healthModel = container.FacadeInstance.GetModuleFacade<HealthController>();

            if (healthModel != null)
            {
                healthModel.onDamaged += UpdateView;
            }
        }
        protected override void OnFacadeDisposed()
        {
            if (healthModel != null)
            {
                healthModel.onDamaged -= UpdateView;
                healthModel = null;
            }
        }

        private void UpdateView()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(DamageFadeRoutine());
        }

        private IEnumerator DamageFadeRoutine()
        {
            float elapsed = 0f;
            Color color = imageView.color;

            color.a = alpha;
            imageView.color = color;

            while (elapsed < cooldown)
            {
                elapsed += Time.deltaTime;
                
                float t = elapsed / cooldown;
                
                color.a = Mathf.Lerp(alpha, 0f, t);
                imageView.color = color;

                yield return null;
            }
        }
    }
}