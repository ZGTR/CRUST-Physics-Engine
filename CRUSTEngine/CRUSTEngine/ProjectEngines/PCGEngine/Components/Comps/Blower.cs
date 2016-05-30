using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps
{
    [Serializable]
    public class Blower : Component {
        public int dir;

        public Blower(String[] pars) {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            dir = Int32.Parse(pars[2]);
            CType = ComponentType.Blower;
        }

        public Blower(int x, int y, Direction d)
        {
            X = x;
            Y = y;
            dir = (int)d;
            CType = ComponentType.Blower;
        }

        public override void AddSelfToEngine()
        {
            Direction bDir = (Direction)(this.dir);
            StaticData.EngineManager.BlowerManagerEngine.AddNewService(new BlowerService(new Vector3(X, Y, 0), bDir));
        }

        public override string ToString()
        {
            return "blower(" + X + "," + Y + "," + (int)dir + ")";
        }
    }
}
