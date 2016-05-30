using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket
{
    [Serializable]
    public class RocketsManager : ServiceManager, IUpdatableComponent
    {
        public List<RocketService> ListOfRockets;

        public RocketsManager()
        {
            ListOfRockets = new List<RocketService>();
        }

        public void AddNewService(RocketService service)
        {
            
        }

        public void RemoveService(RocketService service)
        {
            
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            this.AddNewService(service as RocketService);
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            this.RemoveService(service as RocketService);
        }

        public void Update(GameTime gameTime)
        {
            try
            {
                foreach (var rocketService in ListOfRockets)
                {
                    rocketService.Update(gameTime);
                }
            }
            catch (Exception)
            {
            }

        }

        public void Draw(GameTime gameTime)
        {
            try
            {
                foreach (var rocketService in ListOfRockets)
                {
                    rocketService.Draw(gameTime);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
