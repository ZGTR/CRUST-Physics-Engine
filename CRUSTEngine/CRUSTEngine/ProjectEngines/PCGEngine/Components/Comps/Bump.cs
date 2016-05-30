using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PhysicsEngine;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps
{
    [Serializable]
    public class Bump : Component {
        public int dir;

        public Bump(String[] pars)
        {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            dir = Int32.Parse(pars[2]);
            CType = ComponentType.Bump;
        }

        public Bump(int x, int y, Direction d)
        {
            X = x;
            Y = y;
            dir = (int)d;
            CType = ComponentType.Bump;
        }

        public override void AddSelfToEngine()
        {
            Direction bDir = (Direction)(this.dir);
            BumpRigid boxRigid = new BumpRigid(new Vector3(this.X, this.Y, 0), Material.Wood, StaticData.BumpHalfSize,
                                              bDir);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(boxRigid);

        }

        public override string ToString()
        {
            return "bump(" + X + "," + Y + "," + (int)dir + ")";
        }
    }
}
