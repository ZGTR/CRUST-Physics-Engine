using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim
{
    public class CompTimePair : CATimePair
    {
        public ComponentType CType;
        public String[] Args;

        public CompTimePair(ComponentType cType, int kt)
        {
            this.CType = cType;
            this.KeyTime = kt;
        }

        public override string ToString()
        {
            return CType.ToString();
        }
    }
}
