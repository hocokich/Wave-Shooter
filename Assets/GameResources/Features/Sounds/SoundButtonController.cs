namespace WaveShooter.Features.Sounds
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Обертка для кнопки.
    /// Включает звук по нажатию.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundButtonController : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private SoundButtonModel _SoundModel = default;

        [Header("Громкость звуков")]
        [Range(0f, 1f)]
        [SerializeField] private float _highlightedVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float _pressedVolume = 1f;

        [Header("Тайминг")]
        [Range(0.1f, 1f)]
        [SerializeField] private float _maxDelay = 1.0f;

        private Button _button;
        private AudioSource _audioSource;

        private bool _isBusy;
        private bool _pointerOver;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;
            if (_isBusy || !_button.IsInteractable()) return;
            Play(_SoundModel.Highlighted, _highlightedVolume);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isBusy || !_button.IsInteractable()) return;

            var clip = _SoundModel.Pressed;
            if (clip == null) return;

            _isBusy = true;
            _button.interactable = false;
            _audioSource.PlayOneShot(clip, _pressedVolume);
            StartCoroutine(WaitAndClick(clip.length));
        }

        private IEnumerator WaitAndClick(float clipLength)
        {
            float waitTime = Mathf.Min(clipLength, _maxDelay);
            float elapsed = 0f;

            while (elapsed < waitTime && _audioSource.isPlaying)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            _button.interactable = true;
            _isBusy = false;

            if (_pointerOver)
            {
                Play(_SoundModel.Highlighted, _highlightedVolume);
            }

            _button.onClick.Invoke();
        }

        private void Play(AudioClip clip, float volumeScale)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}