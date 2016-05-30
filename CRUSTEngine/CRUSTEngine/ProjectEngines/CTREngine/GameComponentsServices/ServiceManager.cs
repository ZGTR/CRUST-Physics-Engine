using System;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices
{
    [Serializable]
    public abstract class ServiceManager
    {
        public abstract void AddNewService(IUpdatableComponent service);
        public abstract void RemoveService(IUpdatableComponent service);
    }
}
