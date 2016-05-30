using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.Starters;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.HelperModules
{
    static class FilesHelperModule
    {
        public static void SaveToStateFile(Action action)
        {
            StreamWriter sw = new StreamWriter(@"StatesFiles\" + Tester.FilesCounter + ".txt", true);
            sw.WriteLine(HelperModules.GenericHelperModule.GetActionsString(new List<Action>() {action}) + ";"
                + EngineStateManager.GetEngineStateFactStringWithSpaceDelimiterToStateFile() +
                ";" + CompsOfInterestProlog.GetCompsOfInterestFromProlog());
            sw.Flush();
            sw.Close();
        }

        public static void PrintTreeToFile(ActionNode baseNode, EngineManager engineManager, List<Action> performedActions)
        {
            StreamWriter sw = new StreamWriter(@"C:\CTREngine\CompsTreeFile.txt");
            int ropes = engineManager.SpringsManagerEngine.ListOfServices.Count;
            int rockets = engineManager.RocketsCarrierManagerEngine.GetListOfServices().Count;
            int blowers = engineManager.BlowerManagerEngine.ListOfServices.Count;
            int bubbles = engineManager.BubbleManagerEngine.ListOfServices.Count;
            int bumpers = engineManager.RigidsManagerEngine.ListOfBoxRigids.Where(r => r is BumpRigid).Count();
            sw.WriteLine(ropes + "," + rockets + "," + blowers + "," + bubbles + "," + bumpers);
            sw.WriteLine(GenericHelperModule.GetActionsString(performedActions));
            sw.Close();
            sw = new StreamWriter(@"C:\CTREngine\ActionsTreeFile.txt");
            int tabCounter = 0;
            PrintTreeRecursively(sw, baseNode, tabCounter);
            sw.Close();
        }

        private static void PrintTreeRecursively(StreamWriter sw, ActionNode baseNode, int tabCounter)
        {
            foreach (ActionNode actionNode in baseNode.Childs)
            {
                for (int i = 0; i < tabCounter; i++)
                {
                    sw.Write("\t");
                }
                sw.WriteLine(actionNode.Action);
                sw.Flush();
                PrintTreeRecursively(sw, actionNode, tabCounter + 1);
            }
        }


        //public static ActionNode RetrieveTreeFromFile()
        //{
        //    StreamReader sr = new StreamReader(@"C:\CTREngine\ActionsTreeFile.txt");
        //    int tabCounter = 0;
        //    ActionNode baseNode = new ActionNode(null);
        //    ReconsructTreeRecursively(sr, baseNode, tabCounter);
        //    sr.Close();
        //    return baseNode;
        //}

        //private static void ReconsructTreeRecursively(StreamReader sr, ActionNode baseNode, int tabCounter)
        //{

        //}
        public static void DeepCopyTreeToFile(ActionNode baseNode)
        {
            byte[] bytes = ObjectSerializer.Serialize(baseNode);
            File.WriteAllBytes(@"C:\CTREngine\TreeBytes.txt", bytes);
        }
    }
}
