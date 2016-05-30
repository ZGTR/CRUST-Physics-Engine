using System;
using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    class RandomPlayabilityGenerator
    {
        //public RandomPlayabilityGenerator(List<Component> comps)
        //{
        //    List<Action> avaAction = GenerateAllAvailableActions(comps);
        //}

        //private List<Action> GenerateAllAvailableActions()
        //{
        //    List<Action> actions = new List<Action>();

        //    List<SpringService> ropes = StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
        //    List<BlowerService> blowers = StaticData.EngineManager.BlowerManagerEngine.ListOfServices;
        //    //List<BubbleService> bubbles = StaticData.EngineManager.BubbleManagerEngine.ListOfServices;
        //    //List<RocketCarrierService> rockets = StaticData.EngineManager.RocketsCarrierManagerEngine.ListOfServices;

        //    actions.AddRange(GetRopeActions(ropes));

        //}
        static Random _rand = new Random(DateTime.Now.Millisecond);
        public static Action GetNewRandomAction()
        {
            List<Action> actions = new List<Action>();
            List<SpringService> ropes = StaticData.EngineManager.SpringsManagerEngine.ListOfServices;
            List<BlowerService> blowers = StaticData.EngineManager.BlowerManagerEngine.ListOfServices;
            List<BubbleService> bubbles = StaticData.EngineManager.BubbleManagerEngine.ListOfServices;
            List<RocketCarrierService> rockets = StaticData.EngineManager.RocketsCarrierManagerEngine.GetListOfServices();
            actions.AddRange(GetRopeActions(ropes));
            actions.AddRange(GetBlowersActions(blowers));
            actions.AddRange(GetBubblesActions(bubbles));
            actions.AddRange(GetRocketsActions(rockets));
            actions.AddRange(GetVoidActions(ropes.Count + blowers.Count + bubbles.Count + rockets.Count));
            if (actions.Count > 0)
            {
                int indexRand = _rand.Next(0, actions.Count);
                return actions[indexRand];
            }
            return new VoidAction();
        }

        private static IEnumerable<Action> GetVoidActions(int otherCompsCount)
        {
            List<Action> actions = new List<Action>();
            if (otherCompsCount > 0)
            {
                int nrOfVoids = (otherCompsCount*80)/20;
                for (int i = 0; i < nrOfVoids; i++)
                {
                    actions.Add(new VoidAction());
                }
            }
            else
            {
                actions.Add(new VoidAction());
            }
            return actions;
        }

        private static IEnumerable<Action> GetRopeActions(List<SpringService> ropes)
        {
            List<Action> actions = new List<Action>();
            for (int i = 0; i < ropes.Count; i++)
            {
                SpringService cRope = ropes[i];
                actions.Add(new RopeCut(cRope.Id));
            }
            return actions;
        }

        private static IEnumerable<Action> GetBlowersActions(List<BlowerService> blowers)
        {
            List<Action> actions = new List<Action>();
            var list = blowers.Where(b => b.IsCookieNear).ToList();
            if (list.Count > 0)
            {
                actions.Add(new BlowerPress());
            }
            return actions;
        }

        private static IEnumerable<Action> GetBubblesActions(List<BubbleService> bubbles)
        {
            List<Action> actions = new List<Action>();
            if (bubbles.Count > 0)
            {
                var list = bubbles.Where(b => b.IsCookieCatched).ToList();
                if (list.Count > 0)
                {
                    actions.Add(new BubblePinch(list[0].Id));
                }
            }
            return actions;
        }


        private static IEnumerable<Action> GetRocketsActions(List<RocketCarrierService> rockets)
        {
            List<Action> actions = new List<Action>();
            if (rockets.Count > 0)
            {
                var list = rockets.Where(b => b.IsCookieCatched).ToList();
                if (list.Count > 0)
                {
                    actions.Add(new RocketPress());
                }
            }
            return actions;
        }

        //private List<Action> GenerateAllAvailableActions(List<Component> comps)
        //{
        //    List<Action> actions = new List<Action>();
        //    for (int i = 0; i < comps.Count; i++)
        //    {
        //        Component currComp = comps[i];
        //        actions.Add(GetAction(currComp));
        //    }
        //    return actions;
        //}

        //private Action GetAction(Component currComp)
        //{
        //    switch (currComp.CType)
        //    {
        //        case ComponentType.Cookie:
        //            break;
        //        case ComponentType.Frog:
        //            break;
        //        case ComponentType.Blower:
        //            return new BlowerPress();
        //            break;
        //        case ComponentType.Rope:
        //            return new RopeCut(((Rope) currComp).);
        //            break;
        //        case ComponentType.Bubble:
        //            break;
        //        case ComponentType.Water:
        //            break;
        //        case ComponentType.Rocket:
        //            break;
        //        case ComponentType.Bump:
        //            break;
        //        case ComponentType.Cracker:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

    }
}
