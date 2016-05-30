using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PhysicsEngine;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps
{
    [Serializable]
    public class Frog : Component
    {
        public Frog(String[] pars) 
        {
            X = Int32.Parse(pars[0]);
            Y = Int32.Parse(pars[1]);
            CType = ComponentType.Frog;
        }

        public Frog(int x, int y)
        {
            X = x;
            Y = y;
            CType = ComponentType.Frog;
        }

        public override void AddSelfToEngine()
        {
            StaticData.EngineManager.FrogRB = new FrogRB(new Vector3(X, Y, 0), Material.Glass,
                                                                        StaticData.FrogHalfSize);
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(StaticData.EngineManager.FrogRB);
        }

        public override string ToString()
        {
            return "frog(" + X + "," + Y +  ")";
        }
    }
}
