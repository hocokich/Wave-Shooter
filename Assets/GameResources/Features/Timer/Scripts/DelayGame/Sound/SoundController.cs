namespace WaveShooter.Features.Timer.DelayGame.Sound
{
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Контроллер звука для задержки игры.
    /// </summary>
    [RequireComponent(typeof(DelayGameController))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundController : ProviderCharacter
    {
        [Header("Sound Model")]
        [SerializeField] private SoundModel _soundModel = default;

        [Header("Volumes")]
        [Range(0f, 1f)]
        [SerializeField] private float _tickVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _endVolume = 1f;

        private DelayGameController _delayGameController;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();

            _delayGameController = GetComponent<DelayGameController>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f;
        }

        protected override void OnFacadeInited()
        {
            _delayGameController.onDelaySecondsChange += OnDelaySecondsChange;
        }

        protected override void OnFacadeDisposed()
        {
            if (_delayGameController != null)
            {
                _delayGameController.onDelaySecondsChange -= OnDelaySecondsChange;
                _delayGameController = null;
            }

            _audioSource = null;

            base.OnFacadeDisposed();
        }

        private void OnDelaySecondsChange(int remainingSeconds)
        {
            // > 0 — тик каждого оставшегося значения.
            // == 0 — отсчёт дошёл до нуля, играем финальный звук.
            if (remainingSeconds > 0)
            {
                PlayOneShot(_soundModel != null ? _soundModel.Tik : null, _tickVolume);
            }
            else
            {
                PlayOneShot(_soundModel != null ? _soundModel.EndDelay : null, _endVolume);
            }
        }

        private void PlayOneShot(AudioClip clip, float volumeScale)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}