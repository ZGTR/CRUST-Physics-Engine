using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    public class RocketEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return ((RocketCarrierService)this.CompObj).PositionXNA2D; }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((RocketCarrierService)this.CompObj).PositionXNACenter2D; }
        }

        public RocketEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return new List<List<IntPoint>>() {this.EntraAgentSimple.DefCompPolyHandler.GetDefRocketPoly((RocketCarrierService) CompObj)};
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            return  AreaCompPolyHandler.GetRocketPoly((RocketCarrierService)CompObj);
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adder)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            var initialPoly = this.GetAreaPoly();
            //BumpersHandler.ReAddNewlyAddedBumpsAreas(ref initialPoly, this);
            this.EntraAgentSimple.PolysLogger.Log(new PolyLog(this, initialPoly, adder));

            result = EntraSolver.GetPolySolution(initialPoly, spaceSoFar, ClipType.ctUnion);
            EntraDrawer.DrawIntoFile(result);
            return result;
        }

//        public void ReAddCoveredBumpsToBeProcessed(ref Polygons initialPoly, EntraAgentSimple EntraAgentSimple)
//        {
//            RocketsHandler handler = new RocketsHandler(EntraAgentSimple);
//            List<BumperEntityPoly> rBumpers = EntraAgentSimple.BumpersHandler.GetReachableBumpers
//                (initialPoly, EntraAgentSimple.AllCompsEntities);
//            handler.SetOpennerNonOpenner(initialPoly, EntraAgentSimple.AllCompsEntities, rBumpers);
////            var allBumpers = handler.CollidedBumpers;
//            List<BumperEntityPoly> coveredBumps = null;// PolysHelper.GetCoveredBumpers(entity, initialPoly);
//  //              allBumpers.Where(e => !EntraAgentSimple.BumpersHandler.processedBumps.Contains(e)).ToList();

//            foreach (BumperEntityPoly bumper in coveredBumps)
//            {
//                List<List<IntPoint>> bumperDelPolys = bumper.GetDelPolys(initialPoly, new List<CompEntityPoly>() { this });
//                EntraDrawer.DrawIntoFile(bumperDelPolys);
//                List<List<IntPoint>> delIntersection = BumpersHandler.GetDelPolysIntersection(bumperDelPolys);
//                EntraDrawer.DrawIntoFile(delIntersection);
                
//                //var originalAreaToBeAdded = EntraSolver.GetPolySolution(result, delIntersection, ClipType.ctIntersection);
//                EntraDrawer.DrawIntoFile(initialPoly);

//                initialPoly = EntraSolver.GetPolySolution(initialPoly, delIntersection, ClipType.ctDifference);
//                EntraDrawer.DrawIntoFile(initialPoly);

//                //result = EntraSolver.GetPolySolution(result, originalAreaToBeAdded, ClipType.ctUnion);
//                //EntraDrawer.DrawIntoFile(result);
//            }

//            EntraAgentSimple.BumpersHandler.processedBumps.AddRange(coveredBumps);
//            //.ForEach(e => EntraAgentSimple.BumpersHandler.processedBumps.Remove(e));
//        }
    }
}
