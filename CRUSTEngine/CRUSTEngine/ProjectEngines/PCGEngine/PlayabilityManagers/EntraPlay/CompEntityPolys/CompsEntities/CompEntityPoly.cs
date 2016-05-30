using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    public abstract class CompEntityPoly
    {
        public object CompObj;

        public virtual Vector2 PositionXNA2D
        {
            get { return Vector2.Zero; }
        }

        public virtual Vector2 PositionXNACenter2D
        {
            get { return Vector2.Zero; }
        }

        public CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple EntraAgentSimple;

        public CompEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
        {
            this.EntraAgentSimple = entraAgentSimple;
            this.CompObj = compObj;
        }

        public abstract List<List<IntPoint>> GetDefPoly();
        public abstract List<List<IntPoint>> GetAreaPoly();
        public abstract List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly adderComp);

        public virtual List<List<IntPoint>> GetCoverageSoFar()
        {
            return PolysHelper.GetCompCoverageSoFar(this, this.EntraAgentSimple.PolysLogger.Logs);
        }


    }
}
