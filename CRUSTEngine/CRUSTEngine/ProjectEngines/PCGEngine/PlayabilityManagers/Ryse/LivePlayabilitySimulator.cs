using System;
using System.Collections.Generic;
using System.Threading;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    class LivePlayabilitySimulator
    {
        public LivePlayabilitySimulator(EngineManager engineBase)
        {
            StaticData.EngineManager = engineBase;
        }

        public void SimulateNewWindow(List<Action> performedActions, bool saveStateToFile = false, bool isRyseSim = true)
        {
            //using 
            Game1 game1 = new Game1();
            {
                InitActionsExecuterClass(performedActions, isRyseSim);
                StaticData.EngineManager.Game1 = game1;

                ActionsExecuterGenSim.IsSaveStateToFile = saveStateToFile;
                game1.Run();
                ActionsExecuterGenSim.IsSaveStateToFile = saveStateToFile;
            }
            //StartThread(performedActions);
        }

        private void InitActionsExecuterClass(List<Action> performedActions, bool isRyseSim)
        {
            StaticData.GameSessionMode = SessionMode.PlayingMode;
            ActionsExecuterGenSim.ListOfActions = performedActions;
            ActionsExecuterGenSim.ActionsNotifManager = new ActionsNotificationManager(performedActions);
            ActionsExecuterGenSim.IsSimulatingGamePlayability = true;
            ActionsExecuterGenSim.nextActionIndex = 0;
            ActionsExecuterGenSim.isFinished = false;
            ActionsExecuterGenSim.CookiePosList = new List<string>();
            if (!isRyseSim)
                ActionsExecuterGenSim.RyseFreq = false;

        }

        void StartThread(List<Action> performedActions)
        {
            Thread thread = new Thread(new ThreadStart(() => RunXna(performedActions)));
            thread.Name = "XNA";
            thread.Start();
        }

        void RunXna(List<Action> performedActions)
        {
            try
            {
                Game1 game1 = new Game1();
                ActionsExecuterGenSim.IsSimulatingGamePlayability = true;
                ActionsExecuterGenSim.ListOfActions = performedActions;
                game1.Run();
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                {
                    //Do exception handling.
                }
            }
        }

        public void SimulateSameWindow(List<Action> performedActions, bool isRyseSim = true)
        {
            InitActionsExecuterClass(performedActions, isRyseSim);
        }
    }
}
