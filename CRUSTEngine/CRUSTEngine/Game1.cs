using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Xna.Framework.Media;
using CRUSTEngine.FormsManipualtion;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using LevelDesigner = CRUSTEngine.FormsManipualtion.CTRLevelDesigner;


namespace CRUSTEngine
{
    public class Game1 : Game
    {
        public string[] Args { get; set; }
        public GraphicsDeviceManager Graphics { set; get; }
        public SpriteBatch SpriteBatch;
        public static bool IsUserDesigner = false;
        public static bool IsZgtrPlaying = false;
        public bool IsDrawJustOneTime = false;
        public bool IsPlayabilityCheckerOnly = false;
        public bool ShouldTakeShotNow = false;
        public bool ShouldUpdate = true;

        public Game1()
        {
            GameManager.GameMe = this;
            Graphics = new GraphicsDeviceManager(this);
            if (!IsPlayabilityCheckerOnly)
            //if (false)
            {
                Graphics.PreferredBackBufferWidth = 900;
                Graphics.PreferredBackBufferHeight = 550;
                Graphics.PreferMultiSampling = true;
                try
                {
                    Graphics.ApplyChanges();
                }
                catch (Exception)
                {}
            }

            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
        }

        protected override void Initialize()
        {
            GameManager.InitializeMe();
            base.Initialize();
        }
        
        public void SetOpacity(double op)
        {
            try
            {
                Form myGameForm = (Form)Form.FromHandle(Window.Handle);
                myGameForm.Opacity = op;
            }
            catch (Exception)
            {
            }
        }

        protected override void LoadContent()
        { }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (ShouldUpdate)
            {
                ActionsExecuterGenSim.ManipulateActions(gameTime);
                StaticData.EngineManager.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            StaticData.EngineManager.Draw(gameTime);
            base.Draw(gameTime);
            if (IsDrawJustOneTime)
            {
                this.Exit();
            }
        }

        public void DrawMe()
        {
            Initialize();
            Draw(new GameTime());
        }
    }
}