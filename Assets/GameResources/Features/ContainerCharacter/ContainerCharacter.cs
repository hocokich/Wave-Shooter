namespace WaveShooter.Features.Container
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контейнер персонажа
    /// </summary>
    [CreateAssetMenu(fileName = "New Container Character", menuName = "WaveShooter/Features/ContainerCharacter")]
    public class ContainerCharacter : ScriptableObject
    {
        /// <summary>
        /// Контейнер инициализирован
        /// </summary>
        public event Action onContainerInited = delegate { };

        /// <summary>
        /// Контейнер почищен
        /// </summary>
        public event Action onContainerDisposed = delegate { };

        /// <summary>
        /// Экземпляр фасада
        /// </summary>
        /// <value></value>
        public CharacterFacade FacadeInstance
        {
            get => characterFacade;
            set
            {
                if (value == characterFacade)
                {
                    characterFacade = value;
                    return;
                }

                if (characterFacade != null)
                {
                    onContainerDisposed();
                    characterFacade = null;
                }

                characterFacade = value;

                if (characterFacade != null)
                {
                    onContainerInited();
                }
            }
        }


        [SerializeField] protected CharacterFacade characterFacade = default;
    }
}