using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CRUSTEngine.ProjectEngines.Starters;
using Microsoft.Xna.Framework;
using RigidsInMotion.UIDigitaRune;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Water;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.FormsManipualtion
{
    public partial class CTRLevelDesigner : Form
    {
        public CTRLevelDesigner()
        {
            InitializeComponent();
        }

        ////void valueProperty_Changed(object sender, e)
        ////{
        ////    try
        ////    {
        ////        TextBlock textBlock = "tbSliderValue") as TextBlock;
        ////        StaticData.Dtime = Int32.Parse(textBlock.Text) / 1000f;
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}

        //void bDeleteRigid_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.DeleteRigidMode;
        //}

        ////void bCutTheRope_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        int ropeID = (int)UIControlsParser.GetFloat(tbRopeID);
        ////        StaticData.EngineManager.SpringsManagerEngine.RemoveService(ropeID);
        ////    }
        ////    catch
        ////    {
        ////    }
        ////}

        //void bSelectRigidResize_Click(object sender, EventArgs e)
        //{
        //    if (StaticData.CurrentVisual2D is RigidBody)
        //        StaticData.EngineManager.ResizeManagerEngine.CurrentVisual2DResize = StaticData.CurrentVisual2D as RigidBody;
        //}

        ////void bConnentToRope_Click(object sender, EventArgs e)
        ////{
        ////    int ropeID = (int)UIControlsParser.GetFloat(tbRopeID);
        ////    if (ropeID < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count)
        ////    {
        ////        if (StaticData.RigidToConnectToRope != null)
        ////        {
        ////            try
        ////            {
        ////                StaticData.EngineManager.SpringsManagerEngine.ListOfServices[ropeID].ApplyServiceOnRigid
        ////                    (StaticData.RigidToConnectToRope);
        ////            }
        ////            catch (Exception)
        ////            {
        ////            }
        ////        }
        ////    }
        ////}

        //void bSelectRigidRope_Click(object sender, EventArgs e)
        //{
        //    if (StaticData.CurrentVisual2D is RigidBody)
        //        StaticData.RigidToConnectToRope = StaticData.CurrentVisual2D as RigidBody;
        //}

        //void bDragRigids_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
        //}

        //void bResizeRigids_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.ResizeRigidMode;
        //}

        //void bMForces_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.ForceManipulationMode;
        //}

        //void bMTorques_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.TorqueManipulationMode;
        //}

        ////void bAddRope_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        StaticData.ManipulationGameMode = ManipulationGameMode.AddRopeMode;
        ////        int nrOfMasses = (int)UIControlsParser.GetFloat(tbNrOfMasses);
        ////        float springConstant = UIControlsParser.GetFloat(tbSpringConstant);
        ////        float normalLength = UIControlsParser.GetFloat(tbNormalLength);
        ////        float springInnerFriction = UIControlsParser.GetFloat(tbSpringInnerFriction);
        ////        Vector3 initialPosition = UIControlsParser.GetVector3(tbPositionRope);
        ////        RigidType rigidType = (RigidType)UIControlsParser.GetIndexOfSelection(ddTypeRigidRope);
        ////        Vector3 rigidSize = UIControlsParser.GetVector3(tbRigidSizeRope);
        ////        bool isCollidable = cbIsCollidableRope.Checked;
        ////        SpringType type = (SpringType)UIControlsParser.GetIndexOfSelection(cbSpringType);
        ////        if (rigidSize != Vector3.Zero)
        ////        {
        ////            StaticData.EngineManager.SpringsManagerEngine.AddNewService(
        ////                DefaultAdder.GetDefaultSpringRope(initialPosition,
        ////                                            nrOfMasses,
        ////                                            springConstant,
        ////                                            normalLength,
        ////                                            springInnerFriction,
        ////                                            rigidType,
        ////                                            rigidSize,
        ////                                            isCollidable,
        ////                                            type));
        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}

        //void bNewSketch_Click(object sender, EventArgs e)
        //{
        //    StaticData.EngineManager.Initialize();
        //}

        //void bPause_Click(object sender, EventArgs e)
        //{
        //    StaticData.CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
        //}

        //void bPlayNow_Click(object sender, EventArgs e)
        //{
        //    StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;
        //}

        //void bDesignGame_Click(object sender, EventArgs e)
        //{
        //    //ManipulateBooleanLeft(ref ShouldShowDesignDock, GroupBoxDesignDock);
        //}

        //void bManipulateRT_Click(object sender, EventArgs e)
        //{
        //    //ManipulateBooleanLeft(ref ShouldShowManipulateRTDock, GroupBoxManipulateRTDock);
        //}

        //public void bControlPanel_Click(object sender, EventArgs e)
        //{
        //    //ManipulateBoolean(ref ShouldShowTopDock, GroupBoxTopDock);
        //}

        ////private void bMoveTheRope_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        StaticData.ManipulationGameMode = ManipulationGameMode.MoveRope;
        ////        StaticData.CurrentRopeIdToMove = Int32.Parse(this.tbRopeID.Text);
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}

        //private void bBubbleMode_Click(object sender, EventArgs e)
        //{
        //    //StaticData.BubbleDimBubble = (int)UIControlsParser.GetFloat(StaticData.BubbleDimBubble);
        //    StaticData.ManipulationGameMode = ManipulationGameMode.AddBubblesMode;
        //}

        //private void bDeleteBubble_Click(object sender, EventArgs e)
        //{
        //    StaticData.ManipulationGameMode = ManipulationGameMode.DeleteBubblesMode;
        //}

        ////private void bSetParametersLiquid_Click(object sender, EventArgs e)
        ////{
        ////    int liquidLevel = Int32.Parse(nLiquidLevel.Value.ToString());
        ////    int liquidDensity = Int32.Parse(nLiquidDensity.Value.ToString());
        ////    Vector3 force = UIControlsParser.GetVector3(tbForceLiquid);
        ////    Vector3 torque = UIControlsParser.GetVector3(tbTorqueLiquid);
        ////    StaticData.IsWater = true;
        ////    LiquidService.LiquidLevel = liquidLevel;
        ////    LiquidService.LiquidDensity = liquidDensity;
        ////    LiquidService.ForceLiquid = force;
        ////    LiquidService.TorqueLiquid = torque;
        ////}

        //private void bToggleWater_Click(object sender, EventArgs e)
        //{
        //    StaticData.IsWater = !StaticData.IsWater;
        //}

        //private void nLiquidLevel_ValueChanged(object sender, EventArgs e)
        //{
        //    int liquidLevel = Int32.Parse(nLiquidLevel.Value.ToString());
        //    LiquidService.LiquidLevel = liquidLevel;
        //}

        //private void bRocketMode_Click(object sender, EventArgs e)
        //{
        //    //StaticData.ManipulationGameMode = ManipulationGameMode.RocketMode;
        //}

        //private void bHCSetJoints_Click(object sender, EventArgs e)
        //{
        //    StaticData.CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
        //    StaticData.ManipulationGameMode = ManipulationGameMode.SetJointsHardConstraints;
        //}

        //private void bHCRealtimeRopeCreation_Click(object sender, EventArgs e)
        //{

        //    StaticData.EngineManager.MouseRigidsAdderEngine.CurrentJointsRigids.Clear();
        //}

        //private void bResetEngine_Click(object sender, EventArgs e)
        //{
        //    StaticData.EngineManager = new EngineManager(StaticData.EngineManager.Game1);
        //}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void bDeletionMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DeleteRigidMode;
        }

        private void bDragMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
        }

        private void bBubblesMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddBubblesMode;
        }

        private void bBlowersMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddBlowersMode;
        }

        private void bBumpsMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddBumpsMode;
        }

        private void bRocketsMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddRocketsMode;
        }

        private void bCatchableRopesMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddCatchableRopesMode;
        }

        private void bRopesMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.AddRopesMode;
        }

        private void bLiquidMode_Click(object sender, EventArgs e)
        {
            if (StaticData.GameSessionMode == SessionMode.DesignMode)
            {
                StaticData.IsWater = !(StaticData.IsWater);
            }
        }

        private void bResetLevel_Click(object sender, EventArgs e)
        {
            //StaticData.EngineManager = new EngineManager(StaticData.EngineManager.Game1);
            StaticData.GameSessionMode = SessionMode.DesignMode;
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
            DesignerManager.InitializeEngineForDesignerWithComponents();
        }

        private void LevelDesigner_Load(object sender, EventArgs e)
        {
            MusicManager.Play();
            StaticData.CtrLevelDesigner = this;
            StaticData.GameSessionMode = SessionMode.DesignMode;
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
            StaticData.EngineManager.NotificationManagerEngine.PushNotification(
                NotificationType.DirectionOfBumpsBlowersRockets);
            StaticData.EngineManager.NotificationManagerEngine.PushNotification(
                NotificationType.HeightOfRope);
        }

        private void bChangeCompsDir_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.ChangingCompsDirection;
        }

        private void bSetNeutralMode_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
        }

        private void nLiquidLevel_ValueChanged(object sender, EventArgs e)
        {
            if (StaticData.GameSessionMode == SessionMode.DesignMode)
            {
                int liquidLevel = Int32.Parse(nLiquidLevel.Value.ToString());
                LiquidService.LiquidLevel = liquidLevel;
            }
        }

        private void bPlayThisLevel_Click(object sender, EventArgs e)
        {
            if (StaticData.GameSessionMode != SessionMode.PlayingMode)
            {
                StaticData.SetEngineManagerLastLevel(ObjectSerializer.DeepCopy(StaticData.EngineManager));
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
            }
        }

        private void bLoadLastDesign_Click(object sender, EventArgs e)
        {
            if (StaticData.GetEngineManagerLastLevel() != null)
            {
                Game1 game1 = StaticData.EngineManager.Game1;
                StaticData.EngineManager = ObjectSerializer.DeepCopy(StaticData.GetEngineManagerLastLevel());
                StaticData.EngineManager.Game1 = game1;
                StaticData.GameSessionMode = SessionMode.DesignMode;
            }
        }

        private void cbShowNotif_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.IsNotification = this.cbShowNotif.Checked;
        }

        private void cbMusicOn_CheckedChanged(object sender, EventArgs e)
        {
            if(this.cbMusicOn.Checked == false)
            {
                MusicManager.Stop();
            }
            else
            {
                MusicManager.Play();
            }
        }

        private void bSaveLevel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dialog.FileName, true);
                sw.WriteLine(EngineStateManager.GetEngineStateFactStringWithSpaceDelimiterGEVAStyle());
                sw.Close();
            }
        }

        private void bCheckPlayabilty_Click(object sender, EventArgs e)
        {
            try
            {
                RYSEGenManager.SimulatePlayabiltityFromDesigner();
                SaveEngineState();
                try
                {
                    if (DesignEnhanceManager.PlayabilityActions != String.Empty)
                    {
                        LevelBuilder.CreateRestedLevel(DesignEnhanceManager.GevaLevel, false);
                        //RYSEManager manager = new RYSEManager(10);
                        //manager.IsSaveImage = false;
                        StaticData.GameSessionMode = SessionMode.PlayingMode;
                        //EngineShotsManager.ShowXNAWindow();
                        LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                        simulator.SimulateSameWindow(new ActionsGenerator(DesignEnhanceManager.PlayabilityActions).Actions);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SaveEngineState()
        {
            if (StaticData.GameSessionMode != SessionMode.PlayingMode)
            {
                StaticData.SetEngineManagerLastLevel(ObjectSerializer.DeepCopy(StaticData.EngineManager));
            }
        }

        private void bSaveEngineState_Click(object sender, EventArgs e)
        {
            SaveEngineState();
        }

        private void bLoadLevel_Click(object sender, EventArgs e)
        {
            new SetLevelDialog().Show();
        }

        private void bDesignMode_Click(object sender, EventArgs e)
        {
            StaticData.GameSessionMode = SessionMode.DesignMode;
        }

        private void bSetStaticComps_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.SetStaticComps;
        }

        
        private void bEnhanceDesignAuto_Click(object sender, EventArgs e)
        {
            DesignEnhanceManager.EnhanceDesign(false);
        }

        private void bShowSolutionImm_Click(object sender, EventArgs e)
        {
            SaveEngineState();
            try
            {
                if (DesignEnhanceManager.PlayabilityActions != String.Empty)
                {
                    LevelBuilder.CreateRestedLevel(DesignEnhanceManager.GevaLevel, false);
                    //RYSEManager manager = new RYSEManager(10);
                    //manager.IsSaveImage = false;
                    StaticData.GameSessionMode = SessionMode.PlayingMode;
                    //EngineShotsManager.ShowXNAWindow();
                    LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
                    simulator.SimulateSameWindow(new ActionsGenerator(DesignEnhanceManager.PlayabilityActions).Actions);
                }
            }
            catch (Exception)
            {
            }
        }

        private void rbPCNone_CheckedChanged(object sender, EventArgs e)
        {
            bShowSolutionImm.Visible = false;
        }

        private void rbPCNormalCheck_CheckedChanged(object sender, EventArgs e)
        {
            bShowSolutionImm.Visible = true;
        }

        private void rbPCRandom_CheckedChanged(object sender, EventArgs e)
        {
            bShowSolutionImm.Visible = true;
        }

        //private void bStopPlayThread_Click(object sender, EventArgs e)
        //{
        //    DesignEnhanceManager.PlayThread.Abort();
        //}

        private void bEnhanceDesignSetGrammar_Click(object sender, EventArgs e)
        {
            String strGrammar = String.Empty;
            strGrammar = StaticData.EngineManager.PrefCompsManager.GetPrefCompsToGrammarFile();
            StreamWriter sw = new StreamWriter(DesignEnhanceManager.FileGrammarPath);
            sw.Write(strGrammar);
            sw.Flush();
            sw.Close();
            new SetGrammarDialog().Show();
        }

        private int levelNrSameComps = 0;
        private void bViewSamplesOneComps_Click(object sender, EventArgs e)
        {
            if (levelNrSameComps < 160)
            {
                Tester.WatchLivePlayabilityFromLevelsActionsFilesSameCompsForDesigner(levelNrSameComps);
                StaticData.GameSessionMode = SessionMode.PlayingMode;
                StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
                levelNrSameComps++;
            }
            else
                levelNrSameComps = 0;
        }

        private int levelAllComps = 0;


        private void bPlayableSampleAllComps_Click(object sender, EventArgs e)
        {
            if (levelAllComps < 100)
            {
                Tester.WatchLivePlayabilityFromLevelsActionsFilesAllCompsForDesigner(levelAllComps);
                StaticData.GameSessionMode = SessionMode.DesignMode;
                StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
                levelAllComps++;
            }
            else
                levelAllComps = 0;
        }

        private void bGenerateSamples_Click(object sender, EventArgs e)
        {
            GenericHelperModule.RunJavaProcess(@"C:\CTREngine\AuthoringToolEngineGEVAOnly.jar");
            StreamReader sr = new StreamReader(@"C:\CTREngine\EvolvedLevel.txt");
            String gevaLevel = sr.ReadToEnd().Split('\n')[0];
            sr.Close();
            LevelBuilder.CreateRestedLevel(gevaLevel, false);
            StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
            StaticData.GameSessionMode = SessionMode.DesignMode;
        }

        private void bShowSolutionTree_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\CTREngine\InspectorTreeGraph.exe");
        }

        private void bActivateEntraAgent_Click(object sender, EventArgs e)
        {
            StaticData.IsEntraActivated = !StaticData.IsEntraActivated;
            if (StaticData.IsEntraActivated)
            {
                bActivateEntraAgent.Text = @"Dectivate Entra Agent";
            }
            else
            {
                bActivateEntraAgent.Text = @"Activate Entra Agent";
            }
        }

        private void bActivateEntraPath_Click(object sender, EventArgs e)
        {
            StaticData.IsEntraPathActivated = !StaticData.IsEntraPathActivated;
            if (StaticData.IsEntraPathActivated)
            {
                bActivateEntraPath.Text = @"Dectivate Entra Path Agent";
            }
            else
            {
                bActivateEntraPath.Text = @"Activate Entra Path Agent";
            }
        }

        private void bPathNext_Click(object sender, EventArgs e)
        {
            StaticData.EntraPathIndex += 1;
        }

        private void bPathPrev_Click(object sender, EventArgs e)
        {
            StaticData.EntraPathIndex -= 1;
        }

        private void bGenSimForm_Click(object sender, EventArgs e)
        {
            new GenSimTimeBarForm().Show();
        }
    }
}
