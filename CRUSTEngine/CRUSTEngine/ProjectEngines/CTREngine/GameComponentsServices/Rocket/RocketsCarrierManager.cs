using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket
{
    [Serializable]
    public class RocketsCarrierManager : ServiceManager, IUpdatableComponent
    {
        //private List<RocketCarrierService> ListOfServices { set; get; }
        //public List<RocketCarrierService> GetListOfServices()
        //{
        //    return ListOfServices;
        //}

        //public RocketCarrierService RocketNew;

        public RocketsCarrierManager()
        {
            //ListOfServices = new List<RocketCarrierService>();
        }

        public void AddNewService(RocketCarrierService service)
        {
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(service);
        }

        public void RemoveService(RocketCarrierService service)
        {
           
            StaticData.EngineManager.RigidsManagerEngine.DeleteRigid(service);
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            this.AddNewService(service as RocketCarrierService);
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            this.RemoveService(service as RocketCarrierService);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
        }

        private RocketCarrierService _currentActivatedRocket = null;
        public void SetRocketNew(RocketCarrierService rocketCarrierService)
        {
            if(_currentActivatedRocket == null)
            {
                this._currentActivatedRocket = rocketCarrierService;
            }
            else
            {
                _currentActivatedRocket.CanCatchCookie = false;
                _currentActivatedRocket = rocketCarrierService;
            }
        }

        public List<RocketCarrierService> GetListOfServices()
        {
            List<RocketCarrierService> list = new List<RocketCarrierService>();
            foreach (var rigid in StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids)
            {
                if (rigid is RocketCarrierService)
                {
                    list.Add(rigid as RocketCarrierService);
                }
            }
            return list;
        }
    }
}
