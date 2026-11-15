namespace WaveShooter.Features.UIElements
{
    using UnityEngine;

    /// <summary>
    /// Скрипт для выхода из игры.
    /// </summary>
    public sealed class QuitGameButton : ButtonView
    {
        protected override void OnClick() => QuitGame();
        private void QuitGame()
        {
            Time.timeScale = 1f;

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}