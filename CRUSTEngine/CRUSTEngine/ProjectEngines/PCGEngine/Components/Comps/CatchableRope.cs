using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components
{
    [Serializable]
    public class CatchableRope : Component {
        public int length;

        public CatchableRope(String[] pars)
        {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            length = Int32.Parse(pars[2]);
            CType = ComponentType.Rope;
        }

        public override void AddSelfToEngine()
        {
            int nrOfMasses = (length / 15) - 1;
            if (nrOfMasses <= 0)
            {
                nrOfMasses = 2;
            }
            CatchableRopeService rope = DefaultAdder.GetDefaultCatchableRope(new Vector3(X, Y, 0),
                StaticData.EngineManager.CookieRB.PositionXNA,
                                                                          nrOfMasses, 30*nrOfMasses,
                                                                          10f, 0.1f,
                                                                          RigidType.SphereRigid,
                                                                          new Vector3(5, 5, 0), false,
                                                                          SpringType.StrictRope);
            StaticData.EngineManager.SpringsManagerEngine.AddNewService(rope);
        }
    }
}
