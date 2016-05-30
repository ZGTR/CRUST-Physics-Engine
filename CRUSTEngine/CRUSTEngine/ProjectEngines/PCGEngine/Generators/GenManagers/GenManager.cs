using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers
{
    public class GenManager
    {
        public static void GenerateGevaLevel(String[] args, bool isPrintPositionOnly, bool isSaveImage)
        {
            LevelBuilder.CreateRestedLevelForLevelsShots(args[1], isPrintPositionOnly);
            if (isSaveImage)
            {
                EngineShotsManager shotsManager = new EngineShotsManager();
                shotsManager.TakeEngineShot();
            }

            if (isPrintPositionOnly)
            {
                string pos = EngineStateManager.GetEngineStatePositionsOnlyFactString();
                StreamWriter sw = new StreamWriter("PhysicsEngine_PositionsOnly.txt", true);
                sw.WriteLine(pos);
                sw.Flush();
                sw.Close();
            }
        }

        public static void GenerateGevaLevel(String[] args, String imageName = null)
        {
            LevelBuilder.CreateRestedLevelForLevelsShots(args[1], false);
            if (imageName != null)
            {
                EngineShotsManager shotsManager = new EngineShotsManager();
                shotsManager.TakeEngineShot(imageName);
            }
        }
    }
}
