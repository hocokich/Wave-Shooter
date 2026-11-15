namespace WaveShooter.Features.ShootingCharacter.View
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.ShootingCharacter;

    /// <summary>
    /// Отдача оружия при выстреле.
    /// </summary>
    public sealed class WeaponRecoil : ProviderCharacter
    {
        [Header("Настройки отдачи")]
        [SerializeField, Range(0.1f, 5f)] private float recoilForce = 1f;
        [SerializeField, Range(0.05f, 0.5f)] private float recoilDuration = 0.1f;
        [SerializeField, Range(0f, 1f)] private float recoilNoise = 0.3f;

        private GunController gun;
        private Vector3 originPos;
        private Quaternion originRot;
        private Coroutine recoilCoroutine;

        protected override void OnFacadeInited()
        {
            originPos = transform.localPosition;
            originRot = transform.localRotation;

            gun = Container.FacadeInstance?.GetModuleFacade<GunController>();
            
            if (gun != null)
            {
                gun.onGunShooted += PlayRecoil;
            }
        }

        protected override void OnFacadeDisposed()
        {
            if (gun != null)
            {
                gun.onGunShooted -= PlayRecoil;
                gun = null;
            }

            if (recoilCoroutine != null)
            {
                StopCoroutine(recoilCoroutine);
                recoilCoroutine = null;
            }
            
            transform.localPosition = originPos;
            transform.localRotation = originRot;
        }

        private void PlayRecoil()
        {
            if (recoilCoroutine != null)
            {
                StopCoroutine(recoilCoroutine);
            }
            recoilCoroutine = StartCoroutine(RecoilRoutine());
        }

        private IEnumerator RecoilRoutine()
        {
            float elapsed = 0f;
            float change = 1f;
            Vector3 offsetPos = Vector3.zero;
            Vector3 offsetRot = Vector3.zero;

            if (recoilDuration <= 0f)
            {
                transform.localPosition = originPos;
                transform.localRotation = originRot;
                recoilCoroutine = null;
                yield break;
            }

            while (elapsed < recoilDuration)
            {
                float damper = 1f - (elapsed / recoilDuration);
                
                change += Time.deltaTime * 25f;

                if (change >= 1f)
                {
                    offsetPos = new Vector3(
                        Random.Range(-0.02f, 0.02f) * recoilNoise,
                        0.02f * recoilForce,
                        -0.05f * recoilForce
                    );
                    
                    offsetRot = new Vector3(
                        -3f * recoilForce,
                        0f, 
                        Random.Range(-10f, 10f) * recoilNoise
                    );
                    
                    change = 0f;
                }

                transform.localPosition = originPos + (offsetPos * damper);
                transform.localRotation = originRot * Quaternion.Euler(offsetRot * damper);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originPos;
            transform.localRotation = originRot;
            recoilCoroutine = null;
        }
    }
}