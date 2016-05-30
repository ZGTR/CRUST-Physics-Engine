using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    public static class PolysHelper
    {

        public static List<List<IntPoint>> GetCompCoverageSoFar(CompEntityPoly me, List<PolyLog> polys)
        {
            List<List<IntPoint>> polysMe = new List<List<IntPoint>>();
            foreach (PolyLog polyLog in polys)
            {
                if (polyLog.Comp == me)
                {
                    foreach (APPair apPair in polyLog.ApPairs)
                    {
                        polysMe.AddRange(apPair.Poly);
                    }
                }
            }
            polysMe = EntraSolver.GetPolyUnions(polysMe);
            return polysMe;
        }


        public static List<IntPoint> GetShapeSemiCirclePoly(Vector2 centerPos, int nearWidth, 
            int verticalLineLength, int halfFarWidth)
        {
            int halfNearWidth = nearWidth / 2;
            halfNearWidth = (halfNearWidth == 0 ? 1 : halfNearWidth);


            int xUL = (int)(centerPos.X - halfNearWidth);
            int xUR = (int)(centerPos.X + halfNearWidth);

            int xBL = (int)(centerPos.X - halfFarWidth);
            int xBR = (int)(centerPos.X + halfFarWidth);

            int yUL = (int)(centerPos.Y);
            int yUR = (int)(centerPos.Y);

            int yBL = (int)(centerPos.Y + verticalLineLength);
            int yBR = (int)(centerPos.Y + verticalLineLength);

            return new List<IntPoint>()
                {
                    new IntPoint(xUL, yUL),
                    new IntPoint(xUR, yUR),
                    new IntPoint(xBR, yBR),
                    new IntPoint(xBL, yBL)
                };
        }

        public static List<IntPoint> GetShapeSquarePoly(Vector2 centerPos, int halfLength)
        {
            int xUL = (int)(centerPos.X - halfLength);
            int xUR = (int)(centerPos.X + halfLength);

            int xBL = (int)(centerPos.X - halfLength);
            int xBR = (int)(centerPos.X + halfLength);

            int yUL = (int)(centerPos.Y - halfLength);
            int yUR = (int)(centerPos.Y - halfLength);

            int yBL = (int)(centerPos.Y + halfLength);
            int yBR = (int)(centerPos.Y + halfLength);

            return new List<IntPoint>()
                {
                    new IntPoint(xUL, yUL),
                    new IntPoint(xUR, yUR),
                    new IntPoint(xBR, yBR),
                    new IntPoint(xBL, yBL)
                };
        }

        public static double GetPolygonArea(List<IntPoint> poly)
        {
            int i, j;
            double area = 0;

            for (i = 0; i < poly.Count; i++)
            {
                j = (i + 1) % poly.Count;

                area += poly[i].X * poly[j].Y;
                area -= poly[i].Y * poly[j].X;
            }

            area /= 2;
            return (area < 0 ? -area : area);
        }

        public static List<IntPoint> BuildPolygon(List<Vector2> shadowPoints)
        {
            List<IntPoint> result = new List<IntPoint>();
            shadowPoints.ForEach(p => result.Add(new IntPoint((int) p.X, (int) p.Y)));
            return result;
        }
    }
}
