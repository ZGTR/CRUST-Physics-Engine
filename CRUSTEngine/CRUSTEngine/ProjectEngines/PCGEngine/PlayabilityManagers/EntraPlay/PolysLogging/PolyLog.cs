using System;
using System.Collections.Generic;
using ClipperLib;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging
{
    [Serializable]
    public class PolyLog
    {
        public CompEntityPoly Comp;
        public List<APPair> ApPairs;

        public List<List<IntPoint>> PolysUnion {
            get
            {
                List<List<IntPoint>> union = new List<List<IntPoint>>();
                foreach (APPair apPair in ApPairs)
                {
                    union = EntraSolver.GetPolySolution(union, apPair.Poly, ClipType.ctUnion);
                }
                return union;
            }
        }

        public PolyLog(CompEntityPoly comp)
        {
            this.Comp = comp;
            ApPairs = new List<APPair>();
        }

        public PolyLog(CompEntityPoly comp, List<IntPoint> poly, CompEntityPoly adderComp)
        {
            this.Comp = comp;
            ApPairs = new List<APPair>();
            AddPoly(poly, adderComp);
        }

        public PolyLog(CompEntityPoly comp, List<List<IntPoint>> polys, CompEntityPoly adderComp)
        {
            this.Comp = comp;
            ApPairs = new List<APPair>();
            AddPoly(polys, adderComp);
        }

        public void AddPoly(List<IntPoint> poly, CompEntityPoly adderComp)
        {
            ApPairs.Add(new APPair() {AdderComp = adderComp, Poly = new List<List<IntPoint>>(){poly}});
        }

        public void AddPoly(List<List<IntPoint>> polys, CompEntityPoly adderComp)
        {
            ApPairs.Add(new APPair() {AdderComp = adderComp, Poly = polys});
        }

        //public bool IsOperation(PolyLog parentLog, ClipType ctOperation)
        //{
        //    List<List<IntPoint>> thisPolysUnion = GetPolysUnion(this.ApPairs);
        //    List<List<IntPoint>> parentPolysUnion = GetPolysUnion(parentLog.ApPairs);

        //    //EntraDrawer.DrawIntoFileTesting(thisPolysUnion);
        //    //EntraDrawer.DrawIntoFileTesting(parentPolysUnion);

        //    if (EntraSolver.IsPolyOperation(thisPolysUnion, parentPolysUnion, ClipType.ctIntersection))
        //    {
        //        //if (parentLog.Comp is BumperEntityPoly)
        //        {
        //            foreach (APPair apPair in parentLog.ApPairs)
        //            {
        //                if (apPair.AdderComp == this.Comp)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        //else
        //        //{
        //        //    if (parentLog.ApPairs[0].AdderComp == this.Comp)
        //        //    {
        //        //        return true;
        //        //    }
        //        //}
        //    }

        //    return false;
        //}

        private List<List<IntPoint>> GetPolysUnion(List<APPair> polys)
        {
            var res = new List<List<IntPoint>>();
            foreach (APPair apPair in polys)
            {
                res = EntraSolver.GetPolySolution(res, apPair.Poly, ClipType.ctUnion);
            }
            return res;
        }

        public bool IsOperation(PolyLog ppLog, PolyLog pLog, ClipType ctOperation)
        {
            foreach (APPair aP in pLog.ApPairs)
            {
                if (aP.AdderComp == this.Comp)
                {
                    //foreach (APPair apPairThis in this.ApPairs)
                    if (ppLog != null)
                    {
                        foreach (APPair aPP in ppLog.ApPairs)
                        {
                            if (EntraSolver.IsPolyOperation(aP.Poly, aPP.Poly, ClipType.ctIntersection))
                            {
                                //if (EntraSolver.IsPolyOperation(apPairParent.Poly, apPairThis.Poly, ClipType.ctIntersection))
                                return true;
                            }
                            //EntraDrawer.DrawIntoFileTesting(aPP.Poly);
                            //EntraDrawer.DrawIntoFileTesting(aP.Poly);
                        }
                        
                    }
                    else
                    {
                        // Frog
                        return true;
                    }
                }
            }
            return false;
        }
    }
}