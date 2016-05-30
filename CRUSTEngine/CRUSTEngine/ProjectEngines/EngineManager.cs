using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Water;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.MouseManagers;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rods;
using CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.TileSpring;

namespace CRUSTEngine.ProjectEngines
{
    [Serializable]
    public class EngineManager
    {
        //private FrogRB _frog;
        public FrogRB FrogRB { set; get; }

        //private SphereRigid _cookieRB;
        public CookieRB CookieRB { set; get; }

        #region --- Fields ---

        //[NonSerialized]
        //public GraphicsDevice GraphicsDevice { set; get; }
        //public GameWindow Window { set; get; }
        
        //[NonSerialized]
        //public ContentManager Content { set; get; }


        [NonSerialized]
        public Game1 Game1;

        //[NonSerialized]
        //public SpriteBatch SpriteBatch;
        //public CatchableRopeManager CatchableRopeManagerEngine;
        public RigidsManager RigidsManagerEngine;
        public CollisionManager CollisionManagerEngine;
        public PlanesManager PlanesManagerEngine;
        public MouseAdder MouseRigidsAdderEngine;
        public MouseManager MouseManagerEngine;
        public MouseResizeManager ResizeManagerEngine;
        public ColorsProvider ColorsProviderEngine;
        public SpringsManager SpringsManagerEngine;
        public TileSpringServiceManager TileSpringServiceManagerEngine;
        //public PendulumManager PendulumManagerEngine;
        public LiquidService LiquidServiceEngine;
        public BasicBackGround BasicBackGroundEngine;
        public RocketsManager RocketsManagerEngine;
        public BubbleManager BubbleManagerEngine;
        public BlowerManager BlowerManagerEngine;
        public RodsManager RodsManagerEngine;
        public RopeOfRodsManager RopeOfRodsManagerEngine;
        public RocketsCarrierManager RocketsCarrierManagerEngine;
        public NotificationManager NotificationManagerEngine;
        public PreferredCompsManager PrefCompsManager;
        public EntraManager EntraManager;
        public EntraPathManager EntraPathManager;
        #endregion

        public EngineManager(Game1 game)
        {
            this.Game1 = game;
            StaticData.LoadDataContent(this);
            Initialize();
            
        }

        public void Initialize()
        {
            if (RyseAgent.WithWalls)
                PlanesManagerEngine = new PlanesManager();
            StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;
            RigidsManagerEngine = new RigidsManager();
            CollisionManagerEngine = new CollisionManager();
            MouseRigidsAdderEngine = new MouseAdder();
            ResizeManagerEngine = new MouseResizeManager();
            SpringsManagerEngine = new SpringsManager();
            MouseManagerEngine = new MouseManager();
            BubbleManagerEngine = new BubbleManager();
            BasicBackGroundEngine = new BasicBackGround();
            RocketsManagerEngine = new RocketsManager();
            BlowerManagerEngine = new BlowerManager();
            LiquidServiceEngine = new LiquidService();
            //CatchableRopeManagerEngine = new CatchableRopeManager();
            RodsManagerEngine = new RodsManager();
            RopeOfRodsManagerEngine = new RopeOfRodsManager();
            RocketsCarrierManagerEngine = new RocketsCarrierManager();
            TileSpringServiceManagerEngine = new TileSpringServiceManager();
            NotificationManagerEngine = new NotificationManager();
            PrefCompsManager = new PreferredCompsManager();
            EntraManager = new EntraManager();
            EntraPathManager = new EntraPathManager();
            //TestMe();
        }

        public void TestMe()
        {
            //this.RigidsManagerEngine.AddRigidBody(this.CookieRB);

            //SpringsManagerEngine.AddNewService( DefaultAdder.GetDefaultSpringRope(new Vector3(100, 10, 0), 20, 10000, 0.05f, 
            //    0.2f, RigidType.SphereRigid, new Vector3(10), false, SpringType.StrictRope));

            //SpringsManagerEngine.AddNewService(DefaultAdder.GetDefaultSpringRope(new Vector3(100, 10, 0), 20, 10000, 0.05f,
            //    0.2f, RigidType.BoxRigid, new Vector3(10), false, SpringType.StrictRope));

            //BlowerManagerEngine.AddNewService(new BlowerService(new Vector3(50, 100, 0), Dir.East));



            //BoxRigid b1 = DefaultAdder.GetDefaultBox(new Vector3(10, 10, 10), Material.Glass, new Vector3(10, 20, 0),
            //                                        new Vector3(0, -9.8f, 0), null, null, 0, false);

            //BoxRigid b2 = DefaultAdder.GetDefaultBox(new Vector3(100, 10, 10), Material.Glass, new Vector3(10, 20, 0),
            //                                        new Vector3(0, -9.8f, 0), null, null, 0, false);
            //this.RigidsManagerEngine.AddRigidBody(b1);
            //this.RigidsManagerEngine.AddRigidBody(b2);
            //Rod r1 = new Rod(b1, b2, null);
            //RodsManagerEngine.ListOfRods.Add(r1);

            //Rod r2 = new Rod(new Vector3(10, 10 ,10), new Vector3(0, -9.8f, 0), 90, 5);
            //this.RigidsManagerEngine.AddRigidBody(r2.RodRigidBody);
            //this.RigidsManagerEngine.AddRigidBody(r2.RigidOne);
            //this.RigidsManagerEngine.AddRigidBody(r2.RigidTwo);
            //RodsManagerEngine.ListOfRods.Add(r2);
        }

        public void Update(GameTime gameTime)
        {
            //this.Game1.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 200);
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.N))
            {
                StaticData.ManipulationGameMode = ManipulationGameMode.NeutralMode;
            }
            if (ks.IsKeyDown(Keys.P))
            {
                StaticData.CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
            }
            if (ks.IsKeyDown(Keys.Enter))
            {
                StaticData.CurrentPausePlayGameMode = PlayPauseMode.PlayOnMode;
            }
            NotificationManagerEngine.Update(gameTime);
            MouseRigidsAdderEngine.Update(gameTime);
            MouseManagerEngine.Update(gameTime);

            if (StaticData.CurrentPausePlayGameMode == PlayPauseMode.PlayOnMode)
            {
                CollisionManagerEngine.Update(gameTime);
                if (RyseAgent.WithWalls)
                    PlanesManagerEngine.Update(gameTime);
                //CatchableRopeManagerEngine.Update(gameTime);

                
                RigidsManagerEngine.Update(gameTime);

                BubbleManagerEngine.Update(gameTime);
                RocketsManagerEngine.Update(gameTime);
                TileSpringServiceManagerEngine.Update(gameTime);
                SpringsManagerEngine.Update(gameTime);
                RodsManagerEngine.Update(gameTime);
                RopeOfRodsManagerEngine.Update(gameTime);
                RocketsCarrierManagerEngine.Update(gameTime);
                PrefCompsManager.Update(gameTime);

                if (StaticData.IsEntraActivated)
                {
                    EntraManager.Update(gameTime);
                }
                if (StaticData.IsEntraPathActivated)
                {
                    EntraPathManager.Update(gameTime);
                }

                BlowerManagerEngine.Update(gameTime);
                if (StaticData.IsWater)
                    LiquidServiceEngine.Update(gameTime);
            }
            if (StaticData.ManipulationGameMode == ManipulationGameMode.ResizeRigidMode)
            {
                ResizeManagerEngine.Update(gameTime);
            }
            StaticData.UpdatesSoFar++;
        }

        public void Draw(GameTime gameTime)
        {
            BasicBackGroundEngine.Draw(gameTime);
            //CatchableRopeManagerEngine.Draw(gameTime);
            if (RyseAgent.WithWalls)
                PlanesManagerEngine.Draw(gameTime);

            BlowerManagerEngine.Draw(gameTime);
            BubbleManagerEngine.Draw(gameTime);
            RocketsCarrierManagerEngine.Draw(gameTime);
            RocketsManagerEngine.Draw(gameTime);
            RopeOfRodsManagerEngine.Draw(gameTime);
            SpringsManagerEngine.Draw(gameTime);
            TileSpringServiceManagerEngine.Draw(gameTime);
            RigidsManagerEngine.Draw(gameTime);
            NotificationManagerEngine.Draw(gameTime);
            PrefCompsManager.Draw(gameTime);

            if (StaticData.IsEntraActivated)
            {
                EntraManager.Draw(gameTime);
            }

            if (StaticData.IsEntraPathActivated)
            {
                EntraPathManager.Draw(gameTime);
            }

            if (StaticData.IsWater)
                LiquidServiceEngine.Draw(gameTime);
            if (StaticData.ManipulationGameMode == ManipulationGameMode.ResizeRigidMode)
            {
                ResizeManagerEngine.Draw(gameTime);
            }

            if (ActionsExecuterGenSim.ActionsNotifManager != null &&
                ActionsExecuterGenSim.ActionsNotifManager.Notifications.Count() != 0)
            {
                ActionsExecuterGenSim.ActionsNotifManager.Draw(gameTime);
            }
        }
    }
}