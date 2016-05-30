using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents
{
    [Serializable]
    public class CookieRB : SphereRigid
    {
        public CookieRB(Vector3 positionXNA, Material mat, float radius) : base(positionXNA, mat, radius)
        {
            this.acceleration = new Vector3(0, -9.8f, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
