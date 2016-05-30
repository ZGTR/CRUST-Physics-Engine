using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
    public class RocketPress : CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action
    {
        [NonSerialized]
        public RocketCarrierService RocketPressed;
        public RocketPress() 
        {
            AType = ActionType.RocketPress;
        }

        public override void ExcecuteAction()
        {
            var rockets = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices();
            foreach (var rocketCarrierService in rockets)
            {
                if(rocketCarrierService.CanCatchCookie)
                {
                    if(rocketCarrierService.IsActivated)
                    {
                        RocketPressed = rocketCarrierService;
                        rocketCarrierService.IsClicked = true;
                    }
                }
            }
        }

        public override string ToString()
        {
            return "rocket_press";
        }
    }
}
