namespace WaveShooter.Features.UIElements.Buttons
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Кнопка делает что-то по нажатию.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButtonView : MonoBehaviour
    {
        protected Button btn;
        protected virtual void Awake()
        {
            if(btn == null)
                btn = GetComponent<Button>();
        }

        protected virtual void OnEnable() => btn.onClick.AddListener(OnClick);
        protected virtual void OnDisable() => btn.onClick.RemoveListener(OnClick);

        protected virtual void OnClick()
        {
            //TODO по клику.
        }
    }
}