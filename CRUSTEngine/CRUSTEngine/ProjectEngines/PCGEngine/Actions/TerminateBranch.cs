using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
    public class TerminateBranch : CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action
    {
        public TerminateBranch() 
        {
            AType = ActionType.TerminateBranch;
        }

        public override void ExcecuteAction()
        {
            StaticData.EngineManager.Update(new GameTime());
        }

        public override string ToString()
        {
            return "terminate_branch";
        }
    }
}
