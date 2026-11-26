namespace WaveShooter.Features.MoveCharacter.Sound
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Обёртка-наблюдатель над MoveController: играет шаги во время движения.
    /// MoveController не модифицируется в части логики — только события.
    /// </summary>
    public sealed class FootstepSoundController : GenericModuleFacade<CharacterFacade>
    {
        [Header("Sound Model")]
        [SerializeField] private FootstepSoundModel _soundModel = default;

        [Header("Audio Source")]
        [Tooltip("AudioSource на дочернем объекте персонажа. Если не задан — найдётся автоматически.")]
        [SerializeField] private AudioSource _audioSource = default;

        [Header("Volumes")]
        [Range(0f, 1f)][SerializeField] private float _walkVolume = 0.6f;
        [Range(0f, 1f)][SerializeField] private float _sprintVolume = 0.85f;

        [Header("Intervals")]
        [Tooltip("Интервал между шагами при ходьбе (сек).")]
        [SerializeField, Min(0.1f)] private float _walkInterval = 0.5f;

        [Tooltip("Интервал между шагами при беге (сек).")]
        [SerializeField, Min(0.1f)] private float _sprintInterval = 0.3f;

        private MoveController _moveController;
        private Coroutine _stepRoutine;
        private bool _isMoving;
        private bool _isSprinting;

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
                Debug.LogWarning("FootstepSoundController: AudioSource не найден. Звуки играть не будут.");
            }

            _moveController = ConcreteFacade?.GetModuleFacade<MoveController>();
            if (_moveController == null)
            {
                Debug.LogWarning("FootstepSoundController: MoveController не найден на фасаде.");
                return;
            }

            _moveController.onMoveStart    += HandleMoveStart;
            _moveController.onMoveStop     += HandleMoveStop;
            _moveController.onSprintChanged += HandleSprintChanged;
        }

        protected override void OnDisposed()
        {
            if (_moveController != null)
            {
                _moveController.onMoveStart    -= HandleMoveStart;
                _moveController.onMoveStop     -= HandleMoveStop;
                _moveController.onSprintChanged -= HandleSprintChanged;
                _moveController = null;
            }

            StopSteps();
        }

        private void HandleMoveStart()
        {
            _isMoving = true;
            StopSteps();
            _stepRoutine = StartCoroutine(StepRoutine());
        }

        private void HandleMoveStop()
        {
            _isMoving = false;
            StopSteps();
        }

        private void HandleSprintChanged(bool isSprinting)
        {
            _isSprinting = isSprinting;
            // Не перезапускаем корутину — следующий цикл уже подхватит новый интервал.
        }

        private void StopSteps()
        {
            if (_stepRoutine != null)
            {
                StopCoroutine(_stepRoutine);
                _stepRoutine = null;
            }
        }

        private IEnumerator StepRoutine()
        {
            // Первый шаг — сразу, чтобы отклик на начало движения был мгновенным.
            while (_isMoving)
            {
                PlayStep();

                float interval = _isSprinting ? _sprintInterval : _walkInterval;
                yield return new WaitForSeconds(interval);
            }
            _stepRoutine = null;
        }

        private void PlayStep()
        {
            var clip = _isSprinting
                ? (RandomClip(_soundModel?.SprintSteps) ?? RandomClip(_soundModel?.WalkSteps))
                : RandomClip(_soundModel?.WalkSteps);

            if (clip == null) return;

            float volume = _isSprinting ? _sprintVolume : _walkVolume;
            _audioSource.PlayOneShot(clip, volume);
        }

        private AudioClip RandomClip(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0) return null;

            // Пропускаем пустые слоты (можно оставить дырку в инспекторе).
            for (int i = 0; i < clips.Length; i++)
            {
                var c = clips[Random.Range(0, clips.Length)];
                if (c != null) return c;
            }
            return null;
        }
    }
}