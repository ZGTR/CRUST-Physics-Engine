using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    static class DesignEnhanceManager
    {
        public static String GevaLevel = String.Empty;
        public static String PlayabilityActions = String.Empty;
        //public static int GeLevelEvoCounter = 0;
        public static Thread PlayThread = null;
        public const String FileGrammarPath = @"C:\CTREngine\cut_the_rope_level_gen_pAuthoring.bnf";

        public static void EnhanceDesign(bool gammarsSetByUser)
        {
            if (!gammarsSetByUser)
            {
                String strGrammar = String.Empty;
                strGrammar = StaticData.EngineManager.PrefCompsManager.GetPrefCompsToGrammarFile();
                StreamWriter sw = new StreamWriter(FileGrammarPath);
                sw.Write(strGrammar);
                sw.Flush();
                sw.Close();
            }
            if (StaticData.CtrLevelDesigner.rbPCNone.Checked || 
                (
                    (StaticData.CtrLevelDesigner.rbPCNone.Checked == false
                    && StaticData.CtrLevelDesigner.rbPCNormalCheck.Checked == false)
                    && StaticData.CtrLevelDesigner.rbPCRandom.Checked == false
                )
                )
            {
                GenericHelperModule.RunJavaProcess(@"C:\CTREngine\AuthoringToolEngineGEVAOnly.jar");
                StreamReader sr = new StreamReader(@"C:\CTREngine\EvolvedLevel.txt");
                GevaLevel = sr.ReadToEnd().Split('\n')[0];
                PlayabilityActions = String.Empty;
                sr.Close();
            }
            else
            {
                if (StaticData.CtrLevelDesigner.rbPCNormalCheck.Checked)
                {
                    GenericHelperModule.RunJavaProcess(
                        @"C:\CTREngine\AuthoringToolEngineWithNormalPlayFitness.jar");
                }
                else
                {
                    if (StaticData.CtrLevelDesigner.rbPCRandom.Checked)
                    {
                        GenericHelperModule.RunJavaProcess(
                            @"C:\CTREngine\AuthoringToolEngineWithRandomPlayFitness.jar");
                    }
                }
                StreamReader sr =
                    new StreamReader(@"C:\CTREngine\PhysicsEngine_EvolvePlayActions.txt");
                String line = sr.ReadToEnd();
                GevaLevel = line.Split('\t')[13];
                PlayabilityActions = line.Split('\t')[14];
                if (line.Split('\t')[1].ToLower() == "true")
                {
                    MessageBox.Show(
                        @"Playability-check is finished. The engine has found a playable level.");
                }
                sr.Close();
            }
            LevelBuilder.CreateRestedLevel(DesignEnhanceManager.GevaLevel, false);
            StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
            StaticData.GameSessionMode = SessionMode.DesignMode;
        }
    }
}
