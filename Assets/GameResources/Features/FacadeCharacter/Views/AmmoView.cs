namespace WaveShooter.Features.Facade.View
{
    using UnityEngine.UI;
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Shooting;

    /// <summary>
    /// Вью патронов
    /// </summary>
    public class AmmoView : ProviderCharacter
    {
        [SerializeField] protected Text textView = default;
        
        protected AmmoModel ammo = default;

        protected override void OnFacadeInited()
        {
            if (textView == null)
            {
                textView = GetComponent<Text>();
            }

            ammo = container.FacadeInstance.GetModuleFacade<GunController>().Ammo;
            ammo.onAmmoCountChanged += UpdateView;

            UpdateView();
        }

        protected override void OnFacadeDisposed()
        {
            if (ammo != null)
            {
                ammo.onAmmoCountChanged -= UpdateView;
                ammo = null;
            }
        }

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        protected virtual void UpdateView() => textView.text = $"{ammo.AmmoCount} : {ammo.AmmoCount}";
    }
}