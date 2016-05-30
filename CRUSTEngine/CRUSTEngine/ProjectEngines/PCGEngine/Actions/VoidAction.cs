using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
    public class VoidAction : CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action
    {
        public VoidAction() 
        {
            AType = ActionType.VoidAction;
        }

        public override void ExcecuteAction()
        {
            //if(ActionsExecuterGenSim.RyseFreq)
            //    StaticData.EngineManager.Update(new GameTime());
        }

        public override string ToString()
        {
            return "void_action";
        }
    }
}
