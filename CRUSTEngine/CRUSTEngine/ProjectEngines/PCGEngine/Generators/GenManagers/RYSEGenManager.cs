
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators
{
    static class RYSEGenManager
    {
        public static string StrLevelTesting =
            "cookie(392, 232)  frog(457, 63)  rope(358, 84,150)  rope(489, 138,180)  blower(503, 273, 0)  bubble(1, 441, 394)  rocket(633, 235, 5)  bump(439, 489, 0)  bump(435, 256, 7)  bump(506, 228, 0)  ";
            //"cookie( 300 , 120 ) frog( 360 , 300 )  rope( 550 , 50 , 100 ) rope( 440 , 50 , 180 ) blower( 490 , 180 , 4 )";
            //"cookie(405, 130)  frog(320, 70)  rope(0, 326, 116, 100)  rope(1, 705, 149, 180)  rocket(672, 351, 5) bump(300, 150, 1) bump(300, 200, 1) bump(300, 250, 1)  bump(300, 300, 1)";    

            //"cookie(405, 130)  frog(400, 418)  rope(0, 326, 116, 100)  rope(1, 705, 149, 180)  bubble(7, 672, 351) rocket(720, 60, 3) bump(300, 150, 1) bump(300, 200, 1) bump(300, 250, 1)  bump(300, 300, 1)";    
            //"cookie(405, 130)  frog(450, 450)  rope(1, 705, 149, 180)  rocket(672, 351, 5) bump(300, 150, 1) bump(300, 200, 1) bump(300, 250, 1)  bump(300, 300, 1)";    

            //"cookie(471, 99)  frog(336, 426)  rope(0, 315, 44, 60)  rope(1, 455, 43, 100)  rope(2, 690, 31, 170)  blower(680, 400, 4)  bubble(0, 600, 320)  rocket(458, 262, 3)  bump(715, 165, 2)  bump(714, 236, 2)  bump(475, 206, 0)  bump(409, 207, 0)  bump(339, 207, 0)  bump(502, 381, 5)  bump(526, 442, 2) bump(526, 520, 2)";    
            //"cookie(471, 99)  frog(336, 426)  rope(2, 620, 31, 170)  blower(650, 400, 4)  bubble(0, 600, 320)  rocket(458, 262, 3)  bump(715, 165, 2)  bump(714, 236, 2)  bump(475, 206, 0)  bump(409, 207, 0)  bump(339, 207, 0)  bump(502, 381, 5)  bump(526, 442, 2) bump(526, 520, 2)";

            //"cookie(471, 99)  frog(336, 426)  rope(2, 620, 31, 170)  blower(650, 350, 4)  bubble(0, 600, 320)  rocket(458, 262, 3)  bump(715, 165, 2)  bump(714, 236, 2)  bump(475, 206, 0)  bump(409, 207, 0)  bump(339, 207, 0)  bump(502, 381, 5)  bump(526, 442, 2) bump(526, 500, 2)";

            //"cookie(700, 239)  frog(356, 408)  rope(0, 706, 103, 180)  blower(746, 232, 4)  bubble(0, 597, 240)  rocket(600, 64, 3) bump(536, 210, 2)  bump(536, 270, 2) bump(536, 339, 2)  bump(537, 408, 2)  bump(537, 478, 2)  ";

        public static void GenerateLevelTesting()
        {
            LevelBuilder.CreateRestedLevel(StrLevelTesting, false);
            String str = EngineStateManager.GetEngineStateFactStringWithEnterDelimiterToProlog();
            bool IsDesign = false;
            bool isSaveToFile = true;
            if (!IsDesign)
            {
                ActionsExecuterGenSim.IsSaveStateToFile = true;
                RyseAgent agent = new RyseAgent(0);
                agent.IsSaveImage = false;
                agent.Simulator = new PlayabilitySimulatorEngineProlog(agent.IsSaveImage);
                bool playability = false;
                List<Action> performedActions = new List<Action>();
                List<Vector3> performedVel = new List<Vector3>();
                EngineManager engine = ObjectSerializer.DeepCopy(StaticData.EngineManager);
                DateTime d1 = DateTime.Now;
                ActionNode baseNode = new ActionNode(null);
                agent.SimulatePlayability(baseNode, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.NormalCheck);
                DateTime d2 = DateTime.Now;
                agent.totalTime = (int)(d2 - d1).TotalSeconds;
                int avgLevels = 0;
                if (agent.NrOfTerminates > 0)
                    avgLevels = (int)(agent.TerminateLevelSum / agent.NrOfTerminates);
                int nrOfNonVoids =
                    agent.bestPerformedActions.FindAll(delegate(Action a) { return !(a is VoidAction); }).Count;
                //FilesHelperModule.PrintTreeToFile(baseNode);
                MessageBox.Show("The level is " + (playability ? "PLAYABLE" : "NOT PLAYABLE") + Environment.NewLine
                                + "Finished processing in = " + agent.totalTime.ToString() + " sec." +
                                Environment.NewLine
                                + "Nodes explored = " + agent.nodesExplored + Environment.NewLine
                                + "Nr of tree cuts = " + agent.NrOfTerminates + Environment.NewLine
                                + "Average tree depth cut = " + avgLevels + Environment.NewLine
                                + "Solution tree depth = " + agent.bestPerformedActions.Count + Environment.NewLine
                                + "Best distance = " + agent.Simulator.ClosestCookieFrogDistance + Environment.NewLine
                                + "Best Performed Actions are = " +
                                HelperModules.GenericHelperModule.GetActionsString(agent.bestPerformedActions) +
                                Environment.NewLine);

                if (isSaveToFile)
                {
                    string strFile = "0"
                                     + "\t" + playability
                                     + "\t" + agent.prologTime/1000
                                     + "\t" + agent.totalTime
                                     + "\t" + String.Format("{0:0.00}", agent.bestClosestFrogCookieDist)
                                     + "\t" + agent.maxDepthArr
                                     + "\t" + agent.NrOfTerminates
                                     + "\t" + avgLevels
                                     + "\t" + agent.NrLevelterminatesStdMin
                                     + "\t" + agent.NrLevelterminatesStdMax
                                     + "\t" + agent.nodesExplored
                                     + "\t" + agent.bestPerformedActions.Count
                                     + "\t" + nrOfNonVoids
                                     + "\t" + StrLevelTesting
                                     + "\t" +
                                     HelperModules.GenericHelperModule.GetActionsString(agent.bestPerformedActions)
                                     + "\t" + HelperModules.GenericHelperModule.GetVector3ListString(performedVel);

                    StreamWriter sw = new StreamWriter(@"C:\CTREngine\LevelsTestTemp.txt");
                    sw.WriteLine(strFile);
                    sw.Flush();
                    sw.Close();
                }
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                if (!IsDesign)
                {
                    if (playability)
                    {
                        LivePlayabilitySimulator liveSim = new LivePlayabilitySimulator(engine);
                        liveSim.SimulateNewWindow(performedActions);
                    }
                }
            }
        }

        public static void GenerateGevaLevelEvolvePlayabilityFFForCSharp(bool isPrintPositionOnly, bool isSaveImage, int voidInitPlayTotalCount)
        {
            String[] args;
            StreamReader sr = new StreamReader(@"C:\CTREngine\LevelToPlayCheckDesigner.txt");
            String gevaStr = sr.ReadLine();
            sr.Close();
            args = new string[2];
            args[0] = "0";
            args[1] = gevaStr;
            GenerateGevaLevelEvolvePlayabilityFF(args, isPrintPositionOnly, isSaveImage, voidInitPlayTotalCount);
        }

        public static void GenerateGevaLevelEvolvePlayabilityFF(String[] args, bool isPrintPositionOnly, bool isSaveImage, int voidInitPlayTotalCount)
        {
            StaticData.EngineManager = null;
            LevelBuilder.CreateRestedLevel(args[1], true);
            var engineInit = StaticData.EngineManager;
            RyseAgent agent = new RyseAgent(voidInitPlayTotalCount);
            agent.IsPrintPositionOnly = isPrintPositionOnly;
            agent.IsSaveImage = isSaveImage;
            agent.Simulator = new PlayabilitySimulatorEngineProlog(isSaveImage);
            bool playability = false;
            EngineManager engine = ObjectSerializer.DeepCopy(StaticData.EngineManager);
            List<Action> performedActions = new List<Action>();
            List<Vector3> performedVel = new List<Vector3>();
            double closestDistSoFar = Double.MaxValue;
            performedActions = new List<Action>();
            StaticData.EngineManager = engine;
            DateTime d1 = DateTime.Now;
            ActionNode baseNode = new ActionNode(new VoidAction());
            agent.SimulatePlayability(baseNode, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.NormalCheck);
            DateTime d2 = DateTime.Now;
            FilesHelperModule.DeepCopyTreeToFile(baseNode);
            FilesHelperModule.PrintTreeToFile(baseNode, engineInit, performedActions);
            agent.totalTime = (int)((d2 - d1).TotalSeconds);
            if (agent.bestClosestFrogCookieDist < closestDistSoFar)
            {
                closestDistSoFar = agent.bestClosestFrogCookieDist;
            }
            //if (performedActions.Count <= 3)
            //{
            //    closestDistSoFar = 70;
            //}
            if (closestDistSoFar > 1000)
            {
                closestDistSoFar = 1000;
            }
            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(closestDistSoFar);
            swGEVA.Close();

            int avgLevels = 0;
            if (agent.NrOfTerminates > 0)
                avgLevels = (int)(agent.TerminateLevelSum / agent.NrOfTerminates);
            int nrOfNonVoids =
                agent.bestPerformedActions.FindAll(delegate(Action a) { return !(a is VoidAction); }).Count;

            string strFile = "0"
                             + "\t" + playability
                             + "\t" + agent.prologTime/1000
                             + "\t" + agent.totalTime
                             + "\t" + String.Format("{0:0.00}", agent.bestClosestFrogCookieDist)
                             + "\t" + agent.maxDepthArr
                             + "\t" + agent.NrOfTerminates
                             + "\t" + avgLevels
                             + "\t" + agent.NrLevelterminatesStdMin
                             + "\t" + agent.NrLevelterminatesStdMax
                             + "\t" + agent.nodesExplored
                             + "\t" + agent.bestPerformedActions.Count
                             + "\t" + nrOfNonVoids
                             + "\t" + args[1]
                             + "\t" + HelperModules.GenericHelperModule.GetActionsString(agent.bestPerformedActions)
                             + "\t" + HelperModules.GenericHelperModule.GetVector3ListString(performedVel);
            if (closestDistSoFar <= 50)
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PhysicsEngine_EvolvePlayActions.txt");
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PhysicsEngine_EvolvePlayActionsNonPlayable.txt");
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PhysicsEngine_EvolvePlayActionsAllLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
        }


        public static void GenerateGevaLevelRandPlayabilityFF(String[] args, bool isPrintPositionOnly, bool isSaveImage, int voidInitPlayTotalCount)
        {
            StaticData.EngineManager = null;
            LevelBuilder.CreateRestedLevel(args[1], true);
            RyseAgent agent = new RyseAgent(voidInitPlayTotalCount);
            agent.IsPrintPositionOnly = isPrintPositionOnly;
            agent.IsSaveImage = isSaveImage;
            agent.Simulator = new PlayabilitySimulatorEngineProlog(isSaveImage);
            bool playability = false;
            EngineManager engine = ObjectSerializer.DeepCopy(StaticData.EngineManager);
            DateTime d1 = DateTime.Now;
            List<Action> performedActions = new List<Action>();
            List<Vector3> performedVel = new List<Vector3>();
            double closestDistSoFar = Double.MaxValue;
            for (int j = 0; j < 10; j++)
            {
                if (!playability)
                {
                    performedActions = new List<Action>();
                    StaticData.EngineManager = engine;
                    ActionNode node = new ActionNode(new VoidAction());
                    agent.SimulatePlayability(node, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.RandomCheck);
                    if (agent.bestClosestFrogCookieDist < closestDistSoFar)
                    {
                        closestDistSoFar = agent.bestClosestFrogCookieDist;
                    }
                    if (performedActions.Count <= 3)
                    {
                        closestDistSoFar = 70;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            DateTime d2 = DateTime.Now;
            agent.totalTime = (int)((d2 - d1).TotalMilliseconds);
            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(closestDistSoFar);
            swGEVA.Close();

            int avgLevels = 0;
            if (agent.NrOfTerminates > 0)
                avgLevels = (int)(agent.TerminateLevelSum / agent.NrOfTerminates);
            int nrOfNonVoids =
                agent.bestPerformedActions.FindAll(delegate(Action a) { return !(a is VoidAction); }).Count;
            String strFile = "1"
                             + "\t" + playability
                             + "\t" + agent.prologTime/1000
                             + "\t" + agent.totalTime
                             + "\t" + String.Format("{0:0.00}", agent.bestClosestFrogCookieDist)
                             + "\t" + agent.maxDepthArr
                             + "\t" + agent.NrOfTerminates
                             + "\t" + avgLevels
                             + "\t" + agent.NrLevelterminatesStdMin
                             + "\t" + agent.NrLevelterminatesStdMax
                             + "\t" + agent.nodesExplored
                             + "\t" + agent.bestPerformedActions.Count
                             + "\t" + nrOfNonVoids
                             + "\t" + args[1]
                             + "\t" + HelperModules.GenericHelperModule.GetActionsString(agent.bestPerformedActions)
                             + "\t" + HelperModules.GenericHelperModule.GetVector3ListString(performedVel);
            if (closestDistSoFar <= 50)
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PhysicsEngine_EvolvePlayActions.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\PhysicsEngine_EvolvePlayActionsAllLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
        }

        public static void TestPlayabilityFromLevelsFile(string path, bool isSaveImage)
        {
            //OpenFileDialog op = new OpenFileDialog();
            //op.Title = @"Choose levels file";
            String pathReader = path;
            //if (op.ShowDialog() == DialogResult.OK)
            //{
            //    pathReader = op.FileName;
            //}
            StreamReader sr = new StreamReader(pathReader);
            StreamWriter sw = new StreamWriter(@"ResultsPlayabilityCheck.txt", true);
            String currentLevel = String.Empty;
            while ((currentLevel = sr.ReadLine()) != null)
            {
                LevelBuilder.CreateRestedLevel(currentLevel, true);
                RyseAgent agent = new RyseAgent(10);
                agent.IsSaveImage = isSaveImage;
                agent.maxDepthArr = 0;
                agent.prologTime = 0;
                agent.nodesExplored = 0;
                agent.bestClosestFrogCookieDist = Double.MaxValue;
                agent.bestPerformedActions = new List<Action>();
                agent.Simulator = new PlayabilitySimulatorEngineProlog(agent.IsSaveImage);
                bool playability = false;
                List<Action> performedActions = new List<Action>();
                List<Vector3> performedVel = new List<Vector3>();

                DateTime d1 = DateTime.Now;
                ActionNode node = new ActionNode(new VoidAction());
                agent.SimulatePlayability(node, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.NormalCheck);
                DateTime d2 = DateTime.Now;
                agent.totalTime = (int)((d2 - d1).TotalSeconds);

                // Write to file

                sw.WriteLine(playability
                             + "\t" + agent.prologTime / 1000
                             + "\t" + agent.totalTime
                             + "\t" + String.Format("{0:0.00}", agent.bestClosestFrogCookieDist)
                             + "\t" + agent.maxDepthArr
                             + "\t" + agent.nodesExplored
                             + "\t" + currentLevel
                             + "\t" + HelperModules.GenericHelperModule.GetActionsString(agent.bestPerformedActions)
                             );
                Console.WriteLine(playability);
                // write to db
                //DatabaseHandler.InsertToPlayabilityTestTable(1,
                //                                             playability,
                //                                             prologTime / 1000, 
                //                                             totalTime,
                //                                             String.Format("{0:0.00}", bestClosestFrogCookieDist),
                //                                             maxDepthArr,
                //                                             nodesExplored,
                //                                             _strLevel,
                //                                             HelperModules.GenericHelperModule.GetActionsString(bestPerformedActions));
                sw.Flush();
            }
            sw.Close();
        }

        //public static RYSEManager SimulatePlayabiltityFromDesigner(bool isSaveImage)
        //{
        //    RYSEManager manager = new RYSEManager(10);
        //    manager.IsSaveImage = isSaveImage;
        //    manager.Simulator = new PlayabilitySimulatorEngineProlog(manager.IsSaveImage);
        //    bool playability = false;
        //    List<Action> performedActions = new List<Action>();
        //    List<Vector3> performedVel = new List<Vector3>();
        //    EngineManager engine = ObjectSerializer.DeepCopy(StaticData.EngineManager);
        //    DateTime d1 = DateTime.Now;
        //    ActionNode node = new ActionNode(new VoidAction());
        //    manager.SimulatePlayability(node, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.NormalCheck);
        //    DateTime d2 = DateTime.Now;
        //    manager.totalTime = (int)(d2 - d1).TotalSeconds;
        //    //if (playability == true)
        //    //{
        //    //    MessageBox.Show("Playable! " + this.Simulator.ClosestCookieFrogDistance);
        //    //}
        //    if (playability)
        //    {
        //        MessageBox.Show("The level is Playable!" + Environment.NewLine
        //                        + "Finished processing in = " + manager.totalTime.ToString() + " sec." + Environment.NewLine
        //                        + "Nodes explored = " + manager.nodesExplored + Environment.NewLine
        //                        + "Nr of tree cuts = " + manager.NrOfTerminates + Environment.NewLine
        //                        + "Solution tree depth = " + performedActions.Count + Environment.NewLine
        //                        + "Best distance = " + manager.Simulator.ClosestCookieFrogDistance);
        //        LivePlayabilitySimulator liveSim = new LivePlayabilitySimulator(engine);
        //        liveSim.SimulateSameWindow(performedActions);
        //    }
        //    else
        //    {
        //        MessageBox.Show(@"Can't find a solution for this level.");
        //    }
        //    //StaticData.EngineManager.Game1 = DesignerManager.Game;
        //    return manager;
        //}


        //public static RYSEManager SimulatePlayabiltityFromDesigner(bool isSaveImage)
        //{
        //    String gevaStr = EngineStateManager.GetEngineStateFactStringWithSpaceDelimiterGEVAStyle();
        //    //LevelBuilder.CreateRestedLevel(gevaStr, false);
        //    RYSEManager manager = new RYSEManager(10);
        //    manager.Simulator = new PlayabilitySimulatorEngineProlog(false);
        //    manager.IsSaveImage = false;
        //    StaticData.GameSessionMode = SessionMode.PlayingMode;

        //    bool playability = false;
        //    List<Action> performedActions = new List<Action>();
        //    List<Vector3> performedVel = new List<Vector3>();
        //    DateTime d1 = DateTime.Now;
        //    ActionNode baseNode = new ActionNode(new VoidAction());
        //    manager.SimulatePlayability(baseNode, StaticData.EngineManager, ref playability, 0, performedActions, performedVel, 0, PlayabilityCheckMode.NormalCheck);
        //    DateTime d2 = DateTime.Now;
        //    manager.totalTime = (int)(d2 - d1).TotalSeconds;
        //    FilesHelperModule.PrintTreeToFile(baseNode);
        //    FilesHelperModule.DeepCopyTreeToFile(baseNode);
        //    if (playability)
        //    {
        //        MessageBox.Show("The level is Playable!" + Environment.NewLine
        //                        + "Finished processing in = " + manager.totalTime.ToString() + " sec." + Environment.NewLine
        //                        + "Nodes explored = " + manager.nodesExplored + Environment.NewLine
        //                        + "Nr of tree cuts = " + manager.NrOfTerminates + Environment.NewLine
        //                        + "Solution tree depth = " + performedActions.Count + Environment.NewLine
        //                        + "Best distance = " + manager.Simulator.ClosestCookieFrogDistance);

        //        StaticData.EngineManager.Game1 = DesignerManager.Game;
        //        LevelBuilder.CreateRestedLevel(gevaStr, false);
        //        FilesHelperModule.PrintTreeToFile(baseNode, StaticData.EngineManager, performedActions);
        //        StaticData.GameSessionMode = SessionMode.PlayingMode;
        //        LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
        //        simulator.SimulateSameWindow(performedActions);
        //    }
        //    else
        //    {
        //        MessageBox.Show(@"Can't find a solution for this level.");
        //    }
        //    StaticData.EngineManager.Game1 = DesignerManager.Game;
        //    return manager;
        //}

        public static void SimulatePlayabiltityFromDesigner()
        {
            var strGeva = EngineStateManager.GetEngineStateFactStringWithSpaceDelimiterGEVAStyle();
            StreamWriter sw = new StreamWriter(@"C:\CTREngine\LevelToPlayCheckDesigner.txt");
            sw.WriteLine(strGeva);
            sw.Close();
            //System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            //proc.FileName = @"C:\CTREngine\CRUSTEngine_PlayabilityChecker.exe";
            //proc.Arguments = "0" + " " + @strGeva;
            //proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //System.Diagnostics.Process.Start(proc);


            var process = new Process();
            process.EnableRaisingEvents = false;
            process.StartInfo.FileName = @"C:\CTREngine\CRUSTEngine_PlayabilityChecker_ToDesigner.exe";
            process.StartInfo.Arguments = "";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Close();

            StreamReader sr =
                    new StreamReader(@"C:\CTREngine\PhysicsEngine_EvolvePlayActions.txt");
            String line = sr.ReadToEnd();
            DesignEnhanceManager.GevaLevel = line.Split('\t')[13];
            DesignEnhanceManager.PlayabilityActions = line.Split('\t')[14];
            if (line.Split('\t')[1].ToLower() == "true")
            {
                MessageBox.Show(
                    @"Playability-check is finished. The engine has found a playable level.");
            }
            else
            {
                MessageBox.Show(
                    @"Playability-check is finished. The engine hasn't found a playable level.");
            }
            sr.Close();
        }
    }
}
