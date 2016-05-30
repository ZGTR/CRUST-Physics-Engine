using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower
{
    [Serializable]
    public class BlowerManager : ServiceManager, IUpdatableComponent
    {
        public List<BlowerService> ListOfServices { get; set; }
        private int _currentId = -1;

        public BlowerManager()
        {
            ListOfServices = new List<BlowerService>();
        }


        public int GetNextServiceId()
        {
            _currentId++;
            return _currentId;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var service in ListOfServices)
            {
                service.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var service in ListOfServices)
            {
                service.Draw(gameTime);
            }
        }

        public void RemoveService(BlowerService service)
        {
            this.ListOfServices.Remove(service);
        }

        public void AddNewService(BlowerService service)
        {
            this.ListOfServices.Add(service);
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            if (service is BlowerService)
                this.AddNewService(service as BubbleService);
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            if (service is BlowerService)
                this.RemoveService(service as BubbleService);
        }

        public BlowerService GetService(int id)
        {
            for (int i = 0; i < this.ListOfServices.Count; i++)
            {
                if (this.ListOfServices[i].Id == id)
                {
                    return this.ListOfServices[i];
                }
            }
            return null;
        }
    }
}
