namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;

    /// <summary>
    /// Кнопка выхода из игры.
    /// </summary>
    public sealed class QuitGameButton : AbstractButtonView
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