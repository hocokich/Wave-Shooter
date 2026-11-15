namespace WaveShooter.Features.ShootingCharacter.View
{
    using UnityEngine.UI;
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Вью патронов
    /// </summary>
    public class AmmoView : ProviderCharacter
    {
        [SerializeField] protected Text textView = default;

        [SerializeField] protected Color colorEmptyMag = Color.red;
        [SerializeField] protected Color colorFullMag  = Color.white;

        protected AmmoModel ammo = default;

        protected override void OnFacadeInited()
        {
            if (textView == null)
            {
                textView = GetComponent<Text>();
            }

            ammo = container.FacadeInstance.GetModuleFacade<GunController>().Ammo;
            ammo.onAmmoMagChanged += UpdateView;
            ammo.onAmmoTotalChanged += UpdateView;

            UpdateView();
        }

        protected override void OnFacadeDisposed()
        {
            if (ammo != null)
            {
                ammo.onAmmoMagChanged -= UpdateView;
                ammo.onAmmoTotalChanged      -= UpdateView;
                ammo = null;
            }
        }

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        protected virtual void UpdateView()
        {
            int magazine = ammo.AmmoMag;
            int total = ammo.AmmoTotal;
            int maxMag = Mathf.Max(1, ammo.MaxAmmoMag);

            float t = Mathf.Clamp01(magazine / (float)maxMag);
            Color magColor = Color.Lerp(colorEmptyMag, colorFullMag, t);
            string hex = "#" + ColorUtility.ToHtmlStringRGB(magColor);

            textView.text = $"<color={hex}>{magazine}</color> / {total}";
        }
    }
}