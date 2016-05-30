using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    class ActionsExecuterGenSim
    {
        private static int actionFreq = 0;
        private static long updateIdForNextAction = 0;
        private static bool waitForNextAction = false;
        public static bool isFinished = false;
        public static int nextActionIndex = 0;
        public static bool IsSaveStateToFile = false;
        public static ActionsNotificationManager ActionsNotifManager;
        public static bool IsSimulatingGamePlayability = false;
        public static List<Action> ListOfActions;
        public static bool IsSaveImage = false;
        private static EngineShotsManager _shotsManager;
        public static bool RyseFreq = true;
        public static List<String> CookiePosList;

        private static int actionId = 0;
        public static void ManipulateActions(GameTime gameTime)
        {
            if (IsSimulatingGamePlayability)
            {
                if (ListOfActions != null)
                {
                    if (ActionsNotifManager != null)
                        ActionsNotifManager.Update(gameTime);
                    if (!isFinished)
                    {
                        //if (!waitForNextAction)
                        {
                            if (IsSaveStateToFile)
                                FilesHelperModule.SaveToStateFile(ListOfActions[nextActionIndex]);
                            Action currentAction = ListOfActions[nextActionIndex];
                            if (!(currentAction is VoidAction))
                                if (ActionsNotifManager != null)
                                    ActionsNotifManager.PushNextNotification();
                            if (IsSaveImage)
                            {
                                _shotsManager = _shotsManager ?? new EngineShotsManager();
                                _shotsManager.TakeEngineShot(false);
                            }

                            //if (!(currentAction is VoidAction))
                            //{
                            //    if (GenSimAgent.UpdateActionsIds.Count > actionId)
                            //    {
                            //        var res = GenSimAgent.UpdateActionsIds[actionId];
                            //        long uGen = res.UpdateId;
                            //        Action actionGen = res.Action;

                            //        long i = StaticData.UpdatesSoFar;
                            //        Action action = currentAction;
                            //    }
                            //}
                            actionId++;
                            CookiePosList.Add(StaticData.EngineManager.CookieRB.PositionXNA.ToString());
                            currentAction.ExcecuteAction();

                            if (RyseFreq)
                            {
                                actionFreq = RyseAgent.GetActionsFrequency(currentAction);
                            }
                            else
                            {
                                actionFreq = GenSimAgent.GetActionsFrequency(currentAction);
                            }
                            updateIdForNextAction = StaticData.UpdatesSoFar + actionFreq;
                            //waitForNextAction = true;
                        }
                        //else
                        //{
                        //if (StaticData.UpdatesSoFar == updateIdForNextAction)
                        {
                            //StaticData.UpdatesSoFar = 0;
                            //waitForNextAction = false;
                            if (nextActionIndex < ListOfActions.Count - 1)
                                nextActionIndex++;
                            else
                            {
                                isFinished = true;
                            }
                        }
                        //}
                    }
                    else
                    {
                        ActionsNotifManager = null;
                        //StaticData.EngineManager.Game1.TargetElapsedTime = TimeSpan.FromSeconds(1.0f/60.0f);
                    }
                }
            }
        }

        public static List<String> GetCookiePosList()
        {
            return CookiePosList;
        }
    }
}
