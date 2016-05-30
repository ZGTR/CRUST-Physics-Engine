using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.PCGEngine;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rods;
using CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
{
    [Serializable]
    public class CollisionManager : IUpdatableComponent
    {
        public List<NonCollidableRigidsPair> ListOfNonCollidableRigidsPair;
        private ContactResolver _contactResolver;
        private CollisionData _data;

        public CollisionManager()
        {
            ListOfNonCollidableRigidsPair = new List<NonCollidableRigidsPair>();
            _contactResolver = new ContactResolver();
            _data.contactCount = 0;
            _data.contacts = new List<Contact>();
            _data.contactsLeft = 1000;
            _data.friction = .9f;
            _data.index = 0;
            _data.restitution = .7f;
        }

        public void Update(GameTime gameTime)
        {
            //try
            //{
                UpdateCollisionDetecting(gameTime);
            //}
            //catch (Exception)
            //{
            //}
            try
            {
                UpdateCollisionResloving(gameTime);
            }
            catch (Exception)
            {
            }
        }

        private void UpdateCollisionDetecting(GameTime gameTime)
        {
            RigidsCD();
            RodsSingleCD(StaticData.EngineManager.RodsManagerEngine.ListOfRods);
            RopeOfRodsCD();
        }

        private void RopeOfRodsCD()
        {
            var ropes =  StaticData.EngineManager.RopeOfRodsManagerEngine.ListOfRopeOfRods;
            foreach (RopeOfRods rope in ropes)
            {
                RodsSingleCD(rope.ListOfRods);
            }
        }

        private void RigidsCD()
        {
            SpheresCD();
            BoxesCD();
        }

        private void SpheresCD()
        {
            List<BoxRigid> ListOfBoxRigids = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids;
            List<SphereRigid> ListOfSphereRigids = StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids;
            List<CollisionPlane> ListOfPlanes = null;
            if (RyseAgent.WithWalls)
                ListOfPlanes = StaticData.EngineManager.PlanesManagerEngine.ListOfPlanes;

            for (int i = 0; i < ListOfSphereRigids.Count; i++)
            {
                // Cash the sphere
                SphereRigid sphere = ListOfSphereRigids[i];
                // sphereAndHalfSpace
                if (RyseAgent.WithWalls)
                {
                    foreach (var plane in ListOfPlanes)
                    {
                        bool isCollide = CollisionDetector.sphereAndHalfSpace(sphere, plane, ref _data);
                    }
                }
                if (sphere.IsCollidable)
                {
                    // sphereAndSphere
                    for (int j = 0; j < ListOfSphereRigids.Count; j++)
                    {
                        // Cash the sphere
                        SphereRigid sphere2 = ListOfSphereRigids[j];
                        if (sphere != sphere2)
                        {
                            if (sphere2.IsCollidable)
                            {
                                if (!IsPairOfNonCollidableRigids(sphere, sphere2))
                                {
                                    CollisionDetector.sphereAndSphere(sphere,
                                                                      sphere2, ref _data);
                                }
                            }
                        }
                    }
                    // boxAndSphere
                    for (int j = 0; j < ListOfBoxRigids.Count; j++)
                    {
                        // Cash the box
                        var box = ListOfBoxRigids[j];
                        if (box.IsCollidable)
                        {
                            if (!IsPairOfNonCollidableRigids(box, sphere))
                            {
                                bool collided = CollisionDetector.boxAndSphere(box, sphere, ref _data);
                                if (collided)
                                {
                                    if (box is BumpRigid)
                                    {
                                        ((BumpRigid) box).IsCollidedWithCookie = true;
                                        if (StaticData.RyseComponentsUsageHelper != null)
                                        {
                                            if (!StaticData.RyseComponentsUsageHelper.CollidedBumps.Contains(box))
                                            {
                                                StaticData.RyseComponentsUsageHelper.CollidedBumps.Add(box as BumpRigid);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BoxesCD()
        {
            List<BoxRigid> ListOfBoxRigids = StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids;
            List<CollisionPlane> ListOfPlanes = null;
            if (RyseAgent.WithWalls)
                ListOfPlanes = StaticData.EngineManager.PlanesManagerEngine.ListOfPlanes;
            for (int i = 0; i < ListOfBoxRigids.Count; i++)
            {
                // Cash the box
                BoxRigid box = ListOfBoxRigids[i];
                // boxAndHalfSpace
                if (RyseAgent.WithWalls)
                {
                    foreach (var plane in ListOfPlanes)
                    {
                        CollisionDetector.boxAndHalfSpace(box, plane, ref _data);
                    }
                }
                if (box.IsCollidable)
                {
                    // boxAndBox
                    for (int j = 0; j < ListOfBoxRigids.Count; j++)
                    {
                        BoxRigid box2 = ListOfBoxRigids[j];
                        if (box != box2)
                        {
                            if (box2.IsCollidable)
                            {
                                if (!IsPairOfNonCollidableRigids(box, box2))
                                {
                                    CollisionDetector.boxAndBox(box, box2, ref _data);
                                }
                            }
                        }
                    }
                }
            }
        }

        NonCollidableRigidsComparer comparer = new NonCollidableRigidsComparer();
        private bool IsPairOfNonCollidableRigids(RigidBody r1, RigidBody r2)
        {
            if (ListOfNonCollidableRigidsPair.Contains(new NonCollidableRigidsPair(r1, r2), comparer))
            {
                return true;
            }
            return false;
        }

        private void RodsSingleCD(List<Rod> listOfRods)
        {
            CollisionData dataRod = new CollisionData();
            dataRod.contacts = new List<Contact>();
            dataRod.contactCount = 0;
            dataRod.contactsLeft = 1000;
            dataRod.index = 0;
            dataRod.friction = .9f;
            dataRod.restitution = .7f;

            for (int i = 0; i < listOfRods.Count; i++)
            {
                //dataRod.contacts = new List<Contact>();
                //dataRod.contactCount = 0;
                //dataRod.contactsLeft = 1000;
                //dataRod.index = 0;
                //dataRod.friction = .9f;
                //dataRod.restitution = .7f;

                Rod rodCurrent = listOfRods[i];

                Vector3 position1 = rodCurrent.RigidOne.PositionCenterEngine;
                Vector3 position2 = rodCurrent.RigidTwo.PositionCenterEngine;

                rodCurrent.Joint1.Set(rodCurrent.RigidOne, position1, rodCurrent.RodRigidBody,
                                      rodCurrent.RodRigidBody.vertices[rodCurrent.VertexBoxOneIndex].Position);
                rodCurrent.Joint1.AddContact(ref dataRod);
                //_contactResolver.resolveContacts(dataRod.contacts, dataRod.contactCount, StaticData.Dtime);

                rodCurrent.Joint2.Set(rodCurrent.RigidTwo, position2, rodCurrent.RodRigidBody,
                                      rodCurrent.RodRigidBody.vertices[rodCurrent.VertexBoxTwoIndex].Position);
                rodCurrent.Joint2.AddContact(ref dataRod);
                
            }
            _contactResolver.ResolveContacts(dataRod.contacts, dataRod.contactCount, StaticData.Dtime);
            
        }

        private void UpdateCollisionResloving(GameTime gameTime)
        {
            _contactResolver.ResolveContacts(_data.contacts, _data.contactCount, StaticData.Dtime);
            _data.contacts = new List<Contact>();
            _data.contactCount = 0;
            _data.contactsLeft = 1000;
            _data.index = 0;
            _data.friction = .9f;
            _data.restitution = .7f;
        }

        public void Draw(GameTime gameTime)
        {
            
        }

        public void AddRigidWithNonCollidableRigids(RigidBody rigidBody, List<RigidBody> listOfNonCollidables)
        {
            for (int i = 0; i < listOfNonCollidables.Count; i++)
            {
                if (rigidBody != listOfNonCollidables[i])
                    AddRigidWithNonCollidableRigids(rigidBody, listOfNonCollidables[i]);
            }
        }

        public void AddRigidWithNonCollidableRigids(RigidBody rigidBody1, RigidBody rigidBody2)
        {
            if (rigidBody1 != rigidBody2)
                this.ListOfNonCollidableRigidsPair.Add(new NonCollidableRigidsPair(rigidBody1,
                                                                                   rigidBody2));
        }

        public void RemoveRigidWithNonCollidableRigids(RigidBody rigidBody, List<RigidBody> listOfMasses)
        {
            for (int i = 0; i < listOfMasses.Count; i++)
            {
                if (rigidBody != listOfMasses[i])
                {
                    //var p1ToRemove = this.ListOfNonCollidableRigidsPair.(new NonCollidableRigidsPair(rigidBody,
                    //                                                                               listOfMasses[i]));

                    ListOfNonCollidableRigidsPair.RemoveAll(delegate(NonCollidableRigidsPair pair)
                                                                {
                                                                    if (pair.RigidBody1 == rigidBody &&
                                                                        pair.RigidBody2 == listOfMasses[i]
                                                                        ||
                                                                        pair.RigidBody2 == rigidBody &&
                                                                        pair.RigidBody1 == listOfMasses[i])
                                                                        return true;
                                                                    return false;
                                                                });
                    //this.ListOfNonCollidableRigidsPair.RemoveAt(index);
                }
            }
        }
    }
}
