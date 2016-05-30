using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids
{
    public class BoxRigidHardConstraint : BoxRigid
    {
        public BoxRigidHardConstraint(Vector3 positionXNA, Material mat, Vector3 halfSize) 
            : base(positionXNA, mat, halfSize)
        {

        }

        public override void SetMass(Vector3 halfSize)
        {
            Mass = halfSize.X * 2 * halfSize.Y * 2 * 4 * StaticData.DensityTable[(int)this.material];
            Mass /= (4 * StaticData.MassDivConst);
            InvMass = 1 / Mass;
        }
    }
}
