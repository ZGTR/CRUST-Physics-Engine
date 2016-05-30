using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators
{
    [Serializable]
    public class LevelGenerator
    {
        public List<Component> Items;
        public string LevelStrGeva { get; set; }
        
        public LevelGenerator(String levelStrGeva)
        {
            StaticData.UpdatesSoFar = 0;
            LevelStrGeva = levelStrGeva;
            this.Items = ConvertToItems(LevelStrGeva);
        }

        public void GenerateLevel()
        {
            AddCookieFrog();
            // Add Ropes First
            AddPureRopesToEngine();
            // Link Cookie with ropes
            LinkCookieWithAllRopes();
            // Update till Ropes Rests
            UpdateEngine(10*60);
            // Add Everything Else
            AddNonPureRopes();
            UpdateEngine(5*60);
            //if (listOfItemsLeftToAdd.Count > 0 )
            //{
            //    MessageBox.Show("Some Components not created successfully.");
            //}
        }

        private void AddCookieFrog()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var currectComp = Items[i];
                if (currectComp is Frog || currectComp is Cookie)
                {
                    currectComp.AddSelfToEngine();
                }
            }
        }

        private void AddNonPureRopes()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Component comp = Items[i];
                if (!(comp is Rope) && !(comp is Cookie) && !(comp is Frog))
                {
                    comp.AddSelfToEngine();
                }
            }
            //for (int i = 0; i < Items.Count; i++)
            //{
            //    if (Items[i] is CatchableRope)
            //        Items[i].AddSelfToEngine();
            //}
        }

        private void AddPureRopesToEngine()
        {
            List<Component> allRopes = (Items.Where(item => item is Rope)).ToList();
            for (int i = 0; i < allRopes.Count; i++)
            {
                allRopes[i].AddSelfToEngine();
            }
        }

        private void LinkCookieWithAllRopes()
        {
            SphereRigid cookie = StaticData.EngineManager.CookieRB;
            List<SpringService> allRopes =  StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
            for (int i = 0; i < allRopes.Count; i++)
            {
                if (!(allRopes[i] is CatchableRopeService))
                    allRopes[i].ApplyServiceOnRigid(cookie);
            }
        }

        private void UpdateEngine(int time)
        {
            PlayabilitySimulatorEngineProlog simulator = new PlayabilitySimulatorEngineProlog(false);
            simulator.RunEngineFreely(time, new GameTime());
        }

        private List<Component> GetComponentOfType(List<Component> items, ComponentType cType)
        {
            List<Component> components = new List<Component>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].CType == cType)
                {
                    components.Add(items[i]);
                }
            }
            return components;
        }

        public static List<Component> ConvertToItems(String pheno)
        {
            List<Component> items = new List<Component>();
            String[] pStringArr = pheno.Split(')');
            pStringArr = StringHelper.GetPropperPhenoArrWithTerminals(pStringArr);
            for (int i = 0; i < pStringArr.Length; i++) 
            {
                try
                {
                    Component comp = null;
                    String currentCmp = pStringArr[i].Trim();
                    if (!currentCmp.Contains("("))
                    {
                        comp = GetNewComponent(currentCmp, null);
                    }
                    else
                    {
                        String cmpName = currentCmp.Split('(')[0].Trim();
                        String[] pars = currentCmp.Split('(')[1].Trim().Split(',');
                        comp = GetNewComponent(cmpName, pars);
                    }
                    if (comp != null)
                        items.Add(comp);
                }
                catch (Exception)
                { }
            }
            return items;
        }

        private static Component GetNewComponent(String cmpName, String[] pars) {
            if (cmpName.ToLower().Equals("frog"))
            {
                return new Frog(pars);
            }
            if (cmpName.ToLower().Equals("cookie"))
            {
                return new Cookie(pars);
            }
            if (cmpName.ToLower().Equals("rope"))
            {
                return new Rope(pars);
            }
            if (cmpName.ToLower().Equals("blower"))
            {
                return new Blower(pars);
            }
            if (cmpName.ToLower().Equals("water"))
            {
                return new Water(pars);
            }
            if (cmpName.ToLower().Equals("bubble"))
            {
                return new Bubble(pars);
            }
            if (cmpName.ToLower().Equals("rocket"))
            {
                return new Rocket(pars);
            }
            if (cmpName.ToLower().Equals("bump"))
            {
                return new Bump(pars);
            }
            if (cmpName.ToLower().Equals("catchable_rope"))
            {
                return new CatchableRope(pars);
            }
            if (cmpName.ToLower().Equals("cracker"))
            {
                return new Cracker(pars);
            }
            return null;
        }

        //public static Dir GetDirection(int dir)
        //{
        //    switch (dir)
        //    {
        //        case 1:
        //            return Dir.East;
        //            break;
        //        case 2:
        //            return Dir.SouthEast;
        //            break;
        //        case 3:
        //            return Dir.South;
        //            break;
        //        case 4:
        //            return Dir.SoutWest;
        //            break;
        //        case 5:
        //            return Dir.West;
        //            break;
        //        case 6:
        //            return Dir.NorthWest;
        //            break;
        //        case 7:
        //            return Dir.North;
        //            break;
        //        case 8:
        //            return Dir.NorthEast;
        //            break;
        //    }
        //    return Dir.East;
        //}
    }
}
