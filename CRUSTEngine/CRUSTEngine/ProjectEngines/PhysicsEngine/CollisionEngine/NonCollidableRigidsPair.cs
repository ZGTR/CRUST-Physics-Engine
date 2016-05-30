using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
{
    [Serializable]
    public class NonCollidableRigidsPair
    {
        public RigidBody RigidBody1;
        public RigidBody RigidBody2;

        public NonCollidableRigidsPair(RigidBody r1, RigidBody r2)
        {
            this.RigidBody1 = r1;
            this.RigidBody2 = r2;
        }
    }
}
