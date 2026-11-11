namespace WaveShooter.Features.SwitchScenes
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// Класс асинхронной загрузки сцены.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] protected Text  progressValue = default;
        [SerializeField] protected float timeFakeLoad  = 3f;

        private string targetScene;

        protected virtual void Start()
        {
            targetScene = SceneFlow.GetTargetScene();
            StartCoroutine(FakeLoad(timeFakeLoad));
        }

        protected virtual IEnumerator FakeLoad(float seconds)
        {
            float elapsed = 0f;

            while (elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / seconds);
                SetProgressPercent(Mathf.RoundToInt(Mathf.Lerp(0f, 50f, t)));
                yield return null;
            }
            SetProgressPercent(50);
            StartCoroutine(LoadGame());
        }
        protected virtual IEnumerator LoadGame()
        {
            var op = SceneManager.LoadSceneAsync(targetScene);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                float t = Mathf.Clamp01(op.progress / 0.9f);
                SetProgressPercent(Mathf.RoundToInt(Mathf.Lerp(50f, 100f, t)));
                yield return null;
            }

            SetProgressPercent(100);
            op.allowSceneActivation = true;
        }

        protected virtual void SetProgressPercent(int value)
        {
            if (progressValue == null) return;

            progressValue.text  = $"{value} %";
            progressValue.color = Color.Lerp(Color.red, Color.green, value / 100f);
        }
    }
}