using System;
using System.Collections.Generic;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    [Serializable]
    public class ActionNode
    {
        public Action Action;
        public List<ActionNode> Childs = new List<ActionNode>();

        public ActionNode(Action a)
        {
            this.Action = a;
        }
    }
}
