using System;
using System.IO;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers
{
    static class EntraGenManager
    {
        public static void SetFitnessValueForLevel(String[] args)
        {
            GenManager.GenerateGevaLevel(args, null);
            EntraAgentSimple entraAgentSimple = new EntraAgentSimple();
            DateTime d1 = DateTime.Now;
            float fitness = 0;


            // Shortcut
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            Vector2 fPos = StaticData.EngineManager.FrogRB.PositionXNACenter2D;
            EntraResult res = null;
            if ((cPos - fPos).Length() < 125)
            {
                fitness = 1000;
            }
            else
            {
                res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);
                if (res.IsPlayable)
                {
                    fitness = 0;
                }
                else
                {
                    fitness = res.MinDistToFrog;
                }
            }
            DateTime d2 = DateTime.Now;


            bool isPlayable = false;
            if (res != null)
            {
                isPlayable = res.IsPlayable;
            }
            else
            {
                isPlayable = false;
            }

            string strFile = "0"
                             + "\t" + isPlayable
                             + "\t" + (d2 - d1).TotalMilliseconds
                             + "\t" + String.Format("{0:0.00}", fitness)
                             + "\t" + args[1];

            if (isPlayable)
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\EntraGenPlayableLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(@"C:\CTREngine\EntraGenNonPlayableLevels.txt", true);
                sw.WriteLine(strFile);
                sw.Flush();
                sw.Close();
            }

            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(fitness);
            swGEVA.Close();
        }


        public static void PrintEffetivceSpace(String[] args, bool withLevelImage, int i)
        {
            if (withLevelImage)
            {
                String strImage = StaticData.EntraImageInput;
                GenManager.GenerateGevaLevel(args, strImage);
                EntraAgentSimple entraAgentSimple = new EntraAgentSimple();

                var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);
                EntraDrawer.DrawIntoFileGeva(res.ReachableSpace, true, i);
            }
            else
            {
                GenManager.GenerateGevaLevel(args, null);
                EntraAgentSimple entraAgentSimple = new EntraAgentSimple();

                var res = entraAgentSimple.CheckPlayability(StaticData.EngineManager);
                EntraDrawer.DrawIntoFileGeva(res.ReachableSpace, false, i);
            }

        }
    }
}
