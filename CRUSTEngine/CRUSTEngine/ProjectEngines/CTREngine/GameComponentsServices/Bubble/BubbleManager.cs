using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble
{
    [Serializable]
    public class BubbleManager : ServiceManager, IUpdatableComponent
    {
        public List<BubbleService> ListOfServices { get; set; }
        private int _currentId = -1;

        public BubbleManager()
        {
            ListOfServices = new List<BubbleService>();
        }

        public int GetNextServiceId()
        {
            _currentId++;
            return _currentId;
        }

        public void Update(GameTime gameTime)
        {
            BubbleService serviceToDie = null;
            foreach (var bubbleService in ListOfServices)
            {
                if (!bubbleService.ShouldDie)
                    bubbleService.Update(gameTime);
                else
                    serviceToDie = bubbleService;
            }
            if (serviceToDie != null)
            {
                this.RemoveService(serviceToDie);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var bubbleService in ListOfServices)
            {
                bubbleService.Draw(gameTime);
            }
        }

        public void RemoveService(BubbleService serviceToDelete)
        {
            if (serviceToDelete.RigidInService != null)
                serviceToDelete.RigidInService.SetAwake(true);
            this.ListOfServices.Remove(serviceToDelete);
        }

        public void AddNewService(BubbleService bubbleService)
        {
            this.ListOfServices.Add(bubbleService);
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            if (service is BubbleService)
                this.AddNewService(service as BubbleService);
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            if (service is BubbleService)
                this.RemoveService(service as BubbleService);
        }

        public BubbleService GetService(int id)
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
