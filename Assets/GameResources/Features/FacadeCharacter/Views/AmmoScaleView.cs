namespace WaveShooter.Features.Facade.View
{
    using UnityEngine;
    using System.Collections;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Shooting;

    /// <summary>
    /// Анимирует скейл текста патронов один раз: от максимума к минимуму.
    /// Минимум — текущий размер объекта, не размер шрифта.
    /// </summary>
    public class AmmoScaleView : ProviderCharacter
    {
        [SerializeField, Min(0)] protected float animationSpeed = 1.0f;
        [SerializeField, Min(0)] protected float maxSize = 1.5f;
        [SerializeField, Min(0)] protected float minSize = 1.0f;

        protected Coroutine coroutine = default;
        protected AmmoModel ammo = default;

        protected override void OnFacadeInited()
        {
            ammo = container.FacadeInstance.GetModuleFacade<GunController>().Ammo;

            if(ammo != null)
                ammo.onAmmoMagChanged += PlayOnce;
        }

        protected override void OnFacadeDisposed()
        {
            StopScale();

            if(ammo != null)
            {
                ammo.onAmmoMagChanged -= PlayOnce;
                ammo = null;
            }
        }

        protected virtual void OnDisable() => StopScale();

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        /// <summary>
        /// Запустить анимацию скейла. Предыдущая корутина перезаписывается.
        /// </summary>
        protected virtual void PlayOnce()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(ScaleOnce());
        }

        protected IEnumerator ScaleOnce()
        {
            Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);
            Vector3 minScale = new Vector3(minSize, minSize, minSize);
            float progress = 0f;

            transform.localScale = maxScale;

            if (animationSpeed <= 0f)
            {
                transform.localScale = minScale;
                coroutine = null;
                yield break;
            }

            while (progress < 1f)
            {
                progress += Time.deltaTime * animationSpeed;
                transform.localScale = Vector3.Lerp(maxScale, minScale, Mathf.Clamp01(progress));
                yield return null;
            }

            transform.localScale = minScale;
            coroutine = null;
        }

        protected virtual void StopScale()
        {
            if (coroutine == null)
            {
                return;
            }

            StopCoroutine(coroutine);
            coroutine = null;
            transform.localScale = new Vector3(minSize, minSize, minSize);
        }
    }
}
