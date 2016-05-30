using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.TileSpring
{
    [Serializable]
    public class TileSpringServiceManager : ServiceManager, IUpdatableComponent
    {
        public List<TileSpringService> ListOfTileSprings;

        public TileSpringServiceManager()
        {
            ListOfTileSprings = new List<TileSpringService>();
            //ListOfTileSprings.Add(new TileSpringService(1000, 40, 0.2f, 10, 10, RigidType.SphereRigid));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < ListOfTileSprings.Count; i++)
            {
                ListOfTileSprings[i].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < ListOfTileSprings.Count; i++)
            {
                ListOfTileSprings[i].Draw(gameTime);
            }
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            throw new NotImplementedException();
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            throw new NotImplementedException();
        }
    }
}
