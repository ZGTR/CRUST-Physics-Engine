using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    public class BubbleEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return ((BubbleService)this.CompObj).PositionXNA; }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((BubbleService)this.CompObj).PositionXNACenter; }
        }

        public BubbleEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
            
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adder)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();
            var initialPoly = this.GetAreaPoly();
            EntraDrawer.DrawIntoFile(initialPoly);

            if (adder is BlowerEntityPoly)
            {
                AddBlowersEffect(ref initialPoly);
            }
            EntraDrawer.DrawIntoFile(initialPoly);
            this.EntraAgentSimple.PolysLogger.Log(new PolyLog(this, initialPoly, adder));

            result = EntraSolver.GetPolySolution(initialPoly, spaceSoFar, ClipType.ctUnion);
            EntraDrawer.DrawIntoFile(result);
            return result;
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return new List<List<IntPoint>>() {this.EntraAgentSimple.DefCompPolyHandler.GetDefBubblePoly((BubbleService) CompObj)};
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            List<List<IntPoint>> initialPoly = new List<List<IntPoint>>() { AreaCompPolyHandler.GetBubblePoly((BubbleService)CompObj) };
            return initialPoly;
        }

        private void AddBlowersEffect(ref List<List<IntPoint>> initialPoly)
        {
            BubbleService bubbleService = this.CompObj as BubbleService;
            Vector2 bubblePos = bubbleService.PositionXNACenter;

            foreach (BlowerService blowerService in this.EntraAgentSimple.EngineState.BlowerManagerEngine.ListOfServices)
            {
                if (bubbleService.PositionXNACenter.Y > blowerService.PositionXNACenter.Y - 40)
                {
                    if (Math.Abs(bubbleService.PositionXNACenter.X - blowerService.PositionXNACenter.X) <
                        StaticData.BlowerEffectAreaRadius)
                    {
                        if (HelperModules.GenericHelperModule.InDirectionOf(blowerService.PositionXNACenter, bubblePos,
                                                                            blowerService.Dir))
                        {
                            switch (blowerService.Dir)
                            {
                                case Direction.East:
                                    initialPoly.Add(new List<IntPoint>()
                                        {
                                            new IntPoint((int) bubblePos.X,
                                                         (int) bubblePos.Y - StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X + StaticData.BubbleWithBlowerRange,
                                                         (int) bubblePos.Y - StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X + StaticData.BubbleWithBlowerRange,
                                                         (int) bubblePos.Y + StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X,
                                                         (int) bubblePos.Y + StaticData.LevelFarHeight),
                                        });
                                    break;
                                case Direction.West:
                                    initialPoly.Add(new List<IntPoint>()
                                        {
                                            new IntPoint((int) bubblePos.X - StaticData.BubbleWithBlowerRange,
                                                         (int) bubblePos.Y - StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X,
                                                         (int) bubblePos.Y - StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X,
                                                         (int) bubblePos.Y + StaticData.LevelFarHeight),
                                            new IntPoint((int) bubblePos.X - StaticData.BubbleWithBlowerRange,
                                                         (int) bubblePos.Y + StaticData.LevelFarHeight),
                                        });
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                }
            }
        }
    }
}
