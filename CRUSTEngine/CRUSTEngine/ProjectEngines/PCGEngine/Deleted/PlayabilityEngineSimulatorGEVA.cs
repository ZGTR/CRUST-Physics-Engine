//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using CRUSTEngine.ProjectEngines.HelperModules;

//namespace CRUSTEngine.ProjectEngines.PCGEngine
//{
//    class PlayabilityEngineSimulatorGEVA
//    {
//        public const int TotalUpdateFrequency = 60;
//        public const int ActionsFrequency = 15;
//        public const double NarrativeDist = 32;
//        public bool IsCookieReachedTheFrog = false;
//        public double ClosestCookieFrogDistance = 1000;
//        public static bool IsSaveImage = false;
//        public List<int> DistancesList = new List<int>();
//        public List<String> CookiePosList = new List<String>();

//        public static int DirNr = 0;
//        public static int ImageCounter = 0;

//        public PlayabilityEngineSimulatorGEVA()
//        {
//            this.SetNewDirNumber();
//        }

//        private int SetNewDirNumber()
//        {
//            for (int i = 0; i < 1000; i++)
//            {
//                if (!Directory.Exists(i.ToString()))
//                {
//                    Directory.CreateDirectory(i.ToString());
//                    DirNr = i;
//                    break;
//                }
//            }
//            return DirNr;
//        }

//        public void ShowXNAWindow()
//        {
//            using (Game1 game = new Game1())
//            {
//                game.Run();
//            }
//        }

//        private void TakeEngineShot()
//        {
//            using (Game1 game1 = new Game1())
//            {
//                game1.SetOpacity(0);
//                game1.IsDrawJustOneTime = true;
//                game1.Run();
//                game1.SaveFrame(DirNr, ImageCounter);
//                ImageCounter++;
//            }
//        }

//        public void RunEngineFreely(int time, GameTime gameTime)
//        {
//            int counterTime = 0;
//            while (counterTime <= time)
//            {
//                StaticData.EngineManager.Update(gameTime);
//                counterTime++;
//            }
//        }

//        private void RunEngineFreelyWithPlayabilityCheck(int time, GameTime gameTime)
//        {
//            int counterTime = 0;
//            while (counterTime <= time)
//            {
//                if (!SetPlayability())
//                {
//                    StaticData.EngineManager.Update(gameTime);
//                    counterTime++;
//                }
//                else
//                {
//                    break;
//                }
//            }
//        }

//        public double RunEngine(ActionsGenerator actionsGenerator, bool withResting)
//        {
//            GameTime gameTime = new GameTime();
//            bool canExecuteMore = true;
//            bool reachedFrog = false;
//            int counterTime = 0;

//            if (IsSaveImage)
//                TakeEngineShot();

//            while (canExecuteMore)
//            {
//                if (!SetPlayability())
//                {
//                    DistancesList.Add((int)GetCookieFrogDistance());
//                    CookiePosList.Add(StaticData.EngineManager.CookieRB.PositionXNA.ToString());
//                    StaticData.EngineManager.Update(gameTime);
//                    if (actionsGenerator.Actions.Count > 0)
//                    {
//                        if (counterTime == ActionsFrequency)
//                        {
//                            CookiePosList.Add(
//                                actionsGenerator.Actions[actionsGenerator.currentActionIndex].EType.ToString());
//                            canExecuteMore = actionsGenerator.ExecuteNextActions();
//                            counterTime = 0;
//                            if (IsSaveImage)
//                                TakeEngineShot();
//                        }
//                    }
//                    else
//                    {
//                        canExecuteMore = false;
//                    }
//                    counterTime++;
//                }
//                else
//                {
//                    if (IsSaveImage)
//                        TakeEngineShot();
//                    reachedFrog = true;
//                    break;
//                }
//            }
//            if (withResting)
//            {
//                if (!reachedFrog)
//                {
//                    RunEngineFreelyWithPlayabilityCheck(5 * 60, gameTime);
//                }
//            }
//            return ClosestCookieFrogDistance;
//        }

//        private bool SetPlayability()
//        {
//            double dist = GetCookieFrogDistance();
//            if (dist < NarrativeDist)
//            {
//                ClosestCookieFrogDistance = dist;
//                IsCookieReachedTheFrog = true;
//            }
//            else
//            {
//                if (ClosestCookieFrogDistance > dist)
//                {
//                    ClosestCookieFrogDistance = dist;
//                }
//                IsCookieReachedTheFrog = false;
//            }
//            return IsCookieReachedTheFrog;
//        }

//        public bool IsPlayable()
//        {
//            return IsCookieReachedTheFrog;
//        }

//        private double GetCookieFrogDistance()
//        {
//            return RigidsHelperModule.GetDistance(StaticData.EngineManager.CookieRB, StaticData.EngineManager.FrogRB);
//        }
//    }
//}
