using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs
{
    [Serializable]
    public class SpringsManager : ServiceManager, IUpdatableComponent
    {
        public int NextRopeId = -1;
        public List<SpringService> ListOfServices { set; get; }

        public SpringsManager()
        {
            ListOfServices = new List<SpringService>();
        }

        public void Update(GameTime gameTime)
        {
            UpdateServices(gameTime);
            //if (StaticData.ManipulationGameMode == ManipulationGameMode.MoveRope)
            //{
            //    UpdateCurrentMovableRope();    
            //}
        }

        private void UpdateServices(GameTime gameTime)
        {
            for (int index = 0; index < ListOfServices.Count; index++)
            {
                var SpringService = ListOfServices[index];
                if (SpringService.Masses != null)
                {
                    if (SpringService.Masses.Count == 0)
                    {
                        ListOfServices.RemoveAt(index);
                        index = index - 1;
                    }
                    else
                    {
                        //for (int i = 0; i < 4; i++)
                        //{
                        SpringService.Update(gameTime);
                        //}
                    }
                }
            }
        }

        //public void UpdateCurrentMovableRope()
        //{
        //    if (StaticData.CurrentRopeIdToMove != -1)
        //    {
        //        SpringService currentMovableRope = this.ListOfServices[StaticData.CurrentRopeIdToMove];

        //        var kS = Keyboard.GetState();
        //        var ropeConnectionVel = new Vector3();
        //        if (kS.IsKeyDown(Keys.Right))									
        //            ropeConnectionVel.X += 150;                                

        //        if (kS.IsKeyDown(Keys.Left))							
        //            ropeConnectionVel.X -= 150;						   

        //        if (kS.IsKeyDown(Keys.Up))								
        //            ropeConnectionVel.Y -= 150;                         

        //        if (kS.IsKeyDown(Keys.Down))							
        //            ropeConnectionVel.Y += 150;							

        //        if (kS.IsKeyDown(Keys.Home))							
        //            ropeConnectionVel.Y += 3.0f;						

        //        if (kS.IsKeyDown(Keys.End))							
        //            ropeConnectionVel.Y -= 3.0f;

        //        currentMovableRope.SetConnectionVel(ropeConnectionVel);			
        //    }
        //}

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < ListOfServices.Count; i++)
            {
                //var SpringService = ListOfServices[i];
                ListOfServices[i].Draw(gameTime);
            }
        }

        public void AddNewService(SpringService SpringServiceIn)
        {
            ListOfServices.Add(SpringServiceIn);
        }

        public void RemoveService(SpringService SpringService)
        {
            SpringService.SetMassesNewState(true);
            this.ListOfServices.Remove(SpringService);
        }

        public SpringService RemoveService(int ropeId)
        {
            SpringService rope = GetRopeWithId(ropeId);
            if (rope != null)
            {
                rope.SetMassesNewState(true);
                ListOfServices.Remove(rope);
            }
            return rope;
        }

        private SpringService GetRopeWithId(int ropeId)
        {
            for (int i = 0; i < this.ListOfServices.Count; i++)
            {
                if (this.ListOfServices[i].Id == ropeId)
                {
                    return this.ListOfServices[i];
                }
            }
            return null;
        }

        public void TryDeleteFromRopes(RigidBody rigidToDelete)
        {
            try
            {
                for (int i = 0; i < ListOfServices.Count; i++)
                {
                    if (ListOfServices[i].Masses.Contains(rigidToDelete))
                    {
                        int indexOfRigidToDelete = ListOfServices[i].Masses.IndexOf(rigidToDelete);
                        if (indexOfRigidToDelete == 0)
                        {
                            RemoveService(ListOfServices[i].Id);
                        }
                        else
                        {
                            if (indexOfRigidToDelete + 1 < ListOfServices[i].Masses.Count)
                                ListOfServices[i].Springs[indexOfRigidToDelete - 1].Mass2 = ListOfServices[i].Masses[indexOfRigidToDelete + 1];

                            if (indexOfRigidToDelete < ListOfServices[i].Springs.Count)
                            {
                                ListOfServices[i].Springs.RemoveAt(indexOfRigidToDelete);
                            }

                            if (indexOfRigidToDelete < ListOfServices[i].Masses.Count)
                                ListOfServices[i].Masses.RemoveAt(indexOfRigidToDelete);
                            // No "BREAK" Keyword, coz the Rigid may appear in two ropes when connecting them to each other.
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            if (service is SpringService)
                this.AddNewService(service as SpringService);
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            if (service is SpringService)
                this.RemoveService(service as SpringService);
        }

        public int GetNextRopeId()
        {
            NextRopeId++;
            return NextRopeId;
        }
    }
}
