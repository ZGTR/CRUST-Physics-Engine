using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim
{
    class GenSimHelper
    {
        public static List<ActionTimePair> ConvertTLEventstoActionTimePair(List<TLEvent> events)
        {
            List<ActionTimePair> res = new List<ActionTimePair>();
            int tsNow = 15 * 60 + GenSimAgent.VoidInitCount * GenSimAgent.GetActionsFrequency(new VoidAction());
            foreach (TLEvent tlEvent in events)
            {
                res.Add(new ActionTimePair(tlEvent.eType, tsNow));
                tsNow += (int)((tlEvent.TTN / (float)1000) * 60);
            }
            return res;
        }

        public static List<CATimePair> ConvertTLEventstoCATimePair(List<TLEvent> events)
        {
            List<CATimePair> res = new List<CATimePair>();
            int tsNow = 15 * 60 + GenSimAgent.VoidInitCount * GenSimAgent.GetActionsFrequency(new VoidAction());
            foreach (TLEvent tlEvent in events)
            {
                if (tlEvent.eType == EventType.RopeCut || tlEvent.eType == EventType.RocketPress
                    || tlEvent.eType == EventType.BumperInteraction || tlEvent.eType == EventType.BlowerPress
                    || tlEvent.eType == EventType.BubblePinch)
                {
                    res.Add(new ActionTimePair(tlEvent.eType, tsNow));
                }
                else
                {
                    res.Add(new CompTimePair(GetCompType(tlEvent.eType), tsNow) {Args = tlEvent.Args});
                }
                tsNow += (int)((tlEvent.TTN / (float)1000) * 60);
            }
            return res;
        }

        private static ComponentType GetCompType(EventType eType)
        {
            switch (eType)
            {
                case EventType.RopePlac:
                    return ComponentType.Rope;
                    break;
                case EventType.BlowerPlac:
                    return ComponentType.Blower;
                    break;
                case EventType.BubblePlac:
                    return ComponentType.Bubble;
                    break;
                case EventType.RocketPlac:
                    return ComponentType.Rocket;
                    break;
                case EventType.BumperPlac:
                    return ComponentType.Bump;
                    break;
                case EventType.OmNomFeed:
                    return ComponentType.Frog;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eType");
            }
            throw new ArgumentOutOfRangeException("eType");
        }
    }
}
