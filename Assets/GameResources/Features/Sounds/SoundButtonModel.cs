namespace WaveShooter.Features.Sounds
{
    using UnityEngine;

    /// <summary>
    /// Звуки для кнопки.
    /// </summary>
    [CreateAssetMenu(fileName = "Sound Button Model", menuName = "WaveShooter/Features/SoundButtonModel")]
    public sealed class SoundButtonModel : ScriptableObject
    {
        [Header("Button events")]

        /// <summary>
        /// Звук по нажатию.
        /// </summary>
        public AudioClip Pressed = default;

        /// <summary>
        /// Звук по наведению.
        /// </summary>
        public AudioClip Highlighted = default;
    }}