namespace WaveShooter.Features.Container
{

    /// <summary>
    /// Провайдер персонажа для компонентов.
    /// </summary>
    public class ProviderCharacterComponent : ProviderCharacter
    {
        /// <summary>
        /// Свойство для обращения компонентов к контейнеру.
        /// </summary>
        protected override void OnFacadeInited(){ }
    }
}