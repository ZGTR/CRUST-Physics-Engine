using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps
{
    [Serializable]
    public class Rocket : Component {
        public int dir;

        public Rocket(String[] pars) {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            dir = Int32.Parse(pars[2]);
            CType = ComponentType.Rocket;
        }

        public Rocket(int x, int y, Direction d)
        {
            X = x;
            Y = y;
            dir = (int)d;
            CType = ComponentType.Rocket;
        }

        public override void AddSelfToEngine()
        {
            Direction dir = (Direction)this.dir;
            StaticData.EngineManager.RocketsCarrierManagerEngine.AddNewService(
                new RocketCarrierService(new Vector3(X, Y, 0), dir));
        }

        public override string ToString()
        {
            return "rocket(" + X + "," + Y + "," + (int)dir + ")";
        }
    }
}
