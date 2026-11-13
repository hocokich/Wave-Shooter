namespace WaveShooter.Features.Shooting
{
    using UnityEngine;
    using System;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Модель патронов
    /// </summary>
    [CreateAssetMenu(fileName = "New Ammo Model", menuName = "WaveShooter/Features/Ammo")]
    public class AmmoModel : GenericDataModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Количество патронов изменилось
        /// </summary>
        public event Action onAmmoCountChanged = delegate { };

        public int AmmoCount
        {
            get => ammoCount;
            set
            {
                if (ammoCount != value)
                {
                    ammoCount = value;
                    onAmmoCountChanged();
                    SavePrefs(ammoCount);
                }
            }
        }

        [SerializeField, Min(0)] protected int ammoCount = default;

        protected override void OnInited()
        {
            base.OnInited();
            LoadPrefs();
        }

        protected virtual void SavePrefs(int ammoCount)
        {
            PlayerPrefs.SetInt("ammo", ammoCount);
        }
        protected virtual void LoadPrefs()
        {
            AmmoCount = PlayerPrefs.GetInt("ammo");
        }
    }
}