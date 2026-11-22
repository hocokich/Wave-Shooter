namespace WaveShooter.Features.GameState
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Модель состояний игры.
    /// </summary>
    [CreateAssetMenu(fileName = "Game State Model", menuName = "WaveShooter/Features/GameStateModel")]
    public sealed class GameStateModel : ScriptableObject
    {
        /// <summary>
        /// Состояние изменилось.
        /// </summary>
        public event Action<GameState> onStateChanged = delegate { };

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        public GameState CurrentState { get; private set; } = GameState.Load;

        /// <summary>
        /// Перевести игру в новое состояние.
        /// Повторный перевод в текущее состояние игнорируется.
        /// </summary>
        public void SetState(GameState next)
        {
            if (CurrentState == next) return;

            CurrentState = next;
            onStateChanged.Invoke(next);
        }

        public void ResetState()
        {
            CurrentState = GameState.Load;
        }
    }
}