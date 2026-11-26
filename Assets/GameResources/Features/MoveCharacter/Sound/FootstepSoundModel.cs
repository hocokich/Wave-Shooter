namespace WaveShooter.Features.MoveCharacter.Sound
{
    using UnityEngine;

    /// <summary>
    /// Звуки шагов.
    /// </summary>
    [CreateAssetMenu(fileName = "Footstep Sound Model", menuName = "WaveShooter/Features/Move/FootstepSoundModel")]
    public sealed class FootstepSoundModel : ScriptableObject
    {
        [Header("Walk steps")]
        [Tooltip("Клипы шагов при ходьбе. Клип выбирается случайно.")]
        public AudioClip[] WalkSteps = default;

        [Header("Sprint steps")]
        [Tooltip("Клипы шагов при беге. Если пусто — используются WalkSteps.")]
        public AudioClip[] SprintSteps = default;
    }
}