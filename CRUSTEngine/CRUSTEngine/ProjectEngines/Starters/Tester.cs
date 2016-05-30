using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PCGEngine.TestModule;
using Microsoft.Xna.Framework;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;
using Color = System.Drawing.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace CRUSTEngine.ProjectEngines.Starters
{
    class Tester
    {
        private static String pathNow = @"C:\CTREngine\Test.txt";

        public static void GenerateShotsGevaLevelsFile()
        {
            string path = pathNow;
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            int counter = -1;
            while((line = sr.ReadLine())!= null)
            {
                counter++;
                //if (counter > 43)
                {
                    line = line.Split('\t')[5];
                    string[] args = new string[2];
                    args[0] = "0";
                    args[1] = line;
                    GenManager.GenerateGevaLevel(args, false, true);
                }
            }
            sr.Close();
        }

        public static void GenerateShotsForPlayLevelsFile()
        {
            //for (int i = 3; i < 6; i++)
            //{
            string path = "100 Playability PPr.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            int counter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                //counter++;
                //if (counter < 264)
                //{
                //    continue;
                //}
                //else
                {
                    string[] args = new string[2];
                    args[0] = "0";
                    args[1] = line.Split('\t')[12];
                    GenManager.GenerateGevaLevel(args, false, true);
                }
            }
            sr.Close();
            //}
        }

        public static void GenerateColorMap()
        {
            List<Bitmap> imageList = new List<Bitmap>();
            String basicDir = @"D:\Projects\CRUST\CRUST V3.0 - Oct 2013\XNAInForms\XNAInForms\bin\x86\Debug\PolysTesting\";
            for (int i = 0; i < 100; i++)
            {
                String fileName = basicDir  + "poly"+ i.ToString() + @".jpg";
                if (File.Exists(fileName))
                    imageList.Add(new Bitmap(fileName));
            }
            int xStart = 0;
            int width =  imageList[0].Width;
            int height = imageList[0].Height;
            Bitmap finalImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(finalImage);
            g.Clear(Color.White);
            for (int x = xStart; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    int i = 0;
                    foreach (Bitmap image in imageList)
                    {
                        Color c = image.GetPixel(x, y);
                        {
                            red += c.R;
                            green += c.G;
                            blue += c.B;
                            i++;
                        }
                    }
                    if (red != 0 || green != 0 || blue != 0)
                    {
                        Color originalColor = Color.FromArgb(red / imageList.Count, green / imageList.Count, blue / imageList.Count);
                        finalImage.SetPixel(x, y, originalColor);
                    }
                }
            }
            finalImage.Save("ColorMapImageOutput.jpg");
        }
        
        public static void ExtractComponentToActionExperiment()
        {
            string path = "100 Playability PPr.txt";
            StreamReader sr = new StreamReader(path);
            StreamWriter sw = new StreamWriter("ComponentToActionExperiment.txt");
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                String levelStr = line.Split('\t')[12];
                String actionsStr = line.Split('\t')[13];

                LevelBuilder.CreateRestedLevel(levelStr, false);
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                ActionsExecuterGenSim.ListOfActions = new ActionsGenerator(actionsStr).Actions;
                ActionsExecuterGenSim.IsSimulatingGamePlayability = true;
                ActionsExecuterGenSim.nextActionIndex = 0;
                ActionsExecuterGenSim.isFinished = false;
                ActionsExecuterGenSim.IsSaveStateToFile = false;
                if (false)
                    EngineShotsManager.ShowXNAWindow();
                var gameTime = new GameTime();
                for (int i = 0; i < 1300; i++)
                {
                    ActionsExecuterGenSim.ManipulateActions(gameTime);
                    StaticData.EngineManager.Update(gameTime);
                }

                List<Component> items = new LevelGenerator(levelStr).Items;
                items.RemoveAt(0);
                items.RemoveAt(0);
                List<Action> actions = ActionsExecuterGenSim.ListOfActions;
                int nrOfNonVoids = actions.FindAll(a => !(a is VoidAction)).Count;
                actions = actions.FindAll(a => !(a is VoidAction)).ToList();

                RyseUsageManager usageManager = new RyseUsageManager(items, actions);
                StaticData.RyseComponentsUsageHelper = usageManager;
                usageManager.DoAnalysis();
                
                int nrOfBumps = items.FindAll(item => (item is Bump)).Count;
                int nrOfRockets = items.FindAll(item => (item is Rocket)).Count;
                int nrOfRopes = items.FindAll(item => (item is Rope)).Count;
                int nrOfBubbles = items.FindAll(item => (item is Bubble)).Count;
                int nrOfBlowers = items.FindAll(item => (item is Blower)).Count;

                int nrOfUsedComps = usageManager.UsedRopes
                                    + usageManager.UsedRocket
                                    + usageManager.UsedBlowers
                                    + usageManager.UsedBubbles
                                    + usageManager.UsedBumps;
                
                int nrONonfUsedComps = items.Count - nrOfUsedComps;
                double actionsToComponentsRatio = (nrOfUsedComps) / (double)items.Count;

                sw.WriteLine(items.Count
                    + "\t" + (nrOfNonVoids + usageManager.UsedBumps)
                    + "\t" + nrOfUsedComps
                    + "\t" + nrONonfUsedComps
                    + "\t" + actionsToComponentsRatio

                    + "\t" + usageManager.UsedRopes
                    + "\t" + usageManager.UsedRocket
                    + "\t" + usageManager.UsedBlowers
                    + "\t" + usageManager.UsedBubbles
                    + "\t" + usageManager.UsedBumps 

                    + "\t" + nrOfRopes
                    + "\t" + nrOfRockets
                    + "\t" + nrOfBlowers
                    + "\t" + nrOfBubbles
                    + "\t" + nrOfBumps

                    + "\t" + usageManager.UsedRopes / (double)nrOfRopes
                    + "\t" + usageManager.UsedRocket / (double)nrOfRockets
                    + "\t" + usageManager.UsedBlowers / (double)nrOfBlowers
                    + "\t" + usageManager.UsedBubbles / (double)nrOfBubbles
                    + "\t" + usageManager.UsedBumps / (double)nrOfBumps);
                sw.Flush();
            }
            sr.Close();
        }

        public static void GenerateLevelsPlayabilityWithRuleset1()
        {
            string path = "100 Playability PPr.txt";
            StreamReader sr = new StreamReader(path);
            StreamWriter sw = new StreamWriter("ComponentToActionExperiment.txt");
            string line = String.Empty;
            int counter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (counter < 21)
                {
                    string[] args = new string[2];
                    args[0] = "0";
                    args[1] = line.Split('\t')[12];
                    RYSEGenManager.GenerateGevaLevelEvolvePlayabilityFF(args, false, false, 0);
                }
                else
                {
                    break;
                }
                counter++;
            }
            sr.Close();
        }

        public static void WatchLivePlayabilityFromLevelsActionsFiles()
        {
            //for (int i = 3; i < 6; i++)
            //{
            string path = "160LevelSameCompsPlayable.txt";// "LevelsActions.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                LevelBuilder.CreateRestedLevel(line.Split('\t')[12], false);
                RyseAgent agent = new RyseAgent(10);
                agent.IsSaveImage = false;

                StaticData.GameSessionMode = SessionMode.PlayingMode;
                //EngineShotsManager.ShowXNAWindow();
                LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                simulator.SimulateNewWindow(new ActionsGenerator(line.Split('\t')[13]).Actions);
            }
            sr.Close();
            //}
        }

        public static void WatchLivePlayabilityGenSimLevels()
        {
            //for (int i = 3; i < 6; i++)
            //{
            string path = @"C:\CTREngine\GenSimLevelsPlayable.txt";// "LevelsActions.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                LevelBuilder.CreateRestedLevel(line.Split('\t')[5], false);

                
                //EngineShotsManager.ShowXNAWindow();
                ActionsExecuterGenSim.RyseFreq = false;
                LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                simulator.SimulateNewWindow(new ActionsGenerator(line.Split('\t')[7]).Actions);
            }
            sr.Close();
            //}
        }

        public static void WatchLivePlayabilityFromLevelsActionsFilesSameCompsForDesigner(int levelNr)
        {
            string path = @"C:\CTREngine\160LevelSameCompsPlayable.txt";// "LevelsActions.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            int counter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (counter == levelNr)
                {
                    DesignEnhanceManager.GevaLevel = line.Split('\t')[12];
                    DesignEnhanceManager.PlayabilityActions = line.Split('\t')[13];
                    LevelBuilder.CreateRestedLevel(DesignEnhanceManager.GevaLevel, false);
                    RyseAgent agent = new RyseAgent(10);
                    agent.IsSaveImage = false;
                    StaticData.SetEngineManagerLastLevel(ObjectSerializer.DeepCopy(StaticData.EngineManager));
                    break;
                }
                counter++;
            }
            sr.Close();
        }

        public static void WatchLivePlayabilityFromLevelsActionsFilesAllCompsForDesigner(int levelNr)
        {
            string path = @"C:\CTREngine\100 Playability PPr.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            int counter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (counter == levelNr)
                {
                    DesignEnhanceManager.GevaLevel = line.Split('\t')[12];
                    DesignEnhanceManager.PlayabilityActions = line.Split('\t')[13];
                    LevelBuilder.CreateRestedLevel(DesignEnhanceManager.GevaLevel, false);
                    RyseAgent agent = new RyseAgent(10);
                    agent.IsSaveImage = false;
                    StaticData.SetEngineManagerLastLevel(ObjectSerializer.DeepCopy(StaticData.EngineManager));
                    break;
                }
                counter++;
            }
            sr.Close();
        }


        public static int FilesCounter = 0;
        public static void GenerateStatesFilesPlayabilityFromLevelsActionsFiles()
        {
            //            string path = "160LevelSameCompsPlayable.txt";
            string path = "PhysicsEngine_EvolvePlayActions - BadRemoved.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                LevelBuilder.CreateRestedLevel(line.Split('\t')[12], false);

                StaticData.GameSessionMode = SessionMode.PlayingMode;
                ActionsExecuterGenSim.ListOfActions = new ActionsGenerator(line.Split('\t')[13]).Actions;
                ActionsExecuterGenSim.IsSimulatingGamePlayability = true;
                ActionsExecuterGenSim.nextActionIndex = 0;
                ActionsExecuterGenSim.isFinished = false;
                ActionsExecuterGenSim.IsSaveStateToFile = true;
                ActionsExecuterGenSim.IsSaveImage = true;
                // 1300: arbitrary choice above the 80 max actions
                var gameTime = new GameTime();
                for (int i = 0; i < 1300; i++)
                {
                    ActionsExecuterGenSim.ManipulateActions(gameTime);
                    StaticData.EngineManager.Update(gameTime);
                }
                FilesCounter++;
            }
            sr.Close();
        }

        public static void GenerateStatesFilesPlayabilityFromLevelsActionsFilesTestingTemp()
        {
//            string path = "160LevelSameCompsPlayable.txt";
            //string path = "PhysicsEngine_EvolvePlayActions - BadRemoved.txt";
            string path = "LevelsTestTemp.txt";
            StreamReader sr = new StreamReader(path);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                LevelBuilder.CreateRestedLevel(line.Split('\t')[13], false);
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                

                ActionsExecuterGenSim.ListOfActions = new ActionsGenerator(line.Split('\t')[14]).Actions;
                ActionsExecuterGenSim.ActionsNotifManager = new ActionsNotificationManager(ActionsExecuterGenSim.ListOfActions);
                ActionsExecuterGenSim.IsSimulatingGamePlayability = true;
                ActionsExecuterGenSim.nextActionIndex = 0;
                ActionsExecuterGenSim.isFinished = false;
                ActionsExecuterGenSim.IsSaveStateToFile = true;
                ActionsExecuterGenSim.IsSaveImage = true;

                //LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                //simulator.Simulate(new ActionsGenerator(line.Split('\t')[13]).Actions, true);

                // 1300: arbitrary choice above the 80 max actions
                var gameTime = new GameTime();
                for (int i = 0; i < 1300; i++)
                {
                    ActionsExecuterGenSim.ManipulateActions(gameTime);
                    StaticData.EngineManager.Update(gameTime);
                }



                FilesCounter++;
            }
            sr.Close();
        }

        public static void PrintCountComponents()
        {
            string path = pathNow;
            StreamWriter sw = new StreamWriter("CompsCount.txt");
            //for (int i = 1; i < 6; i++)
            //{
            //    string path = dir + i.ToString() + ".txt";
                StreamReader sr = new StreamReader(path);
               
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    //counter++;
                    //if (i == 3 && counter < 67)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    line = line.Split('\t')[3];
                    LevelGenerator gen = new LevelGenerator(line);
                    int rope = gen.Items.Where(item => item is Rope).Count();
                    int blowers = gen.Items.Where(item => item is Blower).Count();
                    int bubbles = gen.Items.Where(item => item is Bubble).Count();
                    int bumps = gen.Items.Where(item => item is Bump).Count();
                    int rockets = gen.Items.Where(item => item is Rocket).Count();
                    sw.WriteLine(rope + "\t" + bumps + "\t"  + blowers + "\t" + bubbles + "\t" + rockets);
                    sw.Flush();
                    //}
                }
                sr.Close();
            //}
            sw.Close();
        }

         public static void PrintAxialityComponents()
         {
             StreamWriter sw = new StreamWriter("AxiXy.txt");

             StreamReader sr = new StreamReader(pathNow);

                 string line = String.Empty;
                 while ((line = sr.ReadLine()) != null)
                 {
                     line = line.Split('\t')[2];
                     LevelGenerator gen = new LevelGenerator(line);
                     int maxX = 0;
                     int minX = 9999;
                     int maxY = 0;
                     int minY = 9999;
                     for (int j = 0; j < gen.Items.Count; j++)
                     {
                         Component curr = gen.Items[j];
                         int currX = GetX(curr);
                         int currY = GetY(curr);
                         if (currX > maxX)
                         {
                             maxX = currX;
                         }
                         if (currX < minX)
                         {
                             minX = currX;
                         }
                         if (currY > maxY)
                         {
                             maxY = currY;
                         }
                         if (currY < minY)
                         {
                             minY = currY;
                         }
                     }
                     double sqrt = Math.Sqrt(Math.Pow((maxX - minX), 2) + Math.Pow((maxY - minY), 2));
                     sw.WriteLine((maxX - minX) + "\t" + (maxY - minY) + "\t" + sqrt);
                     sw.Flush();
                     //}
                 }
                 sr.Close();
             sw.Close();
         }

        private static int GetX(Component curr)
        {
            return curr.X;
        }

        private static int GetY(Component curr)
        {
            return curr.Y;
        }

        public static void TestEntraAgent(String gevaLvl)
        {
            String[] args = new string[2];
            args[1] = gevaLvl;
            RunEntraAgent(args);
        }

        private static void RunEntraAgent(string[] args)
        {
            IsTestingEntra = true;
            GenManager.GenerateGevaLevel(args, StaticData.EntraImageInput);
            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);
            EntraDrawer.DrawIntoFileTesting(res.ReachableSpace);
        }

        public static void TestEntraAgent()
        {
            String[] args = GetTestArgs();
            RunEntraAgent(args);
        }

        private static string[] GetTestArgs()
        {
            string[] args = new string[2];
            args[1] =
                "cookie( 460 , 120 ) frog( 400 , 100 ) rope( 240 , 100 , 130 ) blower( 460 , 400 , 4 ) bump( 340 , 400 , 6 ) rocket( 220 , 460 , 7 ) bubble( 440 , 340 ) rope( 460 , 120 , 250 ) bump( 220 , 180 , 4 ) blower( 480 , 340 , 4 ) rocket( 380 , 380 , 1 ) blower( 240 , 60 , 0 )";

                //"cookie( 440 , 460 ) frog( 320 , 40 ) rope( 260 , 100 , 270 ) rope( 260 , 260 , 190 ) " +
                //"blower( 200 , 200 , 0 ) bump( 220 , 40 , 7 ) rocket( 320 , 40 , 1 ) bubble( 260 , 80 ) ";



                //"cookie(400, 98)  frog(435, 400)  rope(450, 40,90) " +
                //" rope(353, 56,90)  " +
                //" bump(470, 246, 0) " +
                //" bump(570, 246, 0) ";

                //"cookie(223, 89) frog(235, 400) rope(280, 46, 100) bump(150, 300, 0)"
                //+ "rope(400, 50, 150)"
                //+ "blower(280, 100, 0) bubble (310, 400)";


                //HARD - PATH
                //"cookie(223, 89)  frog(735, 250)  rope(293, 46, 100) rope(357, 77,90)"
                //+ " bump(670, 300, 0)"
                //+ "blower(450, 400, 0) "
                //+ "bubble (500, 400)";

                //"cookie(223, 89)  frog(735, 250)  rope(293, 46, 100) rope(357, 77,90)"
                //+ " bump(670, 300, 0)"
                ////+ "rocket(700, 400 ,6)"
                //+ "blower(450, 400, 0) "
                //+ "bubble (500, 400)";


                //"cookie(223, 89)  frog(735, 100)  rope(293, 46, 100) rope(357, 77,90)"
                //+ " bump(670, 300, 0)"
                //+ "rocket(700, 400 ,6)"
                //+ "blower(450, 400, 0) "
                //+ "bubble (500, 400)";



                //"cookie(223, 89)  frog(35, 100)  rope(293, 46, 100) rope(357, 77,90)"
                //+ "rocket(550, 100,4)"
                //+ "blower(450, 400, 0)"
                //+ "bubble (500, 400)"
                //+ "rocket(550, 400 ,6)";


                //"cookie(384, 86)  frog(573, 188)  rope(750, 40,90)  rope(828, 42,90) " +
                //" rocket(779, 331, 4)  bump(167, 431, 1)  ";

                //"cookie(457, 113)  frog(435, 400)  rope(450, 40,90)  rope(488, 65,90)  " +
                //"blower(498, 330, 0)  bubble(0, 562, 335)  bump(684, 88, 0) ";


                //"cookie(650, 88)  frog(435, 400)  rope(753, 43,90)  rope(557, 77,90)  ";

                //"cookie(393, 367)  frog(545, 134)  " +
                //"rope(442, 301,90)  rope(336, 343,90)  blower(650, 215, 4)  bubble(0, 581, 445)  ";

                // GOOD
                //"cookie(393, 367)  frog(600, 134)"
                ////+ "bump(500, 200, 3)"
                //+ "rope(336, 230, 90) rope(450, 250 ,190)"
                //+ "blower(630, 414, 4)"
                //+ "bump(490, 450, 0)  "
                //+ "bubble (550, 414)";

                //"cookie(393, 367)  frog(345, 134) rope(336, 343,90) rope(450, 301,190)" +
                //"blower(616, 414, 4)" +
                //"bubble(531, 445)" +
                //"bump(500, 200, 1)  ";
                //"bump(521, 350, 0)  bump(451, 466, 0)  bump(451, 356, 0)  ";


                //"cookie(390, 103)  frog(419, 400)  rope(450, 40,90)  rope(326, 83,90)" +
                //" bump(269, 209, 0)" +
                //" bump(229, 345, 0)  " +
                //" bump(320, 118, 0) " +
                //" bump(364, 226, 0) " +
                //" bump(458, 223, 0) " +
                //" bump(602, 253, 0)  ";


                //    "cookie(400, 100)  frog(435, 400)  rope(450, 40,90)  rope(350, 62,90) " +
                //    "rocket(630, 347, 4)  "
                //    //"bump(296, 252, 0)" 
                //    + "bump(500, 205, 0)  "
                //    + "bump(590, 258, 0)  "
                //    + "bump(700, 420, 0) ";
                ////+ "bump(144, 211, 0)  " 

                //+"bump(477, 139, 0)  ";


                //HARD
                //    "cookie(424, 117)  frog(750, 402)  rope(300, 40, 150) rope(429, 40,90) "
                //+ "rocket(450, 150, 0)"
                //+ "bump(100, 324, 1)"
                //+ "bump(700, 250, 0)"
                //+ "bump(770, 250, 3)"
                //+ "rocket(550, 450, 7)";

                //"cookie(424, 117)  frog(750, 402)  rope(300, 40, 150) rope(429, 40,90) "
                //+ "rocket(450, 150, 0)"
                //+ "bump(700, 250, 0)"
                //+ "bump(770, 250, 0)";



                //"cookie(424, 117)  frog(750, 402)"
                //+ "rope(400, 40, 150) "
                //+ "rope(429, 40,90) "
                //+ "rocket(430, 250, 0)"
                //+ "bump(570, 350, 3)";


                //"cookie(424, 117)  frog(750, 402)  rope(300, 40, 150) rope(429, 40,90) "
                //+ "rocket(450, 150, 0)"
                //+ "bump(600, 260, 3)"
                //+ "bump(600, 350, 3)";

                //"cookie(424, 117)  frog(150, 402)  rope(629, 40,90)" +
                //"rocket(650, 300, 5)" +
                //"bump(440, 260, 1)" +
                //"bump(380, 420, 1) bump(470, 420, 1) "
                //+ "bump(400, 220, 1) "
                //+ "bump(400, 370, 1) ";


                //"cookie(424, 117)  frog(750, 402)  rope(429, 40,90) rope(500, 40, 90) "
                //+ "blower(357, 200, 0) "
                //+ "bubble(0, 412, 200) "
                //+ "bump(630, 430, 3)"
                //+ "bump(500, 440, 0) ";

                //"cookie(424, 117)  frog(750, 402)  rope(429, 40,90)  "
                //+ "rocket(529, 306, 4)"
                //+ "rocket(546, 420, 0)  "
                //+ "blower(357, 200, 0) "
                //+ "bubble(0, 412, 200) "
                //+ "bump(450, 440, 3) "
                //+ "bump(470, 300, 3)"
                //+ "bump(540, 260, 3) bump(600, 260, 3) "
                //+ "bump(670, 260, 3) bump(720, 260, 3) bump(780, 260, 3) "
                //+ "bump(630, 430, 1)";

                //"cookie(424, 117)  frog(750, 402)  rope(429, 40,90)  "
                //+ "rocket(546, 420, 0)  "
                //+ "blower(357, 200, 0) "
                //+ "bubble(0, 412, 200) "
                //+ "bump(450, 440, 3) ";
                //+ "bump(470, 300, 3)"
                //+ "bump(540, 260, 3) ";
                //+ "bump(600, 260, 3) "
                //+ "bump(670, 260, 3) bump(720, 260, 3) bump(780, 260, 3) "


                //"cookie(220,100)frog(300,450)rope(300,100,130)rope(480,100,130)" +
                //"Bump(100, 300, 0) Bump(170, 300, 0) Bump(230, 300, 0) " +
                //"Bump(300, 300, 0)" +
                //"rocket(550, 180, 3)";

                //" cookie(497, 130)  frog(668, 300)  rope(454, 99,90)  rope(383, 113,90)  rope(631, 101,90)" +
                //" blower(270, 367, 4)" +
                //" rocket(431, 350, 0)   " +
                //" bump(270, 259, 0) bump(134, 184, 0)  bump(138, 343, 1)  "
                //+ " bump(520, 140, 0)"
                //+ " bump(570, 200, 0) "
                //+ " bump(672, 180, 0)"
                //+ " bump(750, 420 , 3)"; 

                //"cookie(220,100)frog(300,450)rope(300,100,130)rope(480,100,130)" +
                //"Bump(100, 300, 0)" +
                //"Bump(300, 300, 0)";

                //"cookie(163, 135)  frog(789, 190)  " +
                ////"rope(131, 66,105) " +
                //"rope(190, 69,105)  rope(251, 66,170)" +
                //"blower(400, 400, 0)  bubble(0, 450, 450)  " +
                //"rocket(200, 350, 4)" +
                //"rocket(613, 27, 1) " +
                //"bump(100, 300, 1)  bump(307, 300, 3)  ";

                //"cookie(197, 171)  frog(668, 50)  rope(190, 69,135)  rope(251, 66,195)  blower(448, 190, 0)  bubble(0, 505, 222)  rocket(263, 416, 0)  rocket(799, 427, 5)  bump(162, 350, 1)  bump(304, 255, 3)  ";
                //"cookie( 300 , 460 ) frog( 480 , 460 ) rope( 320 , 220 , 100 ) rope( 200 , 440 , 220 ) blower( 520 , 280 , 0 ) bump( 340 , 380 , 2 ) rocket( 420 , 280 , 0 ) bubble( 400 , 120 ) rocket( 480 , 200 , 0 ) blower( 420 , 280 , 0 ) ";

                //PROPLEM STACK
                //"cookie(360,40)frog(440,400)rope(220,400,130)blower(460,140,4)bump(220,280,7)rocket(380,220,1)bubble(280,160)bump(220,280,4)rocket(340,260,4)rope(240,140,130)blower(540,240,4)";

                //"cookie( 540 , 260 ) frog( 220 , 60 ) rope( 340 , 280 , 190 ) blower( 420 , 220 , 0 ) bump( 340 , 440 , 4 ) rocket( 380 , 320 , 5 ) bubble( 300 , 80 ) bubble( 520 , 380 ) rocket( 380 , 80 , 3 ) blower( 300 , 80 , 0 )";
                //"cookie( 540 , 260 ) frog( 220 , 60 ) rope( 100 , 150 , 190 ) rope (120, 200, 100)";

                //"cookie(440, 99)  frog(724, 138)  rope(400, 43,150)  rope(500, 54,90)    bump(300, 286, 0)  "
                //+ "bump(190, 335, 0)";

                //"cookie(418, 104)  frog(647, 43)  rope(450, 40,90)  rope(378, 85,90) bubble(0, 482, 214)  ";
                //+"blower(417, 207, 0) ";

                //"cookie(412, 109)  frog(40, 200)  rope(450, 40,90)  rope(366, 76,90)  rocket(410, 396, 4)  bump(235, 284, 0)  bump(281, 324, 0)  rocket(235, 396, 5) ";

                //"cookie(512, 134)  frog(819, 66)  rope(481, 43,120)  rope(547, 77,90)  blower(460, 262, 0)  bubble(0, 518, 339)  bump(743, 123, 3)  ";

            //"cookie( 280 , 220 ) frog( 540 , 360 ) rope( 420 , 320 , 220 ) blower( 420 , 100 , 4 ) bump( 480 , 340 , 1 ) rocket( 420 , 460 , 4 ) bubble( 260 , 460 ) bubble( 300 , 100 ) rocket( 280 , 360 , 7 ) ";

            // DESIGNER-TO PAPER
            //"cookie(245, 151)  frog(746, 50)  rope(283, 24,150)  rope(227, 93,90)  blower(460, 214, 0)  bubble(0, 520, 227)  rocket(227, 427, 0)  rocket(769, 426, 5)  bump(276, 265, 3)  bump(140, 376, 1) ";
            return args;
        }

        public static bool IsTestingEntra = false;

        public static void TestEntraPathGeneration(String[] args)
        {
            //MessageBox.Show(args[1]);
            //args = new string[2];
            //args[1] =
            //    "cookie( 320 , 420 ) frog( 540 , 440 ) rope( 320 , 280 , 190 ) rope( 540 , 280 , 160 ) blower( 460 , 360 , 0 ) bump( 440 , 40 , 4 ) rocket( 200 , 60 , 0 ) bubble( 200 , 380 ) blower( 400 , 240 , 0 ) ";

            List<Point> designerPath = new List<Point>()
                {
                    new Point(220, 60),
                    new Point(460, 60),
                    //new Point(460, 400),
                    new Point(220, 400)
                };
                //{
                //    new Point(200, 40),
                //    new Point(460, 40),
                //    new Point(300, 200),
                //    new Point(500, 400)
                //};
            EntraPathGenManager.SetFitnessValueForLevel(args, designerPath);
        }

        public static void EntraPathResult(String gevaLvl)
        {
            String[] args = new string[2];
            args[1] = gevaLvl;
            EntraPathGenManager.ShowTestResult(args);
        }

        public static void GeneratePolysShotsGevaLevelsFile()
        {
            StreamReader sr = new StreamReader(pathNow);
            string line = String.Empty;
            int counter = -1;
            while ((line = sr.ReadLine()) != null)
            {
                counter++;
                //if (counter > 38)
                {
                    line = line.Split('\t')[2];
                    string[] args = new string[2];
                    args[0] = "0";
                    args[1] = line;
                    //String pathPoly = counter.ToString() + "_levelPoly.jpg";
                    EntraGenManager.PrintEffetivceSpace(args, true, counter);
                }
            }
            sr.Close();
        }

        public static void PrintDensityFile()
        {
            String resPath = "density_values.txt";
            StreamWriter sw = new StreamWriter(resPath);

            List<String> phenos = GetPhenos(pathNow);
            
            List<double> res = DensityManager.GetDensityArrOfLevelFile(phenos);
            foreach (double val in res)
            {
                sw.WriteLine(val);
            }
            sw.Close();
        }

        public static void GetDiversityImage()
        {
            List<String> phenos = GetPhenos(pathNow);
            DiversityManager.GetDiversityImage(phenos);
        }

        private static List<string> GetPhenos(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<String> phenos = new List<string>();
            String pheno;
            while ((pheno = sr.ReadLine()) != null)
            {
                pheno = pheno.Split('\t')[7];
                phenos.Add(pheno);
            }
            return phenos;
        }

        public static void TestEntraPlusFromFile()
        {
            String path = @"C:\CTREngine\AllEntraPlusEvolvedLevels.txt";
            List<string> phenos = GetPhenos(path);
            int counter = 1;

            foreach (string pheno in phenos)
            {
                String[] args = new string[3];
                args[1] = pheno;
                args[2] = "1";
                EntraPlusGenManager.SetFitnessValueForLevelTest(args, counter);
                counter++;
            }
        }

        //public static void TestGenSimAgent()
        //{
        //    var catPairs = new List<ActionTimePair>();
        //    //catPairs.Add(new CompTimePair(ComponentType.Rope, 5 * 60));
        //    //catPairs.Add(new CompTimePair(ComponentType.Rope, 7 * 60));
        //    ////CATPairs.Add(new CompTimePair(ComponentType.Rope, 7 * 60));
        //    ////CATPairs.Add(new CompTimePair(ComponentType.Rope, 8 * 60));
        //    ////CATPairs.Add(new CompTimePair(ComponentType.Bump, 8*60));
        //    ////CATPairs.Add(new CompTimePair(ComponentType.Bubble, 10 * 60 + 30));
        //    //catPairs.Add(new CompTimePair(ComponentType.Rocket, 8 * 60));
        //    //catPairs.Add(new CompTimePair(ComponentType.Rocket, 10 * 60));
        //    //catPairs.Add(new CompTimePair(ComponentType.Rocket, 12 * 60));
        //    //catPairs.Add(new CompTimePair(ComponentType.Frog, 14 * 60 + 30));

        //    GenSimAgentWrapper agent = new GenSimAgent(catPairs);
        //    agent.ScatterComps();

        //    if (agent.Actions.Count > 0)
        //    {
        //        LevelBuilder.CreateRestedLevel(agent.LevelStr, false);
        //        //var c = agent.Actions[15];
        //        StaticData.GameSessionMode = SessionMode.PlayingMode;
        //        LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
        //        simulator.SimulateNewWindow(agent.Actions);
        //    }
        //    //EngineShotsManager shots = new EngineShotsManager();
        //    //shots.TakeEngineShot(StaticData.EntraImageInput);
        //}

        public static void ClipImageSet()
        {
            List<Bitmap> imageList = new List<Bitmap>();
            string num = "4";
            String basicDir = num + @"\";
            //String outDir = @"ClippedImages\";
            Directory.CreateDirectory(num + "_ClippedImages");
            string[] allfiles = Directory.GetFiles(basicDir);
            for (int i = 0; i < allfiles.Length; i++)
            {
                if (File.Exists(allfiles[i]))
                    imageList.Add(new Bitmap(allfiles[i]));
            }
            //for (int i = 0; i < 1000; i++)
            //{
            //    String fileName = basicDir + i.ToString() + @".jpg";
            //    if (File.Exists(fileName))
            //        imageList.Add(new Bitmap(fileName));
            //}
            int xStart = 180;
            int xEnd = 570;
            int widthNew =  (xEnd - xStart);
            int heightNew = imageList[0].Height;
            int outCounter = 0;
            foreach (Bitmap bitmap in imageList)
            {
                Bitmap newImage = new Bitmap(widthNew, heightNew);
                Graphics g = Graphics.FromImage(newImage);
                g.Clear(Color.White);
                int xNew = 0;
                int yNew = 0;
                for (int x = xStart; x < xEnd; x++)
                {
                    yNew = 0;
                    for (int y = 0; y < heightNew; y++)
                    {
                        newImage.SetPixel(xNew, yNew, bitmap.GetPixel(x, y));
                        yNew++;
                    }
                    xNew++;
                }
                newImage.Save(num + @"_ClippedImages\"  + outCounter + @".jpg");
                outCounter++;
            }
        }
    }
}