namespace WaveShooter.Features.Projectiles.Sound
{
    using UnityEngine;
    using WaveShooter.Features.Spawners.Physics;

    /// <summary>
    /// Контроллер звуков снаряда
    /// </summary>
    [RequireComponent(typeof(DestroyBullet))]
    public sealed class ProjectileSoundController : MonoBehaviour
    {
        [Header("Sound Model")]
        [SerializeField] private ProjectileSoundModel _soundModel = default;

        [Header("Audio Prefab")]
        [Tooltip("Префаб с AudioSource. Спавнится на позиции снаряда. " +
                 "Если не задан — создаётся одноразовый GO в рантайме.")]
        [SerializeField] private AudioSource _audioPrefab = default;

        [Header("Volumes")]
        [Range(0f, 1f)][SerializeField] private float _explosionVolume = 1f;

        [Header("3D")]
        [Tooltip("1 = позиционный (3D), 0 = плоский (2D).")]
        [Range(0f, 1f)][SerializeField] private float _spatialBlend = 1f;

        private DestroyBullet _destroyBullet;

        private void Awake()
        {
            _destroyBullet = GetComponent<DestroyBullet>();
            _destroyBullet.OnExpolde += HandleExplosion;
        }

        private void HandleExplosion()
        {
            if (this == null) return;

            var clip = _soundModel != null ? _soundModel.Explosion : null;
            if (clip == null) return;

            PlayDetached(clip, transform.position, _explosionVolume);
        }
        private void PlayDetached(AudioClip clip, Vector3 position, float volume)
        {
            AudioSource source;

            if (_audioPrefab != null)
            {
                source = Instantiate(_audioPrefab, position, Quaternion.identity);
            }
            else
            {
                var go = new GameObject("~OneShotAudio");
                go.transform.SetPositionAndRotation(position, Quaternion.identity);
                source = go.AddComponent<AudioSource>();
            }

            source.playOnAwake = false;
            source.spatialBlend = _spatialBlend;
            source.clip = clip;
            source.volume = volume;
            source.Play();

            Destroy(source.gameObject, clip.length);
        }
    }
}