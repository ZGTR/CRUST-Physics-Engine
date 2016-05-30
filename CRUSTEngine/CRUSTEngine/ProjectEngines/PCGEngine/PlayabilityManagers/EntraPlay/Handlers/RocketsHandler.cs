using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers
{
    [Serializable]
    public class RocketsHandler
    {
        private readonly CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple _entraAgentSimple;
        public List<RocketEntityPoly> Openners;
        public List<RocketEntityPoly> All;
        public List<BumperEntityPoly> CollidedBumpers;

        public RocketsHandler(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple)
        {
            _entraAgentSimple = entraAgentSimple;
            
        }

        public List<RocketEntityPoly> GetNonOpenners()
        {
            return All.Where(e => !Openners.Contains(e)).ToList();
        }

        public void SetOpennerNonOpenner(List<List<IntPoint>> spaceSoFar, List<CompEntityPoly> allEntitiesList,
            List<BumperEntityPoly> rBumpers)
        {
            var rCompsAllNew = _entraAgentSimple.AreaCompPolyHandler.GetReachServicePolys(spaceSoFar, allEntitiesList);
            All = rCompsAllNew.Where(comp => comp is RocketEntityPoly).Cast<RocketEntityPoly>().ToList();
            
            // Find Openner
            Openners = new List<RocketEntityPoly>();
            CollidedBumpers = new List<BumperEntityPoly>();
            foreach (RocketEntityPoly rocketEntity in All)
            {
                bool collision = false;
                foreach (CompEntityPoly bumperEntity in rBumpers)
                {
                    var sol = EntraSolver.GetPolySolution(bumperEntity.GetDefPoly(), rocketEntity.GetAreaPoly(),
                                                          ClipType.ctIntersection);
                    if (sol.Count > 0)
                    {
                        CollidedBumpers.Add(bumperEntity as BumperEntityPoly);
                        collision = true;
                    }
                }
                if (!collision)
                {
                    Openners.Add(rocketEntity);
                }
            }
        }

        //public List<List<IntPoint>> SetAllReachRocketsEffect(List<List<IntPoint>> spaceSoFar, 
        //    List<CompEntityPoly> allEntitiesList,
        //    List<BumperEntityPoly> rBumpers)
        //{
        //    SetOpennerNonOpenner(spaceSoFar, allEntitiesList, rBumpers);
        //    foreach (RocketEntityPoly rocket in All)
        //    {
        //        spaceSoFar = rocket.ApplyEffect(spaceSoFar);
        //    }
        //    return spaceSoFar;
        //}

        //public List<List<IntPoint>> SetOpennerRocketsEffect(List<List<IntPoint>> spaceSoFar, List<CompEntityPoly> allEntitiesList,
        //    List<BumperEntityPoly> rBumpers)
        //{
        //    SetOpennerNonOpenner(spaceSoFar, allEntitiesList, rBumpers);
        //    if (Openners != null)
        //    {
        //        foreach (RocketEntityPoly openner in Openners)
        //        {
        //            spaceSoFar = openner.ApplyEffect(spaceSoFar);
        //        }
        //    }
        //    return spaceSoFar;
        //}
    }
}
