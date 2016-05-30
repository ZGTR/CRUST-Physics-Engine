using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    class RopeEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return MathHelperModule.Get2DVector(((SpringService)this.CompObj).Masses[0].PositionXNA); }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((SpringService)this.CompObj).Masses[0].PositionXNACenter2D; }
        }

        public RopeEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return AreaCompPolyHandler.GetRopePoly((SpringService) CompObj, this.EntraAgentSimple.EngineState);
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            return AreaCompPolyHandler.GetRopePoly((SpringService) CompObj, this.EntraAgentSimple.EngineState);
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adder)
        {
            var initialPoly = this.GetAreaPoly();
            this.EntraAgentSimple.PolysLogger.Log(new PolyLog(this, initialPoly, null));

            var result = EntraSolver.GetPolySolution(initialPoly, spaceSoFar, ClipType.ctUnion);
            return result;
        }
    }
}
