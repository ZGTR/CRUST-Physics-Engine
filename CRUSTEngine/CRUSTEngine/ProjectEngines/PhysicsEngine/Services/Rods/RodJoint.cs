using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rods
{
    [Serializable]
    public class RodJoint
    {
        public RigidBody[] Body;
        Vector3[] _position;
        float _error;

        public RodJoint(float error)
        {
            this._error = error;
        }

        public void Set(
            RigidBody a, Vector3 aPos,
            RigidBody b, Vector3 bPos
            )
        {
            Body = new RigidBody[2];
            _position = new Vector3[2];

            Body[0] = a;
            Body[1] = b;

            _position[0] = aPos;
            _position[1] = bPos;
        }

        /**
         * Generates the contacts required to restore the joint if it
         * has been violated.
         */
        public void AddContact(ref CollisionData data)
        {
            // Calculate the length of the joint
            Vector3 aToB = _position[1] - _position[0];
            Vector3 normal = aToB;
            normal.Normalize();
            float length = aToB.Length();

            // Check if it is violated
            if (Math.Abs((double)length) > _error)
            {
                //BoxRigid boxLink = (body[1] as BoxRigid);
                //SphereRigid sphereRigid = (body[0] as SphereRigid);
                //this.position[0] = boxLink.PositionCenterEngine;
                //this.position[1] = boxLink.vertices[boxLink.FindJointIndex(sphereRigid.PositionCenterEngine)].Position;
                
                Contact c = new Contact();
                c.ContactNormal = normal;
                c.ContactPoint = (_position[0] + _position[1]) * 0.5f;
                c.Penetration = length - _error;
                c.SetBodyData(Body[0], Body[1], 1, 0);

                c.ContactToWorld.M11 = normal.X;
                c.ContactToWorld.M12 = normal.Y;
                c.ContactToWorld.M21 = normal.Y;
                c.ContactToWorld.M22 = normal.X;

                data.contacts.Add(c);
                data.addContacts(1);
            }
            return;
        }
    }
}
