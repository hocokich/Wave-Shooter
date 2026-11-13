namespace WaveShooter.Features.Facade
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Функциональный модуль фасада, должен располагаться на самом фасаде или в child gameObjects
    /// </summary>
    public abstract class ModuleFacade : MonoBehaviour
    {
        /// <summary>
        /// Модуль инициализирован
        /// </summary>
        public event Action onModuleInited = delegate { };

        /// <summary>
        /// Модуль удален
        /// </summary>
        public event Action onModuleDisposed = delegate { };

        /// <summary>
        /// Фасад модуля
        /// </summary>
        /// <value></value>
        public virtual AbstractFacade Facade
        {
            get => facade;
            set
            {
                facade = value;
                onModuleInited();
            }
        }

        /// <summary>
        /// Идентификатор модуля
        /// </summary>
        public string Id => id;

        [SerializeField] protected string id = string.Empty;
        [SerializeField] protected AbstractFacade facade = default;

        /// <summary>
        /// Инициализация фасада
        /// </summary>
        /// <param name="_facade"></param>
        public void Initialize(AbstractFacade _facade)
        {
            Facade = _facade;
            OnModuleInited();
        }

        /// <summary>
        /// Удаление модуля
        /// </summary>
        public void Dispose()
        {
            onModuleDisposed.Invoke();

            Facade = null;
            OnModuleDisposed();
        }

        /// <summary>
        /// Метод срабатывающий по факту инициализации модуля фасада
        /// </summary>
        public abstract void OnModuleInited();

        /// <summary>
        /// Метод срабатывающий по факту удаления модуля фасада
        /// </summary>
        public abstract void OnModuleDisposed();
    }
}