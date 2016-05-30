using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.ProjectEngines.PCGEngine
{
    class LevelBuilder
    {
        //public String StrLevel;
        //public LevelBuilder(string strLevel)
        //{
        //    StrLevel = strLevel;
        //}

        public static void CreateRestedLevel(string strLevel, bool isPlayabilityCheckerOnly)
        {
            Game1 game1 = null;
            if (StaticData.EngineManager != null)
            {
                game1 = StaticData.EngineManager.Game1;
                StaticData.EngineManager = null;
            }
            StaticData.GameSessionMode = SessionMode.DesignMode;
            StaticData.InitializeEngine(game1);
            var levelGeneratorEngine = new LevelGenerator(strLevel);
            levelGeneratorEngine.GenerateLevel();
        }

        public static void CreateRestedLevelForLevelsShots(string strLevel, bool isPlayabilityCheckerOnly)
        {
            Game1 game1 = null;
            StaticData.EngineManager = null;
            StaticData.GameSessionMode = SessionMode.DesignMode;
            StaticData.InitializeEngine(game1);
            var levelGeneratorEngine = new LevelGenerator(strLevel);
            levelGeneratorEngine.GenerateLevel();
        }

        //private void RetrieveLevelEngineStateFromFile()
        //{
        //    if (IsTotalNew)
        //    {
        //        using (Game1 game1 = new Game1())
        //        {
        //            StaticData.InitializeEngine(game1);
        //        }
        //        BuildLevel();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            byte[] engineRestedBytes = File.ReadAllBytes(_engineFileString);
        //            StaticData.EngineManager = ObjectSerializer.Deserialize<EngineManager>(engineRestedBytes);

        //            StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;

        //            byte[] levelGenBytes = File.ReadAllBytes(_levelGenFileString);
        //            this.LevelGeneratorEngine = ObjectSerializer.Deserialize<LevelGenerator>(levelGenBytes);
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.ToString());
        //            throw;
        //        }
        //    }
        //}
    }
}
