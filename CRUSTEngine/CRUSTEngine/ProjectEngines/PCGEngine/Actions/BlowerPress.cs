using System;
using System.Linq;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Actions
{
    [Serializable]
    class BlowerPress : Action
    {
        private int Id = -1;
        [NonSerialized]
        public BlowerService BlowerPressed;
        public BlowerPress() 
        {
            AType = ActionType.BlowerPress;
        }

        public BlowerPress(int id)
        {
            this.Id = id;
            AType = ActionType.BlowerPress;
        }

        public override void ExcecuteAction()
        {
            try
            {
                if (Id == -1)
                {
                    // Find nearest blower to the cookie and blow it up!
                    BlowerService blowerService =
                        RigidsHelperModule.CatchNearestVisual2D(StaticData.EngineManager.CookieRB,
                                                                StaticData.EngineManager.BlowerManagerEngine
                                                                          .ListOfServices.Select(
                                                                              blower => (Visual2D) blower).ToList(),
                                                                1000) as BlowerService;
                    if (blowerService != null)
                    {
                        BlowerPressed = blowerService;
                        // blow it
                        blowerService.AddForceToCookie();
                    }
                }
                else
                {
                    BlowerService blowerService = StaticData.EngineManager.BlowerManagerEngine.GetService(this.Id);
                    if (blowerService != null)
                    {
                        BlowerPressed = blowerService;
                        blowerService.AddForceToCookie();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public override string ToString()
        {
            return "blower_press(" + this.Id + ")";
        }
    }
}
