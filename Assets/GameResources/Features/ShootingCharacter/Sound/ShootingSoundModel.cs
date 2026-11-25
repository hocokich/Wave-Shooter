namespace WaveShooter.Features.ShootingCharacter.Sound
{
    using UnityEngine;

    /// <summary>
    /// Звуки стрельбы.
    /// </summary>
    [CreateAssetMenu(fileName = "New Shooting Sound Model", menuName = "WaveShooter/Features/Shooting/Sound Model")]
    public sealed class ShootingSoundModel : ScriptableObject
    {
        [Header("Shooting events")]
        public AudioClip Fire = default;
        public AudioClip Empty = default;
        public AudioClip Reload = default;
    }
}