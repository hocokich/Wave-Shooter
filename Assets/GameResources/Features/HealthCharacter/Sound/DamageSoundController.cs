namespace WaveShooter.Features.HealthCharacter.Sound
{
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Обёртка-наблюдатель над HealthController: слушает урон и смерть.
    /// </summary>
    public sealed class DamageSoundController : GenericModuleFacade<CharacterFacade>
    {
        [Header("Sound Model")]
        [SerializeField] private DamageSoundModel _soundModel = default;

        [Header("Audio Source")]
        [Tooltip("AudioSource на дочернем объекте персонажа. Если не задан — найдётся автоматически.")]
        [SerializeField] private AudioSource _audioSource = default;

        [Header("Volumes")]
        [Range(0f, 1f)][SerializeField] private float _hitVolume = 0.8f;
        [Range(0f, 1f)][SerializeField] private float _deathVolume = 1f;

        [Header("Throttle")]
        [Tooltip("Минимальный интервал между звуками урона. " +
                 "Защита от 'дробежания' при уроне каждый кадр (огонь, кислота и т.п.).")]
        [SerializeField, Min(0f)] private float _minInterval = 0.15f;

        private HealthController _healthController;
        private float _lastHitTime = -999f;

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
                Debug.LogWarning("DamageSoundController: AudioSource не найден. Звуки играть не будут.");
            }

            _healthController = ConcreteFacade?.GetModuleFacade<HealthController>();
            if (_healthController == null)
            {
                Debug.LogWarning("DamageSoundController: HealthController не найден на фасаде.");
                return;
            }

            _healthController.onDamaged += HandleDamaged;
            _healthController.onDied    += HandleDied;
        }

        protected override void OnDisposed()
        {
            if (_healthController != null)
            {
                _healthController.onDamaged -= HandleDamaged;
                _healthController.onDied    -= HandleDied;
                _healthController = null;
            }
        }

        private void HandleDamaged()
        {
            if (Time.unscaledTime - _lastHitTime < _minInterval) return;
            _lastHitTime = Time.unscaledTime;

            Play(RandomHitClip(), _hitVolume);
        }

        private void HandleDied() => Play(_soundModel?.Death, _deathVolume);

        private AudioClip RandomHitClip()
        {
            var clips = _soundModel != null ? _soundModel.Hits : null;
            if (clips == null || clips.Length == 0) return null;

            for (int i = 0; i < clips.Length; i++)
            {
                var c = clips[Random.Range(0, clips.Length)];
                if (c != null) return c;
            }
            return null;
        }

        private void Play(AudioClip clip, float volume)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}