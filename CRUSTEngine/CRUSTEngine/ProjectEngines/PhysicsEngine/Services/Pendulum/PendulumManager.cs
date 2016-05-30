//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
//using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
//using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
//using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Pendulum;

//namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
//{
//    [Serializable]
//    public class PendulumManager : IUpdatableComponent
//    {
//        public List<PendulumService> PendulumServiceList { set; get; }

//        public PendulumManager()
//        {
//            PendulumServiceList = new List<PendulumService>();
//            //Vector3 initialPosition = new Vector3(10);
//            //BoxRigid box1 = DefaultAdder.GetDefaultBox(initialPosition,
//            //                                                    Material.Rubber, new Vector3(10, 10, 0), null, null,
//            //                                                    null, 0,
//            //                                                    false);
//            //BoxRigid box2 = DefaultAdder.GetDefaultBox(initialPosition + new Vector3(0,40,0),
//            //                                                    Material.Rubber, new Vector3(10, 10, 0), null, null,
//            //                                                    null, 0,
//            //                                                    false);
//            ////BoxRigid box3 = DefaultAdder.GetDefaultBox(initialPosition,
//            ////                                                    Material.Rubber, new Vector3(10, 10, 0), null, null,
//            ////                                                    null, 0,
//            ////                                                    false);
//            ////BoxRigid box4 = DefaultAdder.GetDefaultBox(initialPosition,
//            ////                                                    Material.Rubber, new Vector3(10, 10, 0), null, null,
//            ////                                                    null, 0,
//            ////                                                    false);
//            //List<RigidBody> list = new List<RigidBody>();
//            //list.Add(box1);
//            //list.Add(box2);
//            ////list.Add(box3);
//            ////list.Add(box4);
//            //StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(box1);
//            //StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(box2);
//            ////StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(box3);
//            ////StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(box4);
//            //PendulumService pService = new PendulumService(20, list);
//            //PendulumServiceList.Add(pService);
//        }

//        public void AddNewPendulum(PendulumService pendulumServiceIn)
//        {
//            PendulumServiceList.Add(pendulumServiceIn);
//        }

//        public void Update(GameTime gameTime)
//        {
//            for (int index = 0; index < PendulumServiceList.Count; index++)
//            {
//                var PendulumService = PendulumServiceList[index];
//                if (PendulumService.Masses.Count == 0)
//                {
//                    PendulumServiceList.RemoveAt(index);
//                    index = index - 1;
//                }
//                else
//                {
//                    PendulumService.Update(gameTime);
//                }
//            }
//            if (StaticData.ManipulationGameMode == ManipulationGameMode.MovePendulum)
//            {
//                UpdateCurrentMovablePendulum();
//            }
//        }

//        public void UpdateCurrentMovablePendulum()
//        {
//            try
//            {
//                PendulumService currentMovablePendulum = this.PendulumServiceList[StaticData.CurrentPendulumIdToMove];

//                var kS = Keyboard.GetState();
//                var PendulumConnectionVel = new Vector3();
//                if (kS.IsKeyDown(Keys.Right))
//                    PendulumConnectionVel.X += 150;

//                if (kS.IsKeyDown(Keys.Left))
//                    PendulumConnectionVel.X -= 150;

//                if (kS.IsKeyDown(Keys.Up))
//                    PendulumConnectionVel.Y -= 150;

//                if (kS.IsKeyDown(Keys.Down))
//                    PendulumConnectionVel.Y += 150;

//                if (kS.IsKeyDown(Keys.Home))
//                    PendulumConnectionVel.Y += 3.0f;

//                if (kS.IsKeyDown(Keys.End))
//                    PendulumConnectionVel.Y -= 3.0f;

//                currentMovablePendulum.SetConnectionVel(PendulumConnectionVel);
//            }
//            catch (Exception)
//            {
//            }
//        }

//        public void Draw(GameTime gameTime)
//        {
//            // No need, Rigids drawn on their own
//        }

//        public void DeletePendulum(int PendulumId)
//        {
//            try
//            {
//                PendulumServiceList.RemoveAt(PendulumId);
//            }
//            catch
//            {
//            }
//        }

//        public void TryDeleteFromPendulums(RigidBody rigidToDelete)
//        {
//            for (int i = 0; i < PendulumServiceList.Count; i++)
//            {
//                if (PendulumServiceList[i].Masses.Contains(rigidToDelete))
//                {

//                    int indexOfRigidToDelete = PendulumServiceList[i].Masses.IndexOf(rigidToDelete);
//                    if (indexOfRigidToDelete == 0)
//                    {
//                        DeletePendulum(i);
//                    }
//                    else
//                    {
//                        if (indexOfRigidToDelete + 1 < PendulumServiceList[i].Masses.Count)
//                            PendulumServiceList[i].PendulumLinks[indexOfRigidToDelete - 1].Mass2 =
//                                PendulumServiceList[i].Masses[indexOfRigidToDelete + 1];

//                        if (indexOfRigidToDelete < PendulumServiceList[i].PendulumLinks.Count)
//                            PendulumServiceList[i].PendulumLinks.RemoveAt(indexOfRigidToDelete);

//                        if (indexOfRigidToDelete < PendulumServiceList[i].Masses.Count)
//                            PendulumServiceList[i].Masses.RemoveAt(indexOfRigidToDelete);
//                        // No "BREAK" Keyword, coz the Rigid may appear in two Pendulums when connecting them to each other.
//                    }
//                }
//            }
//        }
//    }
//}
