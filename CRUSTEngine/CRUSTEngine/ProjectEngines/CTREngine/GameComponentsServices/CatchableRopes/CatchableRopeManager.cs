//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;

//namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble
//{
//    [Serializable]
//    public class CatchableRopeManager : ServiceManager, IUpdatableComponent
//    {
//        public List<CatchableRopeService> ListOfServices { get; set; }
//        private static int _currentId = -1;

//        public CatchableRopeManager()
//        {
//            ListOfServices = new List<CatchableRopeService>();
//        }

//        public static int GetNextServiceId()
//        {
//            _currentId++;
//            return _currentId;
//        }

//        public void Update(GameTime gameTime)
//        {
//            foreach (var service in ListOfServices)
//            {
//                service.Update(gameTime);
//            }
//        }

//        public void Draw(GameTime gameTime)
//        {
//            foreach (var service in ListOfServices)
//            {
//                service.Draw(gameTime);
//            }
//        }

//        public void RemoveService(CatchableRopeService serviceToDelete)
//        {
//            this.ListOfServices.Remove(serviceToDelete);
//        }

//        public void AddNewService(CatchableRopeService service)
//        {
//            this.ListOfServices.Add(service);
//        }

//        public override void AddNewService(IUpdatableComponent service)
//        {
//            if (service is CatchableRopeService)
//                this.AddNewService(service as CatchableRopeService);
//        }

//        public override void RemoveService(IUpdatableComponent service)
//        {
//            if (service is CatchableRopeService)
//                this.RemoveService(service as CatchableRopeService);
//        }

//        public CatchableRopeService GetService(int id)
//        {
//            for (int i = 0; i < this.ListOfServices.Count; i++)
//            {
//                if (this.ListOfServices[i].Id == id)
//                {
//                    return this.ListOfServices[i];
//                }
//            }
//            return null;
//        }
//    }
//}
