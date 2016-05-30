using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps
{
    [Serializable]
    public class Bubble : Component
    {
        public Bubble(String[] pars)
        {
            if (pars.Length == 2)
            {
                X = Int32.Parse(pars[0]);
                Y = Int32.Parse(pars[1]);
            }
            else
            {
                X = Int32.Parse(pars[1]);
                Y = Int32.Parse(pars[2]);
            }
            CType = ComponentType.Bubble;
        }

        public Bubble(int x, int y)
        {
            X = x;
            Y = y;
            CType = ComponentType.Bubble;
        }

        public override void AddSelfToEngine()
        {
            StaticData.EngineManager.BubbleManagerEngine.AddNewService(new BubbleService(new Vector3(X, Y, 0)));
        }

        public override string ToString()
        {
            return "bubble(" + X + "," + Y + ")";
        }
    }
}
