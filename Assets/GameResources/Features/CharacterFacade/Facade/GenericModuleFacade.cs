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

        protected virtual void OnInited()
        {
            //TODO: На случай дополнительной логики на Inited воспользоваться override данного метода
        }
    }
}