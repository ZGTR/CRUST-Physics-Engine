using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ClipperLib;
using CRUSTEngine.ProjectEngines.Starters;
using Microsoft.Xna.Framework;
using CRUSTEngine.Database;
using CRUSTEngine.FormsManipualtion;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.MusicBased;
using Point = Microsoft.Xna.Framework.Point;

namespace CRUSTEngine.ProjectEngines.Starters
{
    internal class StarterManager
    {
        public void Start(string[] args)
        {
            DateTime d1 = DateTime.Now;
            CallMethod(args);
            DateTime d2 = DateTime.Now;
            StreamWriter sw = new StreamWriter("timeTaken.txt", true);
            sw.WriteLine((d2 - d1).TotalSeconds.ToString(CultureInfo.InvariantCulture));
            sw.Flush();
            sw.Close();
        }

        private void CallMethod(string[] args)
        {
            int execCode = Int32.Parse(args[0]);
            switch (execCode)
            {
                case 99:
                    ExecDesigner();
                    break;
                case 90:
                    ExecPhysicsEngine();
                    break;

                default:
                    ExecDesigner();
                    break;
            }
        }

        public void ExecPhysicsEngine()
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
            StaticData.GameSessionMode = SessionMode.DesignMode;
            using (Game1 game1 = new Game1())
            {
                StaticData.EngineManager = new EngineManager(game1);
                StaticData.InitializeEngine(game1);
                Game1.IsUserDesigner = true;
                game1.Run();
            }
        }

        public void ExecDesigner()
        {
            DesignerManager designerManager = new DesignerManager();
            designerManager.Run();
        }

        public static void TestMe(string[] args)
        {
            // ANALYSIS
            //Tester.ClipImageSet();
            //Tester.GenerateShotsGevaLevelsFile();
            //Tester.GeneratePolysShotsGevaLevelsFile();
            //Tester.GenerateColorMap();
            //Tester.GetDensityArrOfLevelFile();
            //Tester.PrintAxialityComponents();
            //Tester.PrintCountComponents();
            //GenManager.GenerateGevaLevel(args, StaticData.EntraImageInput);

            //TestGenBySim(args);

            //Tester.WatchLivePlayabilityGenSimLevels();

            if (true)
            {
                args = new string[3];
                args[0] = "0";
                args[1] = "cookie(420,440)frog(380,380)rope(520,200,160)blower(440,320,0)rope(340,320,160)";
                args[2] = "1";
            }
            //EntraPlusGenManager.SetFitnessValueForLevel(args);
            //EntraGenManager.SetFitnessValueForLevel(args);
            //EntraPathGenManager.SetFitnessValueForLevel(args, new List<Point>());

            //Tester.TestEntraPathGeneration(args);
            //EntraPathGenManager.ShowTestResult(args);

            //RYSEGeneratorManager.GenerateGevaLevel(args, false, false);
            //RYSEGeneratorManager.GenerateLevelTesting();
            //RYSEGenManager.GenerateGevaLevelRandPlayabilityFF(args, false, false, 0);

            // TEST
            //Tester.TestGenSimAgent();
            //Tester.TestEntraPlusFromFile();
            String levelStr =
                //"cookie(540,260)frog(300,40)rope(380,80,100)rope(480,460,190)blower(500,180,4)bump(220,460,0)rocket(380,80,5)bubble(480,460)";
                //"cookie(320,360)frog(400,140)rope(260,140,250)rope(480,220,100)blower(460,160,4)bump(280,300,6)rocket(260,360,1)bubble(240,400)";
                //"cookie(460,240)frog(300,380)rope(460,140,190)blower(400,100,4)bump(280,60,1)rocket(500,460,3)bubble(460,260)bump(300,440,2)";
                //"cookie(348, 115)  frog(429, 307)  rope(380, 55,90)  rope(326, 57,90)  rocket(371, 419, 0)  bump(213, 318, 0)  bump(262, 274, 0) ";
                //"cookie( 240 , 460 ) frog( 380 , 240 ) rope( 220 , 60 , 100 ) rope( 280 , 100 , 160 ) blower( 460 , 420 , 4 ) bump( 200 , 360 , 0 ) rocket( 460 , 160 , 7 ) bubble( 200 , 100 ) bubble( 480 , 280 ) blower( 520 , 260 , 4 ) bump( 500 , 400 , 2 ) bump( 280 , 140 , 7 )";
                //"cookie( 240 , 460 ) frog( 380 , 240 ) rope( 220 , 60 , 100 ) rope( 280 , 100 , 160 ) blower( 460 , 420 , 4 ) bump( 200 , 360 , 0 ) rocket( 460 , 160 , 7) blower( 520 , 260 , 4 ) bump( 500 , 400 , 2 ) bump( 280 , 140 , 7 )";
                //"cookie(245, 151)  frog(746, 50)  rope(283, 24,150)  rope(227, 93,90)  blower(460, 214, 0)  bubble(0, 520, 227)  rocket(227, 427, 0)  rocket(769, 426, 5) bump(276, 265, 3)  bump(140, 376, 1)  ";
                //"cookie(245, 151)  frog(746, 50)  rope(283, 24,150)  rope(227, 93,90)  blower(460, 214, 0)  bubble(0, 520, 227)  rocket(227, 427, 0)  rocket(769, 426, 5)  bump(550, 350, 1)";
                //"cookie(441, 119)  frog(767, 344)  rope(450, 40,90)  rocket(546, 331, 4)";
                //"cookie(444, 117)  frog(835, 397)  rope(450, 40,90)  blower(504, 386, 0)  rocket(435, 310, 0)  ";
                //"cookie(445, 117)  frog(698, 203)  rope(450, 40,90)  blower(446, 331, 0)  bubble(0, 508, 375)  rocket(386, 470, 7)";
                //"cookie(371, 91)  frog(729, 377)  rope(450, 40,90)  rope(296, 68,90)  bump(552, 285, 0)";
                //"cookie(444, 117)  frog(529, 454)  rope(450, 40,90)  bump(460, 246, 0)  bump(580, 325, 3) ";
                //"cookie(445, 117)  frog(455, 400)  rope(450, 40,90)  blower(453, 333, 0)   blower(438, 266, 0) ";
                //"cookie(421, 76)  frog(435, 400)  rope(363, 44,90)  rope(486, 26,90)  bump(380, 124, 0)  ";
                //"cookie(266, 72)  frog(384, 240)  rope(267, 23,90)  bump(200, 360, 0) " + "bump(280, 140, 7)  ";
                //"cookie( 240 , 460 ) frog( 380 , 240 ) rope( 250 , 60 , 100 ) rope( 280 , 100 , 160 ) bump( 200 , 360 , 0 ) bubble( 200 , 100 )";
                //"cookie(300, 61)  frog(417, 431)  rope(450, 40,90)  rope(162, 35,90)  bump(468, 214, 0) bump(561, 189, 0)  bump(395, 292, 0) ";
                //"cookie(300, 61)  frog(417, 431)  rope(450, 40,90)  rope(162, 35,90)  bump(520, 244, 0) bump(590, 189, 0)";
                //"cookie(300, 61)  frog(417, 431)  rope(450, 40,90)  rope(362, 35,90)  bump(520, 244, 0) bump(590, 189, 0)";
                //"cookie(371, 74)  frog(143, 394)  rope(150, 40,90) rope(350, 40,90) blower(769, 276, 4) ";
                //" rocket(469, 276, 0)  rocket(579, 263, 7)  bump(43, 213, 1)  bump(197, 230, 0)  ";
                "cookie(512, 134)  frog(819, 66)  rope(481, 43,120)  rope(547, 77,90) bubble(0, 518, 339) bump(743, 123, 1) blower(460, 262, 0)";
            //"cookie(245, 151)  frog(746, 50)  rope(283, 24,150)  rope(227, 93,90)  blower(460, 214, 0)  bubble(0, 520, 227)  rocket(227, 427, 0)  rocket(769, 426, 5) bump(140, 376, 1) bump(276, 265, 3) ";
            //Tester.TestEntraAgent(levelStr);
            //Tester.EntraPathResult(levelStr);
            //Tester.EntraAgentPlusResult(levelStr);

            //Tester.GetDiversityImage();
            //RYSEGeneratorManager.GenerateGevaLevelEvolvePlayabilityFFForCSharp(false, false, 10);
            //RYSEGeneratorManager.GenerateGevaLevelEvolvePlayabilityFF(args, false, false, 10);
            //RYSEGeneratorManager.GenerateGevaLevelRandPlayabilityFF(args, false, false, 10);
            //Tester.GenerateShotsForPlayLevelsFile();
            //Tester.GenerateLevelsPlayabilityWithRuleset1();
            //Tester.ExtractComponentToActionExperiment();
            //Tester.GenerateStatesFilesPlayabilityFromLevelsActionsFilesTestingTemp();
            //Tester.WatchLivePlayabilityFromLevelsActionsFiles();

            //Tester.GenerateStatesFilesPlayabilityFromLevelsActionsFiles();
            //FileToDbHandler.FileToDB(@"TestPlay\ResultsTests\Results 80 Pattern1.txt", 1);
        }

        private static void ExecTestGenBySim(string[] args)
        {

            //// GENERATION - JAVA
            if (false)
            {
                args = new string[3];
                args[1] =
                    //"frog(400, 400) cookie(100, 100) rope(200, 200, 150) rope(340, 200, 200) bubble(250, 340) blower(200, 220, 0)";
                    //"blower_press( 1600 ) rope_cut( 500 ) rocket_press( 2000 ) omnom_feed(1000)";
                    //"rope_cut( 1600 ) rope_cut( 1400 ) bubble_press( 3000 ) rocket_press( 2100 )  omnom_feed(0) ";
                    //"blower_press( 2000 ) rope_cut( 800 ) rope_cut( 600 ) bubble_press( 2000 ) blower_press( 400 ) bumper_interaction( 1500 ) omnom_feed(0) ";
                    //"blower_press( 1600 ) rope_cut( 400 ) rocket_press( 700 ) bumper_interaction( 1500 ) blower_press( 200 ) omnom_feed(0) ";
                    //"rope_cut( 1000 )  omnom_feed(0) ";
                    //"blower(100, 0 ) blower_press( 1000 ) rope( 440 , 240 , 250 ) rope_cut( 1000 ) omnom_feed(0) ";
                    //"rope(380,140,160)rope_cut(1400)omnom_feed(0)";
                    //"rope( 540 , 40 , 130 ) rope_cut( 1200 ) omnom_feed(0)";
                    //"rope(240,220,130)rope_cut(800)rope(220,240,250)rope_cut(1200)rope(400,220,190)rope_cut(1800)omnom_feed(0)";
                    //"blower(100,4)blower_press(500)rope(400,240,160)rope_cut(200)rocket(1500,3)rocket_press(1800)omnom_feed(0)";
                    //"blower(100, 0 ) blower_press( 2000 ) rope( 400 , 160 , 130 ) rope_cut( 200 ) rope( 280 , 60 , 250 ) rope_cut( 400 ) rocket( 1000 , 1 ) rocket_press( 1000 ) omnom_feed(0) ";
                    //"blower(100,4)blower_press(1000)rope(320,240,100)rope_cut(400)rocket(1000,2)rocket_press(1600)blower(100,4)blower_press(1000)omnom_feed(0)";
                    //"blower(100, 0 ) blower_press( 1000 ) rope( 520 , 60 , 220 ) rope_cut( 200 ) rope( 220 , 220 , 130 ) rope_cut( 600 ) blower(100, 4 ) blower_press( 500 ) bumper(100, 1 ) bumper_interaction( 2000 ) omnom_feed(0) ";
                    //"rope( 260 , 100 , 100 ) rope_cut( 200 ) bumper(100, 1 ) bumper_interaction( 1000 ) bumper(100, 3 ) bumper_interaction( 1000 ) blower(100, 0 ) blower_press( 500 ) rocket( 500 , 2 ) rocket_press( 1200 ) omnom_feed(0) ";
                    //"blower(100, 0 ) blower_press( 2000 ) rope(100, 440 , 80 , 190 ) rope_cut( 200 ) rope(100, 200 , 60 , 160 ) rope_cut( 1600 ) rope( 100, 280 , 220 , 130 ) rope_cut( 400 ) rocket( 1500 , 5 ) rocket_press( 200 ) omnom_feed(0) ";
                    //"blower(100, 0 ) blower_press( 500 ) rope(100, 220 , 120 , 220 ) rope_cut( 600 ) rope(100, 300 , 200 , 220 ) rope_cut( 400 ) blower(100, 0 ) blower_press( 500 ) rocket( 500 , 6 ) rocket_press( 1600 ) omnom_feed(0) ";
                    //"blower(100, 4 ) blower_press( 2000 ) rope(100, 320 , 200 , 100 ) rope_cut( 200 ) rope(100, 520 , 140 , 220 ) rope_cut( 600 ) blower(100, 0 ) blower_press( 500 ) bumper(100, 0 ) bumper_interaction( 2000 ) omnom_feed(0) ";
                    //"blower(100,0)blower_press(500)blower(100,4)blower_press(500)rope(100,340,140,130)rope_cut(200)rocket(500,7)rocket_press(1400)bubble(1800)bubble_press(1000)omnom_feed(0)";
                    //"rope(100, 280 , 80 , 220 ) rope_cut( 600 ) rope(100, 540 , 220 , 220 ) rope_cut( 400 ) rope(100, 500 , 120 , 100 ) rope_cut( 200 ) rope(100, 360 , 60 , 130 ) rope_cut( 400 ) blower(100, 0 ) blower_press( 1500 ) bumper(100, 3 ) bumper_interaction( 1000 ) bubble( 600 ) bubble_press( 2000 ) omnom_feed(0) ";
                    //"rope(100,220,120,130)rope_cut(600)rope(100,260,240,100)rope_cut(400)rope(100,540,220,160)rope_cut(600)bumper(100,0)bumper_interaction(1000)blower(100,4)blower_press(500)bubble(1600)bubble_press(2000)omnom_feed(0)";
                    //"rope(100,220,120,130)rope_cut(600)omnom_feed(0)";  
                    //"rope(100,220,120,130)rope_cut(600)bumper(100,0)bumper_interaction(1000)blower(100,4)blower_press(500)bubble(1600)bubble_press(2000)omnom_feed(0)";
                    //"rope(100, 220 , 180 , 220 ) rope_cut( 200 ) rope(100, 520 , 80 , 100 ) rope_cut( 200 ) bubble( 600 ) bubble_press( 500 ) rocket( 2000 , 3 ) rocket_press( 1200 ) bubble( 600 ) bubble_press( 1500 ) omnom_feed(0) ";
                    //"blower(100,0)blower_press(1500)rope(100,240,100,160)rope_cut(200)bumper(100,3)bumper_interaction(700)bubble(400)bubble_press(500)rocket(500,2)rocket_press(1800)omnom_feed(0)";
                    //"rope(100,480,220,190)rope_cut(600)rope(100,280,140,250)rope_cut(200)rope(100,320,120,130)rope_cut(200)rocket(2000,4)rocket_press(1200)bumper(100,3)bumper_interaction(2000)omnom_feed(0)";
                    //"blower(100, 0 ) blower_press( 500 ) rope(100, 520 , 40 , 160 ) rope_cut( 1000 ) rope(100, 420 , 80 , 100 ) rope_cut( 800 ) rope(100, 500 , 120 , 220 ) rope_cut( 400 ) rope(100, 340 , 100 , 100 ) rope_cut( 400 ) blower(100, 0 ) blower_press( 1000 ) rocket( 500 , 4 ) rocket_press( 800 ) rocket( 500 , 0 ) rocket_press( 200 ) omnom_feed(0) ";
                    "rope_cut(200)rope_cut(500)blower_press(1500)rocket_press(1500)omnom_feed(0)";

                //string str = 
                //                                            @"
                //                                            rope_cut(914)
                //                    
                //                                            blower_press(354)
                //                                            blower_press(352)
                //                                            rope_cut(1131)
                //                    
                //                                            bubble_press(359)
                //                                            bumper_interaction(348)
                //                                            bumper_interaction(1157)
                //                    
                //                                            bumper_interaction(346)
                //                                            bumper_interaction(340)
                //                                            omnom_feed(0)
                //                                            ";

                //                @"
                //                        rope_cut(914)
                //                        rocket_press(1500)
                //                        rocket_press(500)
                //                        omnom_feed(0)
                //                        ";

                //var s = MPCGHelper.ConvertFileToTTN(); 
                args[2] = "1";
                //for (int i = 0; i < 10; i++)
                {
                    GenSimManager.TestGevaLevelCAAll(args, false, false);
                }
                //GenSimManager.TestGevaLevelCAAll(args, false, false);

                //GenSimManager.TestGevaLevelCAAll(args, false, false);
                //GenSimManager.GenerateGevaLevel(args, true, false, false); 
            }
            else
            {
                //GenSimManager.GenerateGevaLevel(args, false, true, true);    
            }
        }
    }
}

