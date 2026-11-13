namespace WaveShooter.Features.Container
{
    using WaveShooter.Features.Facade;
    using UnityEngine;

    /// <summary>
    /// Установщик связи персонажа в контейнере
    /// </summary>
    public class InstallerCharacter : MonoBehaviour
    {
        [SerializeField] protected CharacterFacade characterFacade = default;

        [SerializeField] protected ContainerCharacter containerCharacter = default;

        protected virtual void Awake() => containerCharacter.FacadeInstance = characterFacade;

        protected virtual void OnDestroy() => containerCharacter.FacadeInstance = null;
    }
}