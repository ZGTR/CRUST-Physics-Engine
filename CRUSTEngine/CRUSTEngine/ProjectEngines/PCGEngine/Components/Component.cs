using System;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components
{
    [Serializable]
    public abstract class Component
    {
        public int X;
        public int Y;

        public ComponentType CType;

        public abstract void AddSelfToEngine();
    }
}
