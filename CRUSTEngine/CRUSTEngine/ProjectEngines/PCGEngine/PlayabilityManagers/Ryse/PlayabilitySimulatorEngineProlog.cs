using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    public class PlayabilitySimulatorEngineProlog
    {
        public const int TotalUpdateFrequency = 60;
        public int ActionsFrequency = 20;
        public const double NarrativeDist = 50;
        public bool IsCookieReachedTheFrog = false;
        public double ClosestCookieFrogDistance = 1000;
        public bool IsSaveImage = false;
        public List<int> DistancesList = new List<int>();
        public List<String> CookiePosList = new List<String>();
        public EngineShotsManager ShotsManager;
        //public long UpdateSoFar = 5 * 60;

        public PlayabilitySimulatorEngineProlog(bool isSaveImage)
        {
            this.IsSaveImage = isSaveImage;
            if (IsSaveImage)
                ShotsManager = new EngineShotsManager();
        }

        public double RunEngineFreely(int time, GameTime gameTime)
        {
            int counterTime = 0;
            while (counterTime < time)
            {
                StaticData.EngineManager.Update(gameTime);
                counterTime++;
            }
            return this.ClosestCookieFrogDistance;
        }

        public double RunEngineFreelyWithPlayabilityCheck(int time, GameTime gameTime)
        {
            int counterTime = 0;
            while (counterTime <= time)
            {
                if (!GenericHelperModule.CookieOutsideWindow())
                {
                    if (!SetPlayability())
                    {
                        StaticData.EngineManager.Update(gameTime);
                        counterTime++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return this.ClosestCookieFrogDistance;
        }

        public double RunEngine(ActionsGenerator actionsGenerator)
        {
            GameTime gameTime = new GameTime();
            bool canExecuteMore = true;
            bool reachedFrog = false;
            int counterTime = 1;
            bool firstTimeInLoop = true;
            if (IsSaveImage)
                ShotsManager.TakeEngineShot();

            while (true)
            {
                if (!SetPlayability())
                {
                    DistancesList.Add((int)GetCookieFrogDistance());
                    CookiePosList.Add(StaticData.EngineManager.CookieRB.PositionXNA.ToString());
                    if (firstTimeInLoop)
                    {
                        if (actionsGenerator.Actions.Count > 0)
                        {
                            //CookiePosList.Add(
                            //    actionsGenerator.Actions[actionsGenerator.currentActionIndex].AType.ToString());
                            canExecuteMore = actionsGenerator.ExecuteNextActions();
                            counterTime = 1;
                            //if(actionsGenerator.Actions[0] is VoidAction)
                            //{
                            //    counterTime = 10000;
                            //}
                            if (IsSaveImage)
                                ShotsManager.TakeEngineShot();
                        }
                        firstTimeInLoop = false;
                    }
                    if (counterTime <= ActionsFrequency)
                    {
                        StaticData.EngineManager.Update(gameTime);
                    }
                    else
                    {
                        break;
                    }
                    counterTime++;
                }
                else
                {
                    if (IsSaveImage)
                        ShotsManager.TakeEngineShot();
                    break;
                }
            }
            return ClosestCookieFrogDistance;
        }

        //public double RunEngine(ActionsGenerator actionsGenerator)
        //{
        //    GameTime gameTime = new GameTime();
        //    bool canExecuteMore = true;
        //    bool reachedFrog = false;
        //    int counterTime = 1;

        //    if (IsSaveImage)
        //        _shotsManager.TakeEngineShot();

        //    while (canExecuteMore)
        //    {
        //        if (!SetPlayability())
        //        {
        //            DistancesList.Add((int)GetCookieFrogDistance());
        //            CookiePosList.Add(StaticData.EngineManager.CookieRB.PositionXNA.ToString());
        //            StaticData.EngineManager.Update(gameTime);
        //            if (actionsGenerator.Actions.Count > 0)
        //            {
        //                if (counterTime == ActionsFrequency)
        //                {
        //                    CookiePosList.Add(
        //                        actionsGenerator.Actions[actionsGenerator.currentActionIndex].EType.ToString());
        //                    canExecuteMore = actionsGenerator.ExecuteNextActions();
        //                    counterTime = 0;
        //                    if (IsSaveImage)
        //                        _shotsManager.TakeEngineShot();
        //                }
        //            }
        //            else
        //            {
        //                canExecuteMore = false;
        //            }
        //            counterTime++;
        //        }
        //        else
        //        {
        //            if (IsSaveImage)
        //                _shotsManager.TakeEngineShot();
        //            reachedFrog = true;
        //            break;
        //        }
        //    }
        //    return ClosestCookieFrogDistance;
        //}

        private bool SetPlayability()
        {
            double dist = GetCookieFrogDistance();
            if (dist < NarrativeDist)
            {
                ClosestCookieFrogDistance = dist;
                IsCookieReachedTheFrog = true;
            }
            else
            {
                if (ClosestCookieFrogDistance > dist)
                {
                    ClosestCookieFrogDistance = dist;
                }
                IsCookieReachedTheFrog = false;
            }
            return IsCookieReachedTheFrog;
        }

        public bool IsPlayable()
        {
            return IsCookieReachedTheFrog;
        }

        public double GetCookieFrogDistance()
        {
            return RigidsHelperModule.GetDistance(StaticData.EngineManager.CookieRB, StaticData.EngineManager.FrogRB);
        }

    }
}
