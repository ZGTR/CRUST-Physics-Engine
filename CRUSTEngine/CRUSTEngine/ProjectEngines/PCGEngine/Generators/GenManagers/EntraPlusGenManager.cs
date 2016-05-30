using System;
using System.Collections.Generic;
using System.IO;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlus;
using CRUSTEngine.ProjectEngines.PCGEngine.TestModule;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers
{
    static class EntraPlusGenManager
    {
        public static int SetFitnessValueForLevel(String[] args)
        {
            GenManager.GenerateGevaLevel(args, null);
            String levelStr = args[1];
            bool isShortestPathOnlyComparsion = args[2] != "0";
            
            DateTime d1 = DateTime.Now;
            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);

            DateTime d2 = DateTime.Now;
            int fitness = Int32.MaxValue;
            int usedNoRopes = -1;
            int generatedNoRopes = -1;
            float usageFit = -1;
            if (res.IsPlayable)
            {
                List<List<PolyLog>> chosenPath;
                EntraPathAgent pathAgent = new EntraPathAgent(entraAgentSimple);
                GetFitnessUsage(pathAgent.AllPaths, levelStr, isShortestPathOnlyComparsion, out usedNoRopes,
                                out generatedNoRopes, out usageFit);
                fitness = (int) (40*0 + 60*usageFit);
            }
            else
            {
                fitness = (int) (40*MathHelperModule.Normalize((int) res.MinDistToFrog, 200, 50) + 60);
            }
            
            DateTime d3 = DateTime.Now;
            PrintDataToFiles(d1, d2, d3, res, fitness, args, usageFit, usedNoRopes, generatedNoRopes);
            return fitness;
        }

        public static void SetFitnessValueForLevelTest(String[] args, int counter)
        {
            GenManager.GenerateGevaLevel(args, null);
            String levelStr = args[1];
            bool isShortestPathOnlyComparsion = args[2] != "0";

            DateTime d1 = DateTime.Now;
            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);

            DateTime d2 = DateTime.Now;
            int fitness = Int32.MaxValue;
            int usedNoRopes = -1;
            int generatedNoRopes = -1;
            float usageFit = -1;
            if (res.IsPlayable)
            {
                EntraPathAgent pathAgent = new EntraPathAgent(entraAgentSimple);
                GetFitnessUsage(pathAgent.AllPaths, levelStr, isShortestPathOnlyComparsion, out usedNoRopes,
                                out generatedNoRopes, out usageFit);
                fitness = (int)(40 * 0 + 60 * usageFit);

                EngineShotsManager shots = new EngineShotsManager();
                shots.TakeEngineShot(StaticData.EntraImageInput);
                pathAgent.DrawShortestPath(counter);
            }
            else
            {
                fitness = (int)(40 * MathHelperModule.Normalize((int)res.MinDistToFrog, 300, 50) + 60);
            }

            DateTime d3 = DateTime.Now;

            StreamWriter sw = new StreamWriter(@"PolysTesting\test.txt", true);
            sw.WriteLine(res.IsPlayable + "\t"
                         + fitness + "\t"
                         + usedNoRopes + "\t"
                         + generatedNoRopes + "\t"
                         + String.Format("{0:0.00}", usageFit));
            sw.Close();
            //PrintDataToFiles(d1, d2, d3, res, fitness, args);
        }

        private static void GetFitnessUsage(List<List<PolyLog>> allPaths, string levelStr,
            bool isShortestPathOnlyComparsion, out int usedNoRopes, out int generatedNoRopes, out float usageFit)
        {
            EntraPlusUsageManager usageManager = new EntraPlusUsageManager(allPaths, isShortestPathOnlyComparsion);
            usageManager.DoAnalysis();

            List<Component> items = new LevelGenerator(levelStr).Items;
            items.RemoveAll(x => x is Frog);
            items.RemoveAll(x => x is Cookie);
            items.RemoveAll(x => x is Rope);
            
            usedNoRopes = usageManager.GetUsedCompsCountNoRopes();
            generatedNoRopes = items.Count;

            usageFit = ((generatedNoRopes - usedNoRopes) / (float)generatedNoRopes);
        }

        private static void PrintDataToFiles(DateTime d1, DateTime d2, DateTime d3, 
            EntraResult res, int fitness, String[] args, float usageFit, int usedNoRopes, int generatedNoRopes)
        {
            int t1 = (int)(d2 - d1).TotalMilliseconds;
            int t2 = (int)(d3 - d2).TotalMilliseconds;

            //PRINTING
            {
                string strFile = "0"
                                 + "\t" + res.IsPlayable
                                 + "\t" + fitness
                                 + "\t" + res.MinDistToFrog
                                 + "\t" + t1
                                 + "\t" + t2
                                 + "\t" + (t1 + t2)
                                 + "\t" + usedNoRopes
                                 + "\t" + generatedNoRopes
                                 + "\t" + String.Format("{0:0.00}", usageFit)
                                 + "\t" + args[1];

                StreamWriter sw = new StreamWriter(@"C:\CTREngine\AllEntraPlusEvolvedLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }


            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(fitness);
            swGEVA.Close();

        }
    }
}
