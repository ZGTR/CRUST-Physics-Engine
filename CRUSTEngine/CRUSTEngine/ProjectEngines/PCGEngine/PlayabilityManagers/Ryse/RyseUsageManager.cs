using System;
using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    public class RyseUsageManager
    {
        public  List<BumpRigid> CollidedBumps = new List<BumpRigid>();
        public  int UsedRocket = 0;
        public  int UsedBumps = 0;
        public  int UsedBlowers = 0;
        public  int UsedBubbles = 0;
        public  int UsedRopes = 0;
        public  List<Component> ComponentsHasActions = new List<Component>();
        private List<Action> _actions;
        private List<Component> _items;

        public RyseUsageManager(List<Component> items, List<Action> actions)
        {
            this._items = items;
            this._actions = actions;
        }

        public void DoAnalysis()
        {
            foreach (Component component in _items)
            {
                SetComponentUsageFromActions(component);
            }
        }

        private void SetComponentUsageFromActions(Component component)
        {
            if (component is Bump)
            {
                Bump b = (Bump) component;
                foreach (BumpRigid bumpRigid in CollidedBumps)
                {
                    if (b.X == bumpRigid.PositionXNA.X && b.Y == bumpRigid.PositionXNA.Y)
                    {
                        if (!ComponentsHasActions.Contains(component))
                        {
                            UsedBumps++;
                            ComponentsHasActions.Add(component);
                        }
                        return;
                    }
                }
            }
            else
            {
                List<Action> properActions = GetActionsAccordingToComponent(component);
                foreach (Action action in properActions)
                {
                    if(IsActionOnComponent(component, action))
                    {
                        if (!ComponentsHasActions.Contains(component))
                        {
                            AddUsedComponent(component);
                            ComponentsHasActions.Add(component);
                        }
                        return;
                    }
                }
            }
            return;
        }

        private void AddUsedComponent(Component component)
        {
            switch (component.CType)
            {
                case ComponentType.Cookie:
                    break;
                case ComponentType.Frog:
                    break;
                case ComponentType.Blower:
                    UsedBlowers++;
                    break;
                case ComponentType.Rope:
                    UsedRopes++;
                    break;
                case ComponentType.Bubble:
                    UsedBubbles++;
                    break;
                case ComponentType.Water:
                    break;
                case ComponentType.Rocket:
                    UsedRocket++;
                    break;
                case ComponentType.Bump:
                    break;
                case ComponentType.Cracker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsActionOnComponent(Component component, Action action)
        {
            switch (action.AType)
            {
                case ActionType.BlowerPress:
                    {
                        Blower item = ((Blower) component);
                        BlowerService service = ((BlowerPress) action).BlowerPressed;
                        return item.X == service.PositionXNA.X && item.Y == service.PositionXNA.Y;
                    }
                    break;
                case ActionType.RopeCut:
                    {
                        Rope item = ((Rope)component);
                        SpringService service = ((RopeCut)action).RopeBeingCut;
                        return item.X == service.Masses[0].PositionXNA.X && item.Y == service.Masses[0].PositionXNA.Y;
                    }
                    break;
                case ActionType.VoidAction:
                    break;
                case ActionType.BubblePinch:
                    {
                        Bubble item = ((Bubble)component);
                        BubbleService service = ((BubblePinch)action).BubblePinched;
                        return item.X == service.PositionXNAInitial.X && item.Y == service.PositionXNAInitial.Y;
                    }
                    break;
                case ActionType.TerminateBranch:
                    break;
                case ActionType.RocketPress:
                    {
                        Rocket item = ((Rocket)component);
                        RocketCarrierService service = ((RocketPress)action).RocketPressed;
                        return item.X == service.PositionXNAInitial.X && item.Y == service.PositionXNAInitial.Y;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        private List<Action> GetActionsAccordingToComponent(Component component)
        {
            List<Action> properActions = new List<Action>();
            switch (component.CType)
            {
                case ComponentType.Cookie:
                    break;
                case ComponentType.Frog:
                    break;
                case ComponentType.Blower:
                    properActions.AddRange(_actions.Where(a => a.AType == ActionType.BlowerPress));
                    break;
                case ComponentType.Rope:
                    properActions.AddRange(_actions.Where(a => a.AType == ActionType.RopeCut));
                    break;
                case ComponentType.Bubble:
                    properActions.AddRange(_actions.Where(a => a.AType == ActionType.BubblePinch));
                    break;
                case ComponentType.Water:
                    break;
                case ComponentType.Rocket:
                    properActions.AddRange(_actions.Where(a => a.AType == ActionType.RocketPress));
                    break;
                case ComponentType.Bump:
                    break;
                case ComponentType.Cracker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return properActions;
        }
    }
}
