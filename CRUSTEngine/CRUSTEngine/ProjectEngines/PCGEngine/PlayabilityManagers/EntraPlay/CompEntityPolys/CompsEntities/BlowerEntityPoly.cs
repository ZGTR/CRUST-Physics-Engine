using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    class BlowerEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return ((BlowerService) this.CompObj).PositionXNA; }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((BlowerService)this.CompObj).PositionXNACenter; }
        }

        public BlowerEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return AreaCompPolyHandler.GetDefBlowerPoly((BlowerService) CompObj);
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            return new List<List<IntPoint>>() { AreaCompPolyHandler.GetBlowerPoly((BlowerService)CompObj) };
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adder)
        {
            List<List<IntPoint>> result  = new List<List<IntPoint>>();
            var initialPoly = this.GetAreaPoly();
            EntraDrawer.DrawIntoFile(initialPoly);

            this.EntraAgentSimple.PolysLogger.Log(new PolyLog(this, initialPoly, adder));
            result = EntraSolver.GetPolySolution(initialPoly, spaceSoFar, ClipType.ctUnion);
            EntraDrawer.DrawIntoFile(result);
            return result;
        }

        public void ReAddCoveredBumpsToBeProcessed(ref List<List<IntPoint>> initialPoly,
                                                          List<List<IntPoint>> spaceSoFar, CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple)
        {
            List<BumperEntityPoly> rBumpers = entraAgentSimple.BumpersHandler.GetReachableBumpers(spaceSoFar,
                                                                                            entraAgentSimple.AllCompsEntities);
            foreach (BumperEntityPoly bumperEntityPoly in rBumpers)
            {
                if (EntraSolver.IsPolyOperation(this.GetAreaPoly(), bumperEntityPoly.GetDefPoly(),
                                                ClipType.ctIntersection))
                {
                    //this.EntraAgentSimple.BumpersHandler.processedBumps.Remove(bumperEntityPoly); 
                }
            }
        }
    }
}
