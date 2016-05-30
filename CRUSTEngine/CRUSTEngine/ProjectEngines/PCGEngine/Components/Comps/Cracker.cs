using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps
{
    [Serializable]
    public class Cracker : Component {
        public int dir;

        public Cracker(String[] pars)
        {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            dir = Int32.Parse(pars[2]);
            CType = ComponentType.Cracker;
        }

        public override void AddSelfToEngine()
        {
            Direction bDir = (Direction)(this.dir);
            
        }
    }
}
