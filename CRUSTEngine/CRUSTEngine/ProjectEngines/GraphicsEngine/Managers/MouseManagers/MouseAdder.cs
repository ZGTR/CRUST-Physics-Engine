using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.MouseManagers
{
    [Serializable]
    public class MouseAdder : IUpdatableComponent
    {
        private BoxRigid _boxRigidAdded = null;
        private SphereRigid _sphereRigidAdded = null;
        private BubbleService _bubble = null;
        private RocketCarrierService _rocket = null;
        private SpringService _rope = null;
        private CatchableRopeService _catchableRope = null;
        private BlowerService _blower = null;
        private BumpRigid _bump = null;

        public List<RigidBody> CurrentJointsRigids;
        [NonSerialized]
        private MouseState mouseState;
        
        public MouseAdder()
        {
            CurrentJointsRigids = new List<RigidBody>();
        }

        public void Update(GameTime gameTime)
        {
            if (StaticData.GameSessionMode == SessionMode.DesignMode)
            {
                mouseState = Mouse.GetState();
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                if (mousePosition.X <= 0 || mousePosition.X >= 900 || mousePosition.Y >= 550 || mousePosition.Y <= 0)
                    return;
                switch (StaticData.ManipulationGameMode)
                {
                    case ManipulationGameMode.AddDefaultRectangleMode:
                        ManipulateAddingBoxes();
                        break;
                    case ManipulationGameMode.AddDefaultCircleMode:
                        ManipulateAddingSpheres();
                        break;
                    case ManipulationGameMode.AddBubblesMode:
                        ManipulateAddingBubbles();
                        break;
                    case ManipulationGameMode.AddRocketsMode:
                        ManipulateAddingRockets();
                        break;
                    case ManipulationGameMode.AddRopesMode:
                        ManipulateAddingRopes();
                        break;
                    case ManipulationGameMode.AddCatchableRopesMode:
                        ManipulateAddingCatchableRopes();
                        break;
                    case ManipulationGameMode.AddBlowersMode:
                        ManipulateAddingBlowers();
                        break;
                    case ManipulationGameMode.AddBumpsMode:
                        ManipulateAddingBumps();
                        break;

                    case ManipulationGameMode.SetJointsHardConstraints:
                        ManipulateAddingSpheres();
                        if (_sphereRigidAdded != null)
                        {
                            CurrentJointsRigids.Add(_sphereRigidAdded);
                        }
                        break;
                }
            }
        }

        private void ManipulateAddingBumps()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_bump == null)
                {
                    _bump = AddBumpsToEngine();
                }
            }
            else
            {
                if (_bump != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _bump = null;
                }
            }
        }

        private void ManipulateAddingBlowers()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_blower == null)
                {
                    _blower = AddBlowersToEngine();
                }
            }
            else
            {
                if (_blower != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _blower = null;
                }
            }
        }

        private void ManipulateAddingCatchableRopes()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_catchableRope == null)
                {
                    _catchableRope = AddCatchableRopesToEngine();
                }
            }
            else
            {
                if (_catchableRope != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _catchableRope = null;
                }
            }
        }

        private void ManipulateAddingRopes()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_rope == null)
                {
                    _rope = AddRopesToEngine();
                }
            }
            else
            {
                if (_rope != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _rope = null;
                }
            }
        }

        private void ManipulateAddingRockets()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_rocket == null)
                {
                    _rocket = AddRocketToEngine();
                }
            }
            else
            {
                if (_rocket != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _rocket = null;
                }
            }
        }

        private void ManipulateAddingBubbles()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_bubble == null)
                {
                    _bubble = AddBubbleToEngine();
                }
            }
            else
            {
                if (_bubble != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _bubble = null;
                }
            }
        }

        private void ManipulateAddingBoxes()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_boxRigidAdded == null)
                {
                    _boxRigidAdded = AddBoxRigidToEngine();
                }
            }
            else
            {
                if (_boxRigidAdded != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _boxRigidAdded = null;
                }
            }
        }

        private void ManipulateAddingSpheres()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_sphereRigidAdded == null)
                {
                    _sphereRigidAdded = AddSphereRigidToEngine();
                }
            }
            else
            {
                if (_sphereRigidAdded != null && mouseState.LeftButton == ButtonState.Released)
                {
                    _sphereRigidAdded = null;
                }
            }
        }

        public BoxRigid AddBoxRigidToEngine()
        {
            BoxRigid boxRigidCreated = DefaultAdder.GetDefaultBox(
                new Vector3(mouseState.X - 3, (mouseState.Y - 3), 0),
                Material.Steel,
                StaticData.BoxDefaultSize,
                null,
                null,
                null,
                0,
                false);
            StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids.Add(boxRigidCreated);
            return boxRigidCreated;
        }

        public SphereRigid AddSphereRigidToEngine()
        {
            SphereRigid SphereRigidCreated = DefaultAdder.GetDefaultSphere(
                new Vector3(mouseState.X - 3, (mouseState.Y - 3), 0),
                Material.Steel,
                StaticData.CircleDefaultSize,
                new Vector3(0, -9.8f, 0), 
                null,
                null,
                0,
                false);
            StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids.Add(SphereRigidCreated);
            return SphereRigidCreated;
        }

        private BubbleService AddBubbleToEngine()
        {
            BubbleService bubbleService = DefaultAdder.GetDefaultBubble(new Vector2(mouseState.X, mouseState.Y));
            StaticData.EngineManager.BubbleManagerEngine.AddNewService(bubbleService);
            return bubbleService;
        }

        private RocketCarrierService AddRocketToEngine()
        {
            RocketCarrierService rocket = new RocketCarrierService(new Vector3(mouseState.X, mouseState.Y, 0),
                                                                   Direction.East);

            StaticData.EngineManager.RocketsCarrierManagerEngine.AddNewService(rocket);
            return rocket;
        }

        private SpringService AddRopesToEngine()
        {
            int nrOfMasses = (100 / 15) - 1;
            if (nrOfMasses <= 0)
            {
                nrOfMasses = 2;
            }
            SpringService springService = DefaultAdder.GetDefaultSpringRope(new Vector3(mouseState.X, mouseState.Y, 0),
                StaticData.EngineManager.CookieRB.PositionXNA,
                                                                            nrOfMasses, 20*nrOfMasses,
                                                                            10f, 0.1f,
                                                                            RigidType.SphereRigid,
                                                                            new Vector3(5, 5, 0), false,
                                                                            SpringType.StrictRope);
            springService.ApplyServiceOnRigid(StaticData.EngineManager.CookieRB);
            StaticData.EngineManager.SpringsManagerEngine.AddNewService(springService);
            return springService;
        }

        private CatchableRopeService AddCatchableRopesToEngine()
        {
            int nrOfMasses = (70 / 15) - 1;
            if (nrOfMasses <= 0)
            {
                nrOfMasses = 2;
            }
            CatchableRopeService rope = DefaultAdder.GetDefaultCatchableRope(new Vector3(mouseState.X, mouseState.Y, 0),
                StaticData.EngineManager.CookieRB.PositionXNA,
                                                                          nrOfMasses, 80 * nrOfMasses,
                                                                          10f, 0.1f,
                                                                          RigidType.SphereRigid,
                                                                          new Vector3(5, 5, 0), false,
                                                                          SpringType.StrictRope);
            StaticData.EngineManager.SpringsManagerEngine.AddNewService(rope);
            return rope;
        }

        private BumpRigid AddBumpsToEngine()
        {
            Direction bDir = Direction.East;
            BumpRigid boxRigid = new BumpRigid(new Vector3(mouseState.X, mouseState.Y, 0), Material.Wood,
                                              new Vector3(30, 15, 0),
                                              bDir);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(boxRigid);
            return boxRigid;
        }

        private BlowerService AddBlowersToEngine()
        {
            Direction bDir = Direction.East;
            BlowerService blowerService = new BlowerService(new Vector3(mouseState.X, mouseState.Y, 0), bDir);
            StaticData.EngineManager.BlowerManagerEngine.AddNewService(blowerService);
            return blowerService;
        }

        public void Draw(GameTime gameTime)
        {

        }
        
    }
}
