namespace WaveShooter.Features.SwitchScenes
{
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Состояния перехода между сценами.
    /// </summary>
    public static class SceneFlow
    {
        private static string _loadingScene = "Loading";
        private static string _menuScene = "Menu";
        private static string _nextScene;

        /// <summary>
        /// Запоминает целевую сцену и грузит Loading.
        /// </summary>
        public static void Load(string sceneName)
        {
            _nextScene = sceneName;
            SceneManager.LoadScene(_loadingScene);
        }

        /// <summary>
        /// Отдает таргет сцену.
        /// </summary>
        public static string GetTargetScene()
        {
            string target = string.IsNullOrEmpty(_nextScene) ? _menuScene : _nextScene;
            _nextScene = null;
            return target;
        }
    }
}