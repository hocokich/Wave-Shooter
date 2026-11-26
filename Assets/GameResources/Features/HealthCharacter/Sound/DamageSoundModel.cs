namespace WaveShooter.Features.HealthCharacter.Sound
{
    using UnityEngine;

    /// <summary>
    /// Звуки получения урона.
    /// </summary>
    [CreateAssetMenu(fileName = "New Damage Sound Model", menuName = "WaveShooter/Features/Health/SoundModel")]
    public sealed class DamageSoundModel : ScriptableObject
    {
        [Header("Damage")]
        [Tooltip("Клипы урона. Выбирается случайный. Один клип — тоже валидно.")]
        public AudioClip[] Hits = default;

        [Header("Death (опционально)")]
        [Tooltip("Оставь пустым, если смерть озвучивает другой контроллер.")]
        public AudioClip Death = default;
    }
}