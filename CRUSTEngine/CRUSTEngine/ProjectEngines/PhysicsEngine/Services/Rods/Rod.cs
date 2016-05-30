using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rods
{
    [Serializable]
    public class Rod
    {
        public BoxRigid RodRigidBody;

        public RigidBody RigidOne;
        public RigidBody RigidTwo;

        public int VertexOneIndex;
        public int VertexTwoIndex;

        public int VertexBoxOneIndex;
        public int VertexBoxTwoIndex;

        public RodJoint Joint1;
        public RodJoint Joint2;

        public Rod(Vector3 positionXNA, Vector3 gravityVector, int spacing,
            int rigidHalfSize, float rodLinkHeight, float forgivingFactor)
        {
            SphereRigid rigidOne = DefaultAdder.GetDefaultSphere(positionXNA, Material.Steel,
                                                                  rigidHalfSize, gravityVector, null,
                                                                  null, 0, false);

            SphereRigid rigidTwo = DefaultAdder.GetDefaultSphere(positionXNA + new Vector3(spacing, 0, 0),
                                                                  Material.Steel,
                                                                  rigidHalfSize, gravityVector, null,
                                                                  null, 0, false);

            MakeRod(rigidOne, rigidTwo, spacing, rodLinkHeight, forgivingFactor);
        }

        public Rod(RigidBody r1, RigidBody r2, float? spacing, float rodLinkHeight, float forgivingFactor)
        {
            MakeRod(r1, r2, spacing, rodLinkHeight, forgivingFactor);
        }

        public void MakeRod(RigidBody r1, RigidBody r2, float? spacing, float rodLinkHeight, float forgivingFactor)
        {
            BoxRigidHardConstraint boxLink;
            if (spacing == null)
            {
                boxLink = DefaultAdder.GetDefaultBoxRigidHardConstraint(r1.PositionXNA, Material.Wood,
                                                     new Vector3((r1.PositionXNA - r2.PositionXNA).Length(),
                                                                 rodLinkHeight,
                                                                 0),
                                                     new Vector3(0, 0, 0),
                                                     null,
                                                     null, 0, false);
                
            }
            else
            {
                boxLink = DefaultAdder.GetDefaultBoxRigidHardConstraint(r1.PositionXNA, Material.Wood,
                                                     new Vector3((float) spacing, rodLinkHeight, 0),
                                                     new Vector3(0, 0, 0),
                                                     null,
                                                     null, 0, false);
            }
            //boxLink.SetMass(100000);
            //r1.SetMass(50);
            //r2.SetMass(50);

            //boxLink.IsCollidable = false;
            boxLink.IsDrawable = false;

            int VertexOneIndex = -1;
            int VertexTwoIndex = -1;
            this.RigidOne = r1;
            this.RigidTwo = r2;
            this.VertexOneIndex = VertexOneIndex;
            this.VertexTwoIndex = VertexTwoIndex;

            this.RodRigidBody = boxLink;
            this.Joint1 = new RodJoint(forgivingFactor);
            this.Joint2 = new RodJoint(forgivingFactor);

            Vector3 position1, position2 = new Vector3();
            if (VertexOneIndex == -1)
                position1 = r1.PositionCenterEngine;
            else
                position1 = r1.vertices[VertexOneIndex].Position;

            if (VertexTwoIndex == -1)
                position2 = r2.PositionCenterEngine;
            else
                position2 = r2.vertices[VertexTwoIndex].Position;

            int vertexBoxOneIndex = boxLink.FindJointIndex(position1);
            int vertexBoxTwoIndex = boxLink.FindJointIndex(position2);

            this.VertexBoxOneIndex = vertexBoxOneIndex;
            this.VertexBoxTwoIndex = vertexBoxTwoIndex;

            this.Joint1.Set(r1, position1, boxLink, boxLink.vertices[vertexBoxOneIndex].Position);
            this.Joint2.Set(r2, position2, boxLink, boxLink.vertices[vertexBoxTwoIndex].Position);
        }
    }
}
