using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.AuthoringTool;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
{
    [Serializable]
    public class RigidsManager : IUpdatableComponent
    {
        public List<BoxRigid> ListOfBoxRigids { set; get; }
        public List<SphereRigid> ListOfSphereRigids { set; get; }
        public List<RigidBody> ListOfRigids
        {
            get
            {
                List<RigidBody> listToReturn = new List<RigidBody>();
                listToReturn.AddRange(ListOfBoxRigids);
                listToReturn.AddRange(ListOfSphereRigids);
                return listToReturn;
            }
        }

        public RigidsManager()
        {
            ListOfBoxRigids = new List<BoxRigid>();
            ListOfSphereRigids = new List<SphereRigid>();

        }

        public void Update(GameTime gameTime)
        {
            UpdateRigids(gameTime);
        }

        private void UpdateRigids(GameTime gameTime)
        {
            List<RigidBody> rigidsToDelete = new List<RigidBody>();
            foreach (BoxRigid boxRigid in ListOfBoxRigids)
            {
                if (!ShouldDie(boxRigid))
                {
                    boxRigid.Update(gameTime);
                }
                else
                {
                    rigidsToDelete.Add(boxRigid);
                }
            }

            foreach (SphereRigid circle in ListOfSphereRigids)
            {
                if (!ShouldDie(circle))
                {
                    circle.Update(gameTime);
                }
                else
                {
                    rigidsToDelete.Add(circle);
                }
            }

            foreach (RigidBody rigidBody in rigidsToDelete)
            {
                if (rigidBody is SphereRigid)
                {
                    this.ListOfSphereRigids.Remove((SphereRigid)rigidBody);
                }
                else
                {
                    this.ListOfBoxRigids.Remove((BoxRigid) rigidBody);
                }
            }
        }

        public static bool ShouldDie(RigidBody rigid)
        {
            if (StaticData.GameSessionMode == SessionMode.PlayingMode)
            {
                if (rigid.PositionXNA.X > 1300 || rigid.PositionXNA.X < -200
                    || rigid.PositionXNA.Y > 1000 || rigid.PositionXNA.Y < -200)
                    return true;
            }
            return false;
        }

        public void Draw(GameTime gameTime)
        {
            foreach (BoxRigid boxRigid in ListOfBoxRigids)
            {
                boxRigid.Draw(gameTime);
            }

            foreach (SphereRigid circle in ListOfSphereRigids)
            {
                circle.Draw(gameTime);
            }
        }

        public void AddRigidBody(RigidBody rigidBody)
        {
            if (rigidBody is BoxRigid)
            {
                this.ListOfBoxRigids.Add((BoxRigid)rigidBody);
            }
            else
            {
                if (rigidBody is SphereRigid)
                {
                    this.ListOfSphereRigids.Add((SphereRigid)rigidBody);
                }
            }
        }

        public void DeleteRigid(RigidBody rigidToDelete)
        {
            //try
            //{
                //if (!TryDeleteRope(rigidToDelete))
                //{
                    //TryDeleteCatchableRope();
            if (rigidToDelete != StaticData.EngineManager.CookieRB)
            {
                if (rigidToDelete != StaticData.EngineManager.FrogRB)
                {
                    if (DesignerManager.BasicRope != null)
                    {
                        if (rigidToDelete != DesignerManager.BasicRope.Masses[0])
                        {
                            StaticData.EngineManager.SpringsManagerEngine.TryDeleteFromRopes(rigidToDelete);
                            //StaticData.EngineManager.SpringsManagerEngine.TryDeleteFromCatchableRopes(rigidToDelete
                            if (rigidToDelete is BoxRigid)
                                ListOfBoxRigids.Remove((BoxRigid) rigidToDelete);
                            if (rigidToDelete is SphereRigid)
                                ListOfSphereRigids.Remove((SphereRigid) rigidToDelete);
                        }
                    }
                    else
                    {
                        StaticData.EngineManager.SpringsManagerEngine.TryDeleteFromRopes(rigidToDelete);
                        //StaticData.EngineManager.SpringsManagerEngine.TryDeleteFromCatchableRopes(rigidToDelete
                        if (rigidToDelete is BoxRigid)
                            ListOfBoxRigids.Remove((BoxRigid)rigidToDelete);
                        if (rigidToDelete is SphereRigid)
                            ListOfSphereRigids.Remove((SphereRigid)rigidToDelete);
                    }
                }
            }
            //}
            //}
            //catch (Exception)
            //{

            //}
        }
    }
}
