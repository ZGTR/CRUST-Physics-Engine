using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Managers
{
    [Serializable]
    class NonCollidableRigidsComparer : IEqualityComparer<NonCollidableRigidsPair>
    {
        public bool Equals(NonCollidableRigidsPair x, NonCollidableRigidsPair y)
        {
            if (x.RigidBody1 == y.RigidBody1)
            {
                if (x.RigidBody2 == y.RigidBody2)
                {
                    return true;
                }
            }
            if (x.RigidBody1 == y.RigidBody2)
            {
                if (x.RigidBody2 == y.RigidBody1)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetHashCode(NonCollidableRigidsPair obj)
        {
            throw new NotImplementedException();
        }
    }
}
