using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using RigidsInMotion.UIDigitaRune;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Water;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine
{
    public partial class CRUSTLevelDesigner : Form
    {
        public CRUSTLevelDesigner()
        {
            InitializeComponent();
        }

        //void valueProperty_Changed(object sender, e)
        //{
        //    try
        //    {
        //        TextBlock textBlock = "tbSliderValue") as TextBlock;
        //        StaticData.Dtime = Int32.Parse(textBlock.Text) / 1000f;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        void bDeleteRigid_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DeleteRigidMode;
        }

        void bCutTheRope_Click(object sender, EventArgs e)
        {
            try
            {
                int ropeID = (int)UIControlsParser.GetFloat(tbRopeID);
                StaticData.EngineManager.SpringsManagerEngine.RemoveService(ropeID);
            }
            catch
            {
            }
        }

        void bSelectRigidResize_Click(object sender, EventArgs e)
        {
            if (StaticData.CurrentVisual2D is RigidBody)
                StaticData.EngineManager.ResizeManagerEngine.CurrentVisual2DResize = StaticData.CurrentVisual2D as RigidBody;
        }

        void bConnentToRope_Click(object sender, EventArgs e)
        {
            int ropeID = (int)UIControlsParser.GetFloat(tbRopeID);
            if (ropeID < StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count)
            {
                if (StaticData.RigidToConnectToRope != null)
                {
                    try
                    {
                        StaticData.EngineManager.SpringsManagerEngine.ListOfServices[ropeID].ApplyServiceOnRigid
                            (StaticData.RigidToConnectToRope);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        void bSelectRigidRope_Click(object sender, EventArgs e)
        {
            if (StaticData.CurrentVisual2D is RigidBody)
                StaticData.RigidToConnectToRope = StaticData.CurrentVisual2D as RigidBody;
        }

        void bDragRigids_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
        }

        void bResizeRigids_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.ResizeRigidMode;
        }

        void bMForces_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.ForceManipulationMode;
        }

        void bMTorques_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.TorqueManipulationMode;
        }

        void bAddRope_Click(object sender, EventArgs e)
        {
            try
            {
                StaticData.ManipulationGameMode = ManipulationGameMode.AddRopesMode;
                int nrOfMasses = (int) UIControlsParser.GetFloat(tbNrOfMasses);
                float springConstant = UIControlsParser.GetFloat(tbSpringConstant);
                float normalLength = UIControlsParser.GetFloat(tbNormalLength);
                float springInnerFriction = UIControlsParser.GetFloat(tbSpringInnerFriction);
                Vector3 initialPosition = UIControlsParser.GetVector3(tbPositionRope);
                RigidType rigidType = (RigidType)UIControlsParser.GetIndexOfSelection(ddTypeRigidRope);
                Vector3 rigidSize = UIControlsParser.GetVector3(tbRigidSizeRope);
                bool isCollidable = cbIsCollidableRope.Checked;
                SpringType type = (SpringType) UIControlsParser.GetIndexOfSelection(cbSpringType);
                if (rigidSize != Vector3.Zero)
                {
                    StaticData.EngineManager.SpringsManagerEngine.AddNewService(
                        DefaultAdder.GetDefaultSpringRope(initialPosition,
                        StaticData.EngineManager.CookieRB.PositionXNA,
                                                    nrOfMasses,
                                                    springConstant,
                                                    normalLength,
                                                    springInnerFriction,
                                                    rigidType,
                                                    rigidSize,
                                                    isCollidable,
                                                    type));
                }
            }
            catch(Exception)
            {
            }
        }
        
        void bAddRectangles_Click(object sender, EventArgs e)
        {
            StaticData.BoxDefaultSize = UIControlsParser.GetVector3(tbBoxSizeDefaultBox);
            StaticData.ManipulationGameMode = ManipulationGameMode.AddDefaultRectangleMode;
        }

        void bAddCircles_Click(object sender, EventArgs e)
        {
            StaticData.CircleDefaultSize = (int)UIControlsParser.GetFloat(tbCircleSizeDefaultCircle);
            StaticData.ManipulationGameMode = ManipulationGameMode.AddDefaultCircleMode;
        }

        void bAddRigid_Click(object sender, EventArgs e)
        {
            int index = UIControlsParser.GetIndexOfSelection(ddTypeRigid);
            Vector3 positionXNA = UIControlsParser.GetVector3(tbPositionXNARigid);
            Material material = UIControlsParser.GetMaterial(ddMaterialRigid);

            int width = (int)UIControlsParser.GetFloat(tbWidthRigid);
            int height = (int)UIControlsParser.GetFloat(tbHeightRigid);
            Vector3 halfSize = new Vector3((int)width / 2, (int)height / 2, 0);

            int radius = (int)UIControlsParser.GetFloat(tbRadiusRigid);
            Vector3 acc = UIControlsParser.GetVector3(tbAccRigid);
            Vector3 initialForce = UIControlsParser.GetVector3(tbForceRigid);
            Vector3 initialTorque = UIControlsParser.GetVector3(tbTorqueRigid) * 100000;
            float orientationValue = UIControlsParser.GetFloat(tbOrientationRigid);
            bool obInertia = UIControlsParser.GetBool(ddInertiaRigid);

            if (index == 0 && width != 0 && height != 0)
            {
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(
                    DefaultAdder.GetDefaultBox(positionXNA,
                                                     material,
                                                     halfSize,
                                                     acc,
                                                     initialForce,
                                                     initialTorque,
                                                     orientationValue,
                                                     obInertia));
            }
            else
            {
                if (radius != 0 && index == 1)
                {
                    StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(
                        DefaultAdder.GetDefaultSphere(positionXNA,
                                                      material,
                                                      radius,
                                                      acc,
                                                      initialForce,
                                                      initialTorque,
                                                      orientationValue,
                                                      obInertia));
                }
            }
        }

        void bNewSketch_Click(object sender, EventArgs e)
        {
            StaticData.EngineManager.Initialize();
        }

        void bPause_Click(object sender, EventArgs e)
        {
            StaticData.CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
        }

        void bPlayNow_Click(object sender, EventArgs e)
        {
            StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;
        }

        void bDesignGame_Click(object sender, EventArgs e)
        {
            //ManipulateBooleanLeft(ref ShouldShowDesignDock, GroupBoxDesignDock);
        }

        void bManipulateRT_Click(object sender, EventArgs e)
        {
            //ManipulateBooleanLeft(ref ShouldShowManipulateRTDock, GroupBoxManipulateRTDock);
        }

        public void bControlPanel_Click(object sender, EventArgs e)
        {
            //ManipulateBoolean(ref ShouldShowTopDock, GroupBoxTopDock);
        }

        private void bMoveTheRope_Click(object sender, EventArgs e)
        {
            try
            {
                StaticData.ManipulationGameMode = ManipulationGameMode.MoveRope;
                StaticData.CurrentRopeIdToMove = Int32.Parse(this.tbRopeID.Text);
            }
            catch (Exception)
            {
            }
        }

        private void bBubbleMode_Click(object sender, EventArgs e)
        {
            if ((int)UIControlsParser.GetFloat(tbBubbleDim) > 0)
            {
                StaticData.BubbleDimBubble = (int)UIControlsParser.GetFloat(tbBubbleDim);
                StaticData.ManipulationGameMode = ManipulationGameMode.AddBubblesMode;
            }
        }

        private void bDeleteBubble_Click(object sender, EventArgs e)
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DeleteBubblesMode;
        }

        private void bSetParametersLiquid_Click(object sender, EventArgs e)
        {
            int liquidLevel = Int32.Parse(nLiquidLevel.Value.ToString());
            int liquidDensity = Int32.Parse(nLiquidDensity.Value.ToString());
            Vector3 force = UIControlsParser.GetVector3(tbForceLiquid);
            Vector3 torque = UIControlsParser.GetVector3(tbTorqueLiquid);
            StaticData.IsWater = true;
            LiquidService.LiquidLevel = liquidLevel;
            LiquidService.LiquidDensity = liquidDensity;
            LiquidService.ForceLiquid = force;
            LiquidService.TorqueLiquid = torque;
        }

        private void bToggleWater_Click(object sender, EventArgs e)
        {
            StaticData.IsWater = !StaticData.IsWater;
        }

        private void nLiquidLevel_ValueChanged(object sender, EventArgs e)
        {
            int liquidLevel = Int32.Parse(nLiquidLevel.Value.ToString());
            LiquidService.LiquidLevel = liquidLevel;
        }

        private void nLiquidDensity_ValueChanged(object sender, EventArgs e)
        {
            int liquidDensity = Int32.Parse(nLiquidDensity.Value.ToString());
            LiquidService.LiquidDensity = liquidDensity;
        }

        private void bRocketMode_Click(object sender, EventArgs e)
        {
            //StaticData.ManipulationGameMode = ManipulationGameMode.RocketMode;
        }

        private void bHCSetJoints_Click(object sender, EventArgs e)
        {
            StaticData.CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
            StaticData.ManipulationGameMode = ManipulationGameMode.SetJointsHardConstraints;
        }

        private void bHCCreateRopeOfRods_Click(object sender, EventArgs e)
        {
            Vector3 posXNA = UIControlsParser.GetVector3(this.tbHCPosXNA);
            float linkHeight = UIControlsParser.GetFloat(this.tbHCLinkHeight);
            int rigidHalfSize = (int)UIControlsParser.GetFloat(this.tbHCRigidHalfSize);
            Vector3 gravity = UIControlsParser.GetVector3(this.tbHCGravityVec);
            //bool isExpandable = this.cbHCIsExpandable.Checked;
            float forgettingFactor = UIControlsParser.GetFloat(this.tbHCForgettingFactor);
            bool isFixed = this.cbHCIsFixed.Checked;
            bool isClosed = this.cbHCIsClosed.Checked;

            int nrOfMasses = (int)UIControlsParser.GetFloat(this.tbHCNrOfMasses);
            int spacing = (int)UIControlsParser.GetFloat(this.tbHCSpacing);

            RopeOfRods rope = new RopeOfRods(posXNA, isFixed, isClosed, nrOfMasses, gravity, spacing, rigidHalfSize,
                                                linkHeight, forgettingFactor);
            StaticData.EngineManager.RopeOfRodsManagerEngine.ListOfRopeOfRods.Add(rope);
        }

        private void bHCRealtimeRopeCreation_Click(object sender, EventArgs e)
        {
            
            StaticData.EngineManager.MouseRigidsAdderEngine.CurrentJointsRigids.Clear();
        }

        private void bHCCutTheRods_Click(object sender, EventArgs e)
        {
            try
            {
                int rodsId = (int)UIControlsParser.GetFloat(this.tbHCRodID);
                StaticData.EngineManager.RopeOfRodsManagerEngine.RemoveService(rodsId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void bResetEngine_Click(object sender, EventArgs e)
        {
            StaticData.EngineManager = new EngineManager(StaticData.EngineManager.Game1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
