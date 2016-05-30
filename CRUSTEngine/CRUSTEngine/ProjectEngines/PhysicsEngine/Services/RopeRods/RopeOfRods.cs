using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rods;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods
{
    [Serializable]
    public class RopeOfRods
	{
        public List<Rod> ListOfRods = new List<Rod>();
        public List<RigidBody> ListOfMasses = new List<RigidBody>();
        public Vector3 PositionXNA;
        public bool IsClosed = true;
        public bool IsFixed = false;

        public RopeOfRods(Vector3 PositionXNA, bool isFixed, bool isClosed, int numberOfMasses, Vector3 gravityVector,
            int spacing, int rigidHalfSize, float rodLinkHeight, float forgivingFactor = 0.15f)
        {
            this.PositionXNA = PositionXNA;
            this.IsFixed = isFixed;
            this.IsClosed = isClosed;
            BuildRope(isClosed, numberOfMasses, PositionXNA, gravityVector, spacing, rigidHalfSize, rodLinkHeight,
                      forgivingFactor);

            //this.PositionXNA = new Vector3(100, 100, 0);
            //BuildRope(3, PositionXNA, new Vector3(0, -9.8f, 0), 100, 10, rodLinkHeight, forgivingFactor, IsClosed);
        }

        public void BuildRope(bool isClosed, int numberOfMasses, Vector3 positionXNA, Vector3 gravityVector,
            int spacing, int rigidHalfSize, float rodLinkHeight, float forgivingFactor)
        {
            RigidBody r1 = DefaultAdder.GetDefaultSphere(positionXNA, Material.Wood,
                                                                    rigidHalfSize, gravityVector, null,
                                                                    null, 0, false);
            ListOfMasses.Add(r1);

            Rod rodProperty = null;
            for (int i = 1; i < numberOfMasses; i++)
            {
                r1 = ListOfMasses[ListOfMasses.Count - 1];
                RigidBody r2 = DefaultAdder.GetDefaultSphere(positionXNA + new Vector3(spacing, 0, 0)
                                                             , Material.Wood,
                                                             rigidHalfSize, gravityVector, null,
                                                             null, 0, false);
                rodProperty = new Rod(r1, r2, spacing, rodLinkHeight, forgivingFactor);
                ListOfMasses.Add(rodProperty.RigidTwo);
                
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(rodProperty.RodRigidBody);
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(rodProperty.RigidOne);
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(rodProperty.RigidTwo);
                
                ListOfRods.Add(rodProperty);
                positionXNA = positionXNA + new Vector3(spacing, 0, 0);
            }
            if (isClosed)
            {
                rodProperty = new Rod(ListOfRods[0].RigidOne, ListOfRods[ListOfRods.Count - 1].RigidTwo, spacing,
                                      rodLinkHeight, forgivingFactor);
                StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(rodProperty.RodRigidBody);
                ListOfRods.Add(rodProperty);
            }
            AddPairsOfNonCollidableToCollisionEngine();
        }

        private void AddPairsOfNonCollidableToCollisionEngine()
        {
            for (int i = 0; i < ListOfRods.Count; i++)
            {
                // Cash it
                Rod currentRod = ListOfRods[i];
                // rod link with masses
                StaticData.EngineManager.CollisionManagerEngine.AddRigidWithNonCollidableRigids(
                    currentRod.RodRigidBody, this.ListOfMasses);
                // rod link with other rod links
                for (int j = 0; j < ListOfRods.Count; j++)
                {
                    // rod link with masses
                    StaticData.EngineManager.CollisionManagerEngine.AddRigidWithNonCollidableRigids(
                        currentRod.RodRigidBody, ListOfRods[j].RodRigidBody);
                }
            }
            // mass with all other masses
            for (int i = 0; i < ListOfMasses.Count; i++)
            {
                StaticData.EngineManager.CollisionManagerEngine.AddRigidWithNonCollidableRigids(ListOfMasses[i],
                                                                                                this.ListOfMasses);
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this.ListOfMasses.Count - 1; i++)
            {
                Vector3 posXna1 = ListOfMasses[i].PositionCenterEngine;
                Vector3 posXna2 = ListOfMasses[i + 1].PositionCenterEngine;
                Visual2DRotatable visual2DRotatable = new Visual2DRotatable(posXna1, posXna2, 1, TextureType.DefaultBox);
                visual2DRotatable.Draw(gameTime);
            }
            if (IsClosed)
            {
                Vector3 posXna1 = ListOfMasses[0].PositionCenterEngine;
                Vector3 posXna2 = ListOfMasses[ListOfMasses.Count - 1].PositionCenterEngine;
                Visual2DRotatable visual2DRotatable = new Visual2DRotatable(posXna1, posXna2, 1, TextureType.DefaultBox);
                visual2DRotatable.Draw(gameTime);
            }
        }

        public void SetMassesNewState(bool isCollidable)
        {
            // Make joints drawable and set acceleration
            for (int i = 0; i < this.ListOfRods.Count; i++)
            {
                ListOfRods[i].RodRigidBody.IsDrawable = true;
                ListOfRods[i].RodRigidBody.SetAcceleration(new Vector3(0, -9.8f, 0));
            }

            // Remove Joints VS Masses from non collidables
            for (int i = 0; i < this.ListOfRods.Count; i++)
            {
                StaticData.EngineManager.CollisionManagerEngine.RemoveRigidWithNonCollidableRigids(
                    ListOfRods[i].RodRigidBody,
                    this.ListOfMasses);
            }

            // Remove Joints VS Joints from non collidables
            List<RigidBody> listOfJoints = new List<RigidBody>();
            ListOfRods.ForEach(rod => listOfJoints.Add(rod.RodRigidBody));
            for (int i = 0; i < this.ListOfRods.Count; i++)
            {
                StaticData.EngineManager.CollisionManagerEngine.RemoveRigidWithNonCollidableRigids(
                    ListOfRods[i].RodRigidBody,
                    listOfJoints);
            }

            // Remove Masses VS Masses from non collidables
            for (int i = 0; i < ListOfMasses.Count; i++)
            {
                StaticData.EngineManager.CollisionManagerEngine.RemoveRigidWithNonCollidableRigids(ListOfMasses[i],
                                                                                                   this.ListOfMasses);
            }
        }
	}
}
