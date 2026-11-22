namespace WaveShooter.Features.GameState
{
    /// <summary>
    /// Состояния игры.
    /// </summary>
    public enum GameState
    {
        Load = 0,
        DelayGameplay = 1,
        Gameplay = 2,
        Pause = 3,
        GameOver = 4
    }
}