namespace WaveShooter.Features.ShootingCharacter.View
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Трясёт камеру один раз: смещение утихает от амплитуды до нуля.
    /// </summary>
    public class CameraShakeView : ProviderCharacter
    {
        [SerializeField] protected Camera cameraView = default;

        [SerializeField, Min(0)] protected float amplitude = 0.2f;

        [SerializeField, Min(0)] protected float decayTime = 0.3f;

        [SerializeField, Min(0)] protected float effectSpeed = 25.0f;

        protected Coroutine coroutine = default;

        protected Vector3 originPosition = default;

        protected override void OnFacadeInited()
        {
            if (cameraView == null)
            {
                cameraView = GetComponent<Camera>();
            }

            if (cameraView == null)
            {
                cameraView = Camera.main;
            }

            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted += PlayOnce;
        }

        protected override void OnFacadeDisposed()
        {
            StopShake();

            if (container.FacadeInstance == null)
            {
                return;
            }

            container.FacadeInstance.GetModuleFacade<GunController>().onGunShooted -= PlayOnce;
        }

        protected virtual void OnDisable() => StopShake();

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        /// <summary>
        /// Запустить тряску камеры. Предыдущая корутина перезаписывается.
        /// </summary>
        protected virtual void PlayOnce()
        {
            if (!isActiveAndEnabled || cameraView == null)
            {
                return;
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            else
            {
                originPosition = cameraView.transform.localPosition;
            }

            coroutine = StartCoroutine(ShakeOnce());
        }

        protected IEnumerator ShakeOnce()
        {
            float elapsed = 0f;
            float change = 1f;
            Vector2 offset = Vector2.zero;

            if (decayTime <= 0f)
            {
                cameraView.transform.localPosition = originPosition;
                coroutine = null;
                yield break;
            }

            while (elapsed < decayTime)
            {
                float damper = 1f - (elapsed / decayTime);
                change += Time.deltaTime * effectSpeed;

                if (change >= 1f)
                {
                    //NOTE: Выбираем случайную точку куда камера будет сдвигаться и домножаем на усиливающий эффект
                    offset = Random.insideUnitCircle * amplitude;
                    change = 0f;
                }

                cameraView.transform.localPosition = originPosition + new Vector3(offset.x, offset.y, 0f) * damper;
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraView.transform.localPosition = originPosition;
            coroutine = null;
        }

        protected virtual void StopShake()
        {
            if (coroutine == null)
            {
                return;
            }

            StopCoroutine(coroutine);
            coroutine = null;

            if (cameraView == null)
            {
                return;
            }

            cameraView.transform.localPosition = originPosition;
        }
    }
}
