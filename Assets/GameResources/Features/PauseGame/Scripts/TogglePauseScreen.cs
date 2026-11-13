namespace WaveShooter.Features.PauseGame
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Вкл/Выкл меню паузы.
    /// </summary>
    public sealed class TogglePauseScreen : ProviderCharacter
    {
        [SerializeField] private Canvas pauseMenu = default;
        private PauseManager pauseManager = default;

        protected override void OnFacadeInited()
        {
            if(pauseManager == null)
            {
                pauseManager = GetComponent<PauseManager>();
                pauseManager.onIsPausedChange += OnIsPausedChange;  
            }
        }
        protected override void OnFacadeDisposed()
        {
            if(pauseManager != null)
            {
                pauseManager.onIsPausedChange -= OnIsPausedChange;
            }
        }

        private void OnIsPausedChange(bool obj)
        {
            pauseMenu.gameObject.SetActive(obj);
        }   
    }
}
