using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Rods
{
    [Serializable]
    public class RodsManager : IUpdatableComponent
    {
        public List<Rod> ListOfRods;
        public RodsManager()
        {
            ListOfRods = new List<Rod>();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            
        }
    }
}
