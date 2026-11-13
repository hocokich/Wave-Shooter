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
        /// Изменилось количество патронов в обойме.
        /// </summary>
        public event Action onAmmoMagChanged = delegate { };

        /// <summary>
        /// Общее количество патронов изменилось.
        /// </summary>
        public event Action onAmmoTotalChanged = delegate { };

        /// <summary>
        /// Патронов в обойме.
        /// </summary>
        public int AmmoMag
        {
            get => ammoMag;
            set
            {
                int clamped = Mathf.Clamp(value, 0, maxAmmoMag);
                if (ammoMag == clamped) return;

                ammoMag = clamped;
                onAmmoMagChanged();
                SavePrefs();
            }
        }
        /// <summary>
        /// Общий боезапас.
        /// </summary>
        public int AmmoTotal
        {
            get => ammoTotal;
            set
            {
                if (ammoTotal != value)
                {
                    ammoTotal = value;
                    onAmmoTotalChanged();
                    SavePrefs();
                }
            }
        }
        /// <summary>
        /// Максимум патрон в обойме.
        /// </summary>
        public int MaxAmmoMag => maxAmmoMag;

        /// <summary>
        /// Максимальный боезапас.
        /// </summary>
        public int MaxAmmoTotal => maxAmmoTotal;

        /// <summary>
        /// Полная ли сейчас обойма.
        /// </summary>
        public bool IsMagFull  => ammoMag >= maxAmmoMag;

        /// <summary>
        /// Пустая ли сейчас обойма.
        /// </summary>
        public bool IsMagEmpty => ammoMag <= 0;

        /// <summary>
        /// Есть ли еще патроны в запасе.
        /// </summary>
        public bool HasReserve => ammoTotal > 0;

        /// <summary>
        /// Можно ли перезарядиться.
        /// </summary>
        public bool CanReload => !IsMagFull && HasReserve;

        [Header("Максимумы")]
        [SerializeField, Range(1, 30)] protected int maxAmmoMag = 20;
        [SerializeField, Range(0, 300)] protected int maxAmmoTotal = 60;

        [Header("Текущее кол-во патрон")]
        [SerializeField, Range(1, 30)] protected int ammoMag = 20;
        [SerializeField, Range(0, 300)] protected int ammoTotal = 60;


        protected override void OnInited()
        {
            base.OnInited();
            LoadPrefs();
        }

        protected virtual void SavePrefs()
        {
            PlayerPrefs.SetInt("ammoInMagazine", ammoMag);
            PlayerPrefs.SetInt("ammoTotal", ammoTotal);
        }

        protected virtual void LoadPrefs()
        {
            if (!PlayerPrefs.HasKey("ammoInMagazine")
                && !PlayerPrefs.HasKey("ammoTotal")
                && PlayerPrefs.HasKey("ammo"))
            {
                int legacy = PlayerPrefs.GetInt("ammo");
                AmmoMag = Mathf.Min(legacy, maxAmmoMag);
                AmmoTotal = Mathf.Min(Mathf.Max(0, legacy - AmmoMag), maxAmmoTotal);
                PlayerPrefs.DeleteKey("ammo");
                return;
            }

            AmmoMag = PlayerPrefs.GetInt("ammoInMagazine", ammoMag);
            AmmoTotal      = PlayerPrefs.GetInt("ammoTotal", ammoTotal);
        }
    }
}