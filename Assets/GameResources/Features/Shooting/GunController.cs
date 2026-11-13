namespace WaveShooter.Features.Shooting
{
    using System;
    using WaveShooter.Features.Facade;
    using UnityEngine;

    /// <summary>
    /// Контроллер оружия который стреляет и обрабатывает патроны. 
    /// Оружие не отвечает здесь за прицеливание и факт попадания. 
    /// </summary>
    public class GunController : GenericModuleFacade<CharacterFacade>
    {
        /// <summary>
        /// Предупреждение о выстреле.
        /// </summary>
        public event Action onGunTryShoot = delegate { };

        /// <summary>
        /// Оружие выстрелило
        /// </summary>
        public event Action onGunShooted = delegate { };

        /// <summary>
        /// Оружие перезаряжено
        /// </summary>
        public event Action onGunReload = delegate { };

        public AmmoModel Ammo => modelAmmo;

        [SerializeField] protected AmmoModel modelAmmo = default;

        [SerializeField, Min(0)] protected float cooldown = 1.0f;

        protected float nextShotTime = 0f;

        protected int originCount = 0;

        protected virtual void Awake() => originCount = modelAmmo.AmmoCount;

        /// <summary>
        /// Выстрелить
        /// </summary>
        public virtual void TryShoot()
        {
            onGunTryShoot.Invoke();
            
            if (Time.time < nextShotTime || modelAmmo.AmmoCount <= 0)
            {
                return;
            }

            modelAmmo.AmmoCount--;
            onGunShooted();
            nextShotTime = Time.time + cooldown;
        }

        /// <summary>
        /// Перезарядить
        /// </summary>
        public virtual void Reload()
        {
            if (modelAmmo.AmmoCount != 0) return;

            modelAmmo.AmmoCount = 20;
            onGunReload();
        }
    }
}