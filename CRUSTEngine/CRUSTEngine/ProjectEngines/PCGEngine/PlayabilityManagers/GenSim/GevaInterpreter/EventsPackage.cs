using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter
{
    public class TLEventConverter
    {
        public static List<TLEvent> ConvertToItems(String pheno)
        {
            List<TLEvent> items = new List<TLEvent>();
            String[] pStringArr = pheno.Split(')');
            pStringArr = StringHelper.GetPropperPhenoArrWithTerminals(pStringArr);
            for (int i = 0; i < pStringArr.Length; i++)
            {
                try
                {
                    TLEvent comp = null;
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

        private static TLEvent GetNewComponent(String cmpName, String[] pars)
        {
            if (cmpName.ToLower().Equals("omnom_feed"))
            {
                return new EOmNomFeed(pars);
            }
            if (cmpName.ToLower().Equals("rope_cut"))
            {
                return new ERopeCut(pars);
            }
            if (cmpName.ToLower().Equals("blower_press"))
            {
                return new EBlowerPress(pars);
            }
            if (cmpName.ToLower().Equals("bubble_press"))
            {
                return new EBubblePress(pars);
            }
            if (cmpName.ToLower().Equals("rocket_press"))
            {
                return new ERocketPress(pars);
            }
            if (cmpName.ToLower().Equals("bumper_interaction"))
            {
                return new EBumperInteraction(pars);
            }


            if (cmpName.ToLower().Equals("rope"))
            {
                return new ERopePlac(pars);
            }
            if (cmpName.ToLower().Equals("blower"))
            {
                return new EBlowerPlac(pars);
            }
            if (cmpName.ToLower().Equals("bubble"))
            {
                return new EBubblePlac(pars);
            }
            if (cmpName.ToLower().Equals("rocket"))
            {
                return new ERocketPlac(pars);
            }
            if (cmpName.ToLower().Equals("bumper"))
            {
                return new EBumperPlac(pars);
            }
            return null;
        }
    }

    public enum EventType
    {
        RopeCut,
        BlowerPress,
        BubblePinch,
        RocketPress,
        BumperInteraction,
        OmNomFeed,
        RopePlac,
        BlowerPlac,
        BubblePlac,
        RocketPlac,
        BumperPlac,
    }

    public class TLEvent
    {
        public EventType eType;
        public int TTN;

        public TLEvent(String[] args)
        {
            TTN = Int32.Parse(args[0].Trim());
        }

        public string[] Args;
    }

    public class EBlowerPlac : TLEvent
    {
        public int dir;
        public EBlowerPlac(String[] args): base(args)
        {
            Args = args;
            eType = EventType.BlowerPlac;
        }
    }

    public class ERopePlac : TLEvent
    {
        public ERopePlac(String[] args)
            : base(args)
        {
            Args = args;
            eType = EventType.RopePlac;
        }
    }

    public class EBubblePlac : TLEvent
    {
        public EBubblePlac(String[] args)
            : base(args)
        {
            Args = args;
            eType = EventType.BubblePlac;
        }
    }

    public class ERocketPlac : TLEvent
    {
        public ERocketPlac(String[] args)
            : base(args)
        {
            Args = args;
            eType = EventType.RocketPlac;
            //dir = Int32.Parse(args[1]);
        }
    }

    public class EBumperPlac : TLEvent
    {
        public EBumperPlac(String[] args)
            : base(args)
        {
            Args = args;
            eType = EventType.BumperPlac;
        }
    }

    public class EBlowerPress : TLEvent
    {
        public EBlowerPress(String[] args)
            : base(args)
        {
            eType = EventType.BlowerPress;
        }
    }

    public class EBubblePress : TLEvent
    {

        public EBubblePress(String[] args)
            : base(args)
        {

            eType = EventType.BubblePinch;
        }
    }

    public class EBumperInteraction : TLEvent
    {

        public EBumperInteraction(String[] args)
            : base(args)
        {

            eType = EventType.BumperInteraction;
        }
    }


    public class EOmNomFeed : TLEvent
    {

        public EOmNomFeed(String[] args)
            : base(args)
        {

            eType = EventType.OmNomFeed;
        }
    }

    public class ERocketPress : TLEvent
    {

        public ERocketPress(String[] args)
            : base(args)
        {

            eType = EventType.RocketPress;
        }
    }

    public class ERopeCut : TLEvent
    {

        public ERopeCut(String[] args)
            : base(args)
        {

            eType = EventType.RopeCut;
        }
    }
}
