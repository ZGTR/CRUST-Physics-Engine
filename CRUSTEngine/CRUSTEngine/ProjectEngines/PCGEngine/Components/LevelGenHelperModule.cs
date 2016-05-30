using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;

namespace CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components
{
    public class LevelGenHelperModule
    {
        public static List<Blower> GetBlowerType(List<Component> blowersComps)
        {
            List<Blower> blowers = new List<Blower>();
            for (int i = 0; i < blowersComps.Count; i++)
            {
                blowers.Add((Blower)blowersComps[i]);
            }
            return blowers;
        }

        public static List<Rope> GetRopeType(List<Component> ropesComps)
        {
            List<Rope> ropes = new List<Rope>();
            for (int i = 0; i < ropesComps.Count; i++)
            {
                ropes.Add((Rope)ropesComps[i]);
            }
            return ropes;
        }
    }
}
