using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim
{
    public class ActionTimePair : CATimePair
    {
        public EventType EType;

        public ActionTimePair(EventType eType, int kt)
        {
            this.EType = eType;
            this.KeyTime = kt;
        }

        public override string ToString()
        {
            return EType.ToString();
        }
    }
}
