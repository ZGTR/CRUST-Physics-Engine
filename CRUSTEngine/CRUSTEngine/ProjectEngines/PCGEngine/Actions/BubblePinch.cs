using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions
{
    [Serializable]
    class BubblePinch : CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action
    {
        private int Id;
        [NonSerialized]
        public BubbleService BubblePinched;
        public BubblePinch(String[] pars)
        {
            this.Id = Int32.Parse(pars[0]);
            AType = ActionType.BubblePinch;
        }


        public BubblePinch(int id)
        {
            this.Id = id;
            AType = ActionType.BubblePinch;
        }

        public override void ExcecuteAction()
        {
            try
            {
                BubbleService bubbleService =  StaticData.EngineManager.BubbleManagerEngine.GetService(this.Id);
                if (bubbleService != null)
                {
                    BubblePinched = bubbleService;
                    bubbleService.IsClicked = true;
                }
            }
            catch (Exception)
            {
            }
        }

        public override string ToString()
        {
            return "bubble_pinch(" + this.Id +")";
        }
    }
}
