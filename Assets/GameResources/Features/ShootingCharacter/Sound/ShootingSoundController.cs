namespace WaveShooter.Features.ShootingCharacter.Sound
{
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Обёртка-наблюдатель над GunController: слушает события и играет звуки.
    /// GunController не модифицируется.
    /// </summary>
    public sealed class ShootingSoundController : GenericModuleFacade<CharacterFacade>
    {
        [Header("Sound Model")]
        [SerializeField] private ShootingSoundModel _soundModel = default;

        [Header("Audio Source")]
        [Tooltip("AudioSource на дочернем объекте персонажа. Если не задан — найдётся автоматически.")]
        [SerializeField] private AudioSource _audioSource = default;

        [Header("Volumes")]
        [Range(0f, 1f)][SerializeField] private float _fireVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float _emptyVolume = 0.7f;
        [Range(0f, 1f)][SerializeField] private float _reloadVolume = 0.8f;

        [Header("Empty click")]
        [Tooltip("Минимальный интервал между кликами 'пусто' при удержании огня.")]
        [SerializeField, Min(0f)] private float _emptyCooldown = 0.25f;

        private GunController _gunController;
        private float _lastEmptyTime = -999f;

        protected override void OnInited()
        {
            if (_audioSource == null)
                _audioSource = GetComponentInChildren<AudioSource>(true);

            if (_audioSource != null)
            {
                _audioSource.playOnAwake = false;
                _audioSource.spatialBlend = 0f;
            }
            else
            {
                Debug.LogWarning("ShootingSoundController: AudioSource не найден. Звуки играть не будут.");
            }

            _gunController = ConcreteFacade?.GetModuleFacade<GunController>();
            if (_gunController == null)
            {
                Debug.LogWarning("ShootingSoundController: GunController не найден на фасаде.");
                return;
            }

            _gunController.onGunShooted += HandleShooted;
            _gunController.onGunEmpty += HandleEmpty;
            _gunController.onGunReload += HandleReload;
        }

        protected override void OnDisposed()
        {
            if (_gunController != null)
            {
                _gunController.onGunShooted -= HandleShooted;
                _gunController.onGunEmpty -= HandleEmpty;
                _gunController.onGunReload -= HandleReload;
                _gunController = null;
            }
        }

        private void HandleShooted() => Play(_soundModel?.Fire, _fireVolume);
        private void HandleReload()  => Play(_soundModel?.Reload, _reloadVolume);

        private void HandleEmpty()
        {
            if (Time.unscaledTime - _lastEmptyTime < _emptyCooldown) return;
            _lastEmptyTime = Time.unscaledTime;

            Play(_soundModel?.Empty, _emptyVolume);
        }

        private void Play(AudioClip clip, float volume)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}