using System;
using System.Collections.Generic;
using ClipperLib;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging
{
    [Serializable]
    public class APPair
    {
        public CompEntityPoly AdderComp;
        public List<List<IntPoint>> Poly;
    }
}
