using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components
{
    [Serializable]
    public class Cookie : Component 
    {
        public Cookie(String[] pars) {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            CType = ComponentType.Cookie;
        }

        public override void AddSelfToEngine()
        {
            StaticData.EngineManager.CookieRB = new CookieRB(new Vector3(X, Y, 0), Material.Glass, 10);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(StaticData.EngineManager.CookieRB);
        }

        public override string ToString()
        {
            return "cookie(" + X + "," + Y + ")";
        }
    }
}
