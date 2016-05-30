using System;
using System.Collections.Generic;
using System.IO;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers
{
    static class EntraPathGenManager
    {
        public static void SetFitnessValueForLevel(String[] args, List<Point> designerPath)
        {
            GenManager.GenerateGevaLevel(args, null);
            bool isOrdering = args[2] != "0";
            //EngineShotsManager shots = new EngineShotsManager();
            //shots.TakeEngineShot(false);
            DateTime d1 = DateTime.Now;

            EntraPlusGenManager.SetFitnessValueForLevel(args);

            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);

            DateTime d2 = DateTime.Now;
            int fitness = Int32.MaxValue;
            if (res.IsPlayable)
            {
                EntraPathAgent pathAgent = new EntraPathAgent(entraAgentSimple);
                fitness = pathAgent.GetFitnessValue(designerPath, isOrdering);
            }
            else
            {
                fitness = 100;
                //40 * (int)MathHelperModule.Normalize((int)res.MinDistToFrog, 400, 0) + 60;
            }

            DateTime d3 = DateTime.Now;

            int t1 = (int)(d2 - d1).TotalMilliseconds;
            int t2 = (int)(d3 - d2).TotalMilliseconds;
            

            //if (res.IsPlayable)
            //PRINTING
            {
                string strFile = "0"
                                 + "\t" + res.IsPlayable
                                 + "\t" + fitness
                                 + "\t" + res.MinDistToFrog
                                 + "\t" + t1
                                 + "\t" + t2
                                 + "\t" + (t1 + t2)
                                 + "\t" + String.Format("{0:0.00}", fitness)
                                 + "\t" + args[1];

                StreamWriter sw = new StreamWriter(@"C:\CTREngine\AllEntraPathEvolvedLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }


            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(fitness);
            swGEVA.Close();
        }


        public static void ShowTestResult(String[] args)
        {
            GenManager.GenerateGevaLevel(args, StaticData.EntraImageInput);

            //EngineShotsManager shots = new EngineShotsManager();
            //shots.TakeEngineShot(StaticData.EntraImageInput);

            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);
            EntraDrawer.DrawIntoFileTesting(res.ReachableSpace);

            EntraPathAgent pathAgent = new EntraPathAgent(entraAgentSimple);
            pathAgent.DrawPaths();
        }

        //public static void ShowTestResult(String[] args, List<Point> designerPath)
        //{
        //    GenManager.GenerateGevaLevel(args, StaticData.EntraImageInput);

        //    EngineShotsManager shots = new EngineShotsManager();
        //    shots.TakeEngineShot(StaticData.EntraImageInput);

        //    EntraAgentSimple EntraAgentSimple = new EntraAgentSimple();
        //    var res = EntraAgentSimple.CheckPlayability(StaticData.EngineManager);
        //    EntraDrawer.DrawIntoFileTesting(res.ReachableSpace);

        //    EntraPathAgent pathAgent = new EntraPathAgent(EntraAgentSimple);
        //    int fitness = pathAgent.GetFitnessValue(designerPath, true);
        //    pathAgent.DrawPaths();
        //}
    }
}

