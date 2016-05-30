using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps
{
    [Serializable]
    public class Rope : Component {
        public int Length;

        public Rope(String[] pars) {
            if (pars.Length == 3)
            {
                X = Int32.Parse(pars[0]);
                Y = Int32.Parse(pars[1]);
                Length = Int32.Parse(pars[2]);
            }
            else
            {
                X = Int32.Parse(pars[1]);
                Y = Int32.Parse(pars[2]);
                Length = Int32.Parse(pars[3]);
            }
            CType = ComponentType.Rope;
        }

        public Rope(int x, int y, int length)
        {
            X = x;
            Y = y;
            Length = length;
  
            CType = ComponentType.Rope;
        }

        public override void AddSelfToEngine()
        {
            int nrOfMasses = (int)Math.Floor(Length / (double)15);
            if (nrOfMasses <= 0)
            {
                nrOfMasses = 2;
            }
            StaticData.EngineManager.SpringsManagerEngine.AddNewService(
                DefaultAdder.GetDefaultSpringRope(new Vector3(X, Y, 0),
                StaticData.EngineManager.CookieRB.PositionXNA,
                                                  nrOfMasses, 25*nrOfMasses,
                                                  10f, 0.1f,
                                                  RigidType.SphereRigid,
                                                  new Vector3(5, 5, 0), false,
                                                  SpringType.StrictRope));
        }

        public override string ToString()
        {
            return "rope(" + X + "," + Y + "," + Length + ")";
        }
    }
}
