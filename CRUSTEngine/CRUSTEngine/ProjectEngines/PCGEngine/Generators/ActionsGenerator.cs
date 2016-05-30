using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine
{
    public class ActionsGenerator
    {
        public List<Action> Actions;
        public string ActionsStrGeva { get; set; }
        public int currentActionIndex = 0;

        public ActionsGenerator(String actionsStrGeva)
        {
            ActionsStrGeva = actionsStrGeva;
            this.Actions = ConvertToActions(ActionsStrGeva);
        }

        public ActionsGenerator(Action action)
        {
            this.Actions = new List<Action>(){action};
        }

        public ActionsGenerator(List<Action> listOfActions)
        {
            this.Actions = listOfActions;
        }

        public static List<Action> ConvertToActions(String pheno)
        {
            List<Action> actions = new List<Action>();
            String[] pStringArr = pheno.Trim().Split(')');
            pStringArr = StringHelper.GetPropperPhenoArrWithTerminals(pStringArr);
            for (int i = 0; i < pStringArr.Length; i++) 
            {
                try
                {
                    Action action = null;
                    String currentCmp = pStringArr[i].Trim();
                    if (!currentCmp.Contains("("))
                    {
                        action = GetNewAction(currentCmp, null);
                    }
                    else
                    {
                        String cmpName = currentCmp.Split('(')[0].Trim();
                        String[] pars = currentCmp.Split('(')[1].Trim().Split(',');
                        action = GetNewAction(cmpName, pars);
                    }
                    if (action != null)
                        actions.Add(action);
                }
                catch (Exception)
                { }
            }
            return actions;
        }

        private static Action GetNewAction(String cmpName, String[] pars) {
            if (cmpName.ToLower().Equals("blower_press"))
            {
                return new BlowerPress();
            }
            if (cmpName.ToLower().Equals("rope_cut"))
            {
                return new RopeCut(pars);
            }
            if (cmpName.ToLower().Equals("void_action"))
            {
                return new VoidAction();
            }
            if (cmpName.ToLower().Equals("bubble_pinch"))
            {
                return new BubblePinch(pars);
            }
            if (cmpName.ToLower().Equals("terminate_branch"))
            {
                return new TerminateBranch();
            }
            if (cmpName.ToLower().Equals("rocket_press"))
            {
                return new RocketPress();
            }
            return null;
        }

        //private void ExecuteActions()
        //{
        //    for (int i = 0; i < this.Actions.Count; i++)
        //    {
        //        this.Actions[i].ExcecuteAction(this.Engine);
        //    }
        //}

        public bool ExecuteNextActions()
        {
            // Execute the Action
            if (currentActionIndex < this.Actions.Count)
            {
                this.Actions[currentActionIndex].ExcecuteAction();
            }
            else
            {
                return false;
            }
            // Increment For Next Action
            if (currentActionIndex < this.Actions.Count - 1)
            {
                currentActionIndex++;
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
