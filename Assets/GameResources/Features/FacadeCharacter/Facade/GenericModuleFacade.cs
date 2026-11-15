namespace WaveShooter.Features.Facade
{
    /// <summary>
    /// Generic версия ModuleFacade с автоматическим кэшированием T.
    /// </summary>
    /// <typeparam name="T">Тип кэшируемого значения.</typeparam>
    public abstract class GenericModuleFacade<T> : ModuleFacade where T : class
    {
        /// <summary>
        /// Кэшированное значение T.
        /// </summary>
        public T ConcreteFacade => concreteFacade;
        
        protected T concreteFacade = default;

        public sealed override void OnModuleInited()
        {
            concreteFacade = facade as T;
            OnInited();
        }
        public sealed override void OnModuleDisposed()
        {
            concreteFacade = facade as T;
            OnDisposed();
        }

        protected virtual void OnInited()
        {
            //TODO: На случай дополнительной логики на Inited воспользоваться override данного метода
        }
        protected virtual void OnDisposed()
        {
            //TODO: На случай дополнительной логики на Disposed воспользоваться override данного метода
        }
    }
}