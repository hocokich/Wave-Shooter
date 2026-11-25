namespace WaveShooter.Features.Projectiles.Sound
{
    using UnityEngine;

    /// <summary>
    /// Модель звуков снаряда.
    /// </summary>
    [CreateAssetMenu(fileName = "New Projectile Sound Model", menuName = "WaveShooter/Features/Projectiles/ProjectileSoundModel")]
    public sealed class ProjectileSoundModel : ScriptableObject
    {
        [Header("Shooting events")]
        public AudioClip Explosion = default;
        public AudioClip Knock = default;
    }
}