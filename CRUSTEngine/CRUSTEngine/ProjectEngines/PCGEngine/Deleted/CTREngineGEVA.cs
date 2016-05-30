//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
//using CRUSTEngine.ProjectEngines.HelperModules;
//using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
//using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
//using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;


//namespace CRUSTEngine.ProjectEngines.PCGEngine
//{
//    class CTREngineGEVA
//    {
//        private String _engineFileString =
//            @"Z:\ZGTR Physics Engine - PCG 13\SourceCode\CRUSTEngine_withEclipse\XNAInForms\XNAInForms\bin\x86\Debug\engineStateRested";
//        private String _levelGenFileString =
//            @"Z:\ZGTR Physics Engine - PCG 13\SourceCode\CRUSTEngine_withEclipse\XNAInForms\XNAInForms\bin\x86\Debug\levelGenBytes";
        
//        public LevelGenerator LevelGeneratorEngine;
//        public ActionsGenerator ActionsGeneratorEngine;
//        public string[] Args;

//        public static bool IsZGTRPlaying = true;
//        public static bool IsDesign = true;
//        public static bool IsTestHere = true;
//        private bool _isTotalNew = false;
//        public bool IsSaveImagePlayability = false;
//        public PlayabilityEngineSimulatorProlog Simulator;
//        public EngineShotsManager ShotsManager;

//        private String _strLevel = //"";
//            //"rocket ( 100, 100, 0 )  rocket ( 300, 100, 0 ) Bump (200, 100, 0) Bump (200, 300, 1) Bump (200, 400, 2) cookie( 100 , 10 ) Bubble (100, 100) frog( 400 , 400 )";
//            //"catchable_rope(400, 400, 50) rocket ( 600 , 100 , 0 ) blower(70, 250, 0) cookie( 60 , 60 ) frog( 400 , 400 )";
//            "cookie( 30 , 30 ) bump(400 ,110, 1) catchable_rope(200, 10, 100) frog( 400 , 400 )";
//            //" cookie( 0 , 0 ) frog( 400 , 500 )";
//            //"cookie( 100 , 120 ) frog( 110 , 210 )  rope( 20 , 100 , 40 ) rope( 220 , 40 , 30 ) blower(250, 100)";
//            //" water( 100 , 1 ) cookie( 100 , 120 ) bubble (200, 200) frog( 210 , 210 )  rope( 100 , 270 , 60 ) blower( 280 , 60 , 3 ) blower( 260 , 140 , 3 )";
//            //" cookie( 100 , 120 ) frog( 110 , 210 )  ";
//        //"rope( 120 , 40 , 40 ) rope(10 , 40 , 40 )"
//        //"water( 1 , 100 )"
//        private String _strAction =
//            "";

        
//        //"blower_press rope_cut( 0 ) void_action blower_press blower_press blower_press blower_press blower_press blower_press rope_cut( 1 ) ";
        
//        public CTREngineGEVA(String[] args, bool isSaveImagePlay)
//        {
//            this.IsSaveImagePlayability = isSaveImagePlay;
//            this.Args = args;
//            this.Simulator = new PlayabilityEngineSimulatorProlog(this.IsSaveImagePlayability);
//            this.ShotsManager = new EngineShotsManager();
//            System.Console.WriteLine(EngineStateManager.GetEngineStateFactStringWithEnterDelimiter(false));
//            InitilaizeEngine();
//            //EngineShotsManager.ShowXNAWindow();
//            //ShotsManager.TakeEngineShot();
//        }

//        private void InitilaizeEngine()
//        {
//            if (IsTestHere)
//            {
//                this.Args = new string[3];
//                Args[0] = "0";
//                Args[1] = "0";
//                Args[2] = "0";
//            }

//            if (Args[0] == "0")     // 0: If we are creating the level for the first time, save it into a file
//            {
//                CreateRestedLevelFirstTime();
//            }
//            else                    // 1: else, we have created the engine before; just retieve the level from file
//            {
//                RetrieveLevelEngineStateFromFile();
//            }
//            PrepareActions();
//        }


//        private void CreateRestedLevelFirstTime()
//        {
//            using (Game1 game1 = new Game1())
//            {
//                StaticData.InitializeEngine(game1);
//                BuildLevel();
//                // Run Engine for 10 seconds till it rests
//                GameTime gameTime = new GameTime();

//                if (!IsDesign)
//                    Simulator.RunEngineFreely(10 * 60, gameTime);

//                // Save Physics Engine State
//                byte[] engineRestedBytes = ObjectSerializer.Serialize(StaticData.EngineManager);
//                File.WriteAllBytes(_engineFileString, engineRestedBytes);

//                byte[] levelGenBytes = ObjectSerializer.Serialize(this.LevelGeneratorEngine);
//                File.WriteAllBytes(_levelGenFileString, levelGenBytes);
//            }
//        }

//        private void RetrieveLevelEngineStateFromFile()
//        {
//            if (_isTotalNew)
//            {
//                using (Game1 game1 = new Game1())
//                {
//                    StaticData.InitializeEngine(game1);
//                }
//                BuildLevel();
//            }
//            else
//            {
//                try
//                {
//                    byte[] engineRestedBytes = File.ReadAllBytes(_engineFileString);
//                    StaticData.EngineManager = ObjectSerializer.Deserialize<EngineManager>(engineRestedBytes);

//                    StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;
                    
//                    byte[] levelGenBytes = File.ReadAllBytes(_levelGenFileString);
//                    this.LevelGeneratorEngine = ObjectSerializer.Deserialize<LevelGenerator>(levelGenBytes);
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.ToString());
//                    throw;
//                }
//            }
//        }


//        public void WritePlayablilityDistanceToFile()
//        {
//            String str = String.Empty;
//            Simulator.DistancesList.ForEach(t => str += t + " ");

//            StreamWriter sw = new StreamWriter(@"C:\CTREngine\Playability.txt");
//            if (Simulator.ClosestCookieFrogDistance > PlayabilityEngineSimulatorProlog.NarrativeDist)
//            {
//                sw.WriteLine(Simulator.ClosestCookieFrogDistance);
//            }
//            else
//            {
//                sw.WriteLine(0);
//                sw.Close();

//                sw = new StreamWriter(@"C:\CTREngine\PlayabilityBEST.txt");
//                sw.WriteLine(ShotsManager.DirNr);
//                sw.Write(ActionsGeneratorEngine.ActionsStrGeva + Environment.NewLine);
//                sw.Write(Simulator.ClosestCookieFrogDistance + Environment.NewLine);
//                sw.Write(str + Environment.NewLine);
//                str = "";
//                Simulator.CookiePosList.ForEach(t => str += t + Environment.NewLine);
//                sw.Write(str);
//            }
//            sw.Close();

//            sw = new StreamWriter(@"C:\CTREngine\PlayabilityALL.txt", true);
//            sw.Write(ShotsManager.DirNr + "\t");
//            sw.Write(Simulator.ClosestCookieFrogDistance.ToString());
//            sw.WriteLine("\t" + ActionsGeneratorEngine.ActionsStrGeva);
//            sw.WriteLine("__________________________");

//            if (IsTestHere)
//            {
//                sw.WriteLine(0);
//                sw = new StreamWriter(@"C:\CTREngine\PlayabilityBEST.txt");
//                sw.Write(ActionsGeneratorEngine.ActionsStrGeva + Environment.NewLine);
//                sw.Write(Simulator.ClosestCookieFrogDistance + Environment.NewLine);
//                sw.Write(str + Environment.NewLine);
//                str = "";
//                Simulator.CookiePosList.ForEach(t => str += t + Environment.NewLine);
//                sw.Write(str);
//            }
//            sw.Close();
//        }

//        private void BuildLevel()
//        {
//            try
//            {
//                String strLevel = String.Empty;
//                if (IsTestHere)
//                {
//                    strLevel = _strLevel;
//                }
//                else
//                {
//                    strLevel = Args[1];
//                }
//                LevelGeneratorEngine = new LevelGenerator(strLevel);
//                LevelGeneratorEngine.GenerateLevel();
//            }
//            catch (Exception)
//            {
//            }
//        }

//        private void PrepareActions()
//        {
//            try
//            {
//                String strActions = String.Empty;
//                if (IsTestHere)
//                {
//                    strActions = _strAction;
//                }
//                else
//                {
//                    strActions = Args[2];
//                }
//                ActionsGeneratorEngine = new ActionsGenerator(strActions);
//            }
//            catch (Exception)
//            {
//            }
//        }

//        public void RunEngine()
//        {
//            Simulator.RunEngine(this.ActionsGeneratorEngine);
//        }
//    }
//}