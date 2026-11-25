namespace WaveShooter.Features.Timer.DelayGame.Sound
{
    using UnityEngine;

    /// <summary>
    /// Звуки задержки
    /// </summary>
    [CreateAssetMenu(fileName = "DelayGame Sound Model", menuName = "WaveShooter/Features/DelayGame/SoundModel")]
    public class SoundModel : ScriptableObject
    {
        [Header("Delay events")]

        /// <summary>
        /// Звук тика.
        /// </summary>
        public AudioClip Tik = default;

        /// <summary>
        /// Звук конца отсчета.
        /// </summary>
        public AudioClip EndDelay = default;
    }
}