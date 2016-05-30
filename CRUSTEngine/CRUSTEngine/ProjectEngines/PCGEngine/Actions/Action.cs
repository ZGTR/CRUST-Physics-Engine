using System;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
	public abstract class Action
	{
        public ActionType AType;

	    public abstract void ExcecuteAction();
	}
}
