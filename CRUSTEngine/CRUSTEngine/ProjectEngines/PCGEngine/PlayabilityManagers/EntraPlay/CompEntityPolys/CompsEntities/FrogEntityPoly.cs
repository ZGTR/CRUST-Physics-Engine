using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    class FrogEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return ((FrogRB) this.CompObj).PositionXNA2D; }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((FrogRB)this.CompObj).PositionXNACenter2D; }
        }

        public FrogEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return new List<List<IntPoint>>()
                {
                    PolysHelper.GetShapeSquarePoly(this.EntraAgentSimple.EngineState.FrogRB.PositionXNACenter2D, 25)
                };
        }

        public override List<List<IntPoint>> GetCoverageSoFar()
        {
            return GetDefPoly();
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            throw new NotImplementedException();
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adder)
        {
            throw new NotImplementedException();
        }
    }
}
