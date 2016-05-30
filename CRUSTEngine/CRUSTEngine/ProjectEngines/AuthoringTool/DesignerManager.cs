using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.AuthoringTool
{
    public class DesignerManager
    {
        public static SpringService BasicRope;
        public static Game1 Game;
        public DesignerManager()
        {
            StaticData.ManipulationGameMode = ManipulationGameMode.DragRigidMode;
            StaticData.GameSessionMode = SessionMode.DesignMode;
            InitializeEngineForDesignerWithComponents();
        }

        public static void InitializeEngineForDesignerWithComponents()
        {
            if (Game == null)
            {
                using (Game1 game1 = new Game1())
                {
                    StaticData.EngineManager = new EngineManager(game1);
                    StaticData.InitializeEngine(game1);
                }
            }
            else
            {
                StaticData.EngineManager = new EngineManager(Game);
                StaticData.InitializeEngine(Game);
            }
            InitializeBasicComponents();
        }

        private static void InitializeBasicComponents()
        {
            InitializeCookie();
            InitializeFrog();
            InitializeBasicRope();
        }

        private static void InitializeCookie()
        {
            StaticData.EngineManager.CookieRB = new CookieRB(new Vector3(450, 150, 0), Material.Glass, 10);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(StaticData.EngineManager.CookieRB);
        }

        private static void InitializeFrog()
        {
            StaticData.EngineManager.FrogRB = new FrogRB(new Vector3(435, 400, 0), Material.Glass,
                                                           StaticData.FrogHalfSize);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(StaticData.EngineManager.FrogRB);
        }

        private static void InitializeBasicRope()
        {
            int nrOfMasses = (100 / 15) - 1;
            if (nrOfMasses <= 0)
            {
                nrOfMasses = 2;
            }
            BasicRope = DefaultAdder.GetDefaultSpringRope(new Vector3(450, 40, 0),
                StaticData.EngineManager.CookieRB.PositionXNA,
                                                                            nrOfMasses, 
                                                                            20 * nrOfMasses,
                                                                            10f,
                                                                            0.1f,
                                                                            RigidType.SphereRigid,
                                                                            new Vector3(5, 5, 0), false,
                                                                            SpringType.StrictRope);
            //BasicRope = DefaultAdder.GetDefaultSpringRope(new Vector3(450, 40, 0),
            //                                                    nrOfMasses,
            //                                                    500,
            //                                                    5f,
            //                                                    0.1f,
            //                                                    RigidType.SphereRigid,
            //                                                    new Vector3(10, 10, 0), false,
            //                                                    SpringType.StrictRope);
            BasicRope.ApplyServiceOnRigid(StaticData.EngineManager.CookieRB);
            StaticData.EngineManager.SpringsManagerEngine.AddNewService(BasicRope);
        }

        public void Run()
        {
            using (Game = new Game1())
            {
                Game1.IsUserDesigner = true;
                Game.Run();
            }
        }
    }
}
