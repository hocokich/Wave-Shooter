namespace WaveShooter.Features.Facade
{
    /// <summary>
    /// Generic версия DataModuleFacade с автоматическим кэшированием T.
    /// </summary>
    /// <typeparam name="T">Тип кэшируемого значения.</typeparam>
    public abstract class GenericDataModuleFacade<T> : DataModuleFacade where T : class
    {
        /// <summary>
        /// Кэшированное значение T.
        /// </summary>
        public T ConcreteFacade => concreteFacade;

        protected T concreteFacade = default;

        /// <summary>
        /// Установка кэша.
        /// </summary>
        /// <param name="value">Значение для кэша.</param>

        public sealed override void OnModuleInited()
        {
            concreteFacade = Facade as T;
            OnInited();
        }

        protected virtual void OnInited()
        {
            //TODO: На случай дополнительной логики на Inited воспользоваться override данного метода
        }
    }
}