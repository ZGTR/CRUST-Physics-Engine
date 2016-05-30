using System;
using System.Collections.Generic;
using ClipperLib;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    [Serializable]
    class EntraSolver
    {
        public static List<List<IntPoint>> GetPolySolution(List<IntPoint> poly1, List<IntPoint> poly2, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(new List<List<IntPoint>>() { poly1 }, PolyType.ptSubject);
            c.AddPolygons(new List<List<IntPoint>>() { poly2 }, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            //ExecuteOperation();

            try
            {
                //Thread t =
                //new Thread(() =>
                //    {
                        c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
                //    });
                //t.Start();
                //t.Join(1000);
                ////t.Abort();
                
                
            }
            catch (Exception e1)
            {
                //throw;
            }
            return solution;
        }

        public static List<List<IntPoint>> GetPolySolution(List<List<IntPoint>> polys, List<IntPoint> poly2,
            ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(polys, PolyType.ptSubject);
            c.AddPolygons(new List<List<IntPoint>>() { poly2 }, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution;
        }

        public static List<List<IntPoint>> GetPolySolution(List<List<IntPoint>> polys1, 
            List<List<IntPoint>> polys2, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(polys1, PolyType.ptSubject);
            c.AddPolygons(polys2, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution;
        }

        public static List<List<IntPoint>> GetPolyUnions(List<List<IntPoint>> polys)
        {
            Clipper c = new Clipper();
            c.AddPolygons(polys, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution;
        }

        public static List<List<IntPoint>> GetPolySolution(List<IntPoint> poly, List<List<IntPoint>> polys, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(new List<List<IntPoint>>(){poly}, PolyType.ptSubject);
            c.AddPolygons(polys, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution;
        }

        public static bool IsPolyOperation(List<IntPoint> poly1, List<IntPoint> poly2, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(new List<List<IntPoint>>() { poly1 }, PolyType.ptSubject);
            c.AddPolygons(new List<List<IntPoint>>() { poly2 }, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution.Count != 0;
        }

        public static bool IsPolyOperation(List<List<IntPoint>> polys, List<IntPoint> poly2, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(polys, PolyType.ptSubject);
            c.AddPolygons(new List<List<IntPoint>>() { poly2 }, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution.Count != 0;
        }

        public static bool IsPolyOperation(List<List<IntPoint>> polys1, List<List<IntPoint>> polys2, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(polys1, PolyType.ptSubject);
            c.AddPolygons(polys2, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution.Count != 0;
        }

        public static bool IsPolyOperation(List<IntPoint> poly, List<List<IntPoint>> polys, ClipType cType)
        {
            Clipper c = new Clipper();
            c.AddPolygons(new List<List<IntPoint>>() { poly }, PolyType.ptSubject);
            c.AddPolygons(polys, PolyType.ptClip);
            var solution = new List<List<IntPoint>>();
            try
            {
                c.Execute(cType, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            catch (Exception)
            {
            }
            return solution.Count != 0;
        }
    }
}
