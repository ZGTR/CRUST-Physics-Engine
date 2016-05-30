using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rods;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.RopeRods
{
    [Serializable]
    public class RopeOfRodsManager : ServiceManager, IUpdatableComponent
    {
        public List<RopeOfRods> ListOfRopeOfRods;

        public RopeOfRodsManager()
        {
            ListOfRopeOfRods = new List<RopeOfRods>();
            //ListOfRopeOfRods.Add(new RopeOfRods(new Vector3(50, 50, 0), false, false, 7, new Vector3(0, -9.8f, 0), 60,
            //                                    10, 10));
            //ListOfRopeOfRods.Add(new RopeOfRods(new Vector3(100, 100, 0), false, false, 7, new Vector3(0, -9.8f, 0), 60,
            //                                    10, 10));
            //ListOfRopeOfRods.Add(new RopeOfRods(new Vector3(200, 200, 0), false, false, 7, new Vector3(0, -9.8f, 0), 60,
            //                                    10, 10));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < ListOfRopeOfRods.Count; i++)
            {
                if (ListOfRopeOfRods[i].IsFixed)
                    ListOfRopeOfRods[i].ListOfRods[0].RigidOne.PositionXNA =
                        ListOfRopeOfRods[i].PositionXNA;
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < ListOfRopeOfRods.Count; i++)
            {
                ListOfRopeOfRods[i].Draw(gameTime);
            }
        }

        public override void AddNewService(IUpdatableComponent service)
        {
            
        }

        public override void RemoveService(IUpdatableComponent service)
        {
            throw new NotImplementedException();
        }

        public void RemoveService(int ropeId)
        {
            try
            {
                ListOfRopeOfRods[ropeId].SetMassesNewState(true);
                ListOfRopeOfRods.RemoveAt(ropeId);
            }
            catch
            {
            }
        }
    }
}
