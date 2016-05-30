using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using ClipperLib;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.Starters;
using Color = System.Drawing.Color;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui
{
    [Serializable]
    public class EntraDrawer
    {
        private List<List<IntPoint>> solution;
        private List<List<IntPoint>> clips;
        private List<List<IntPoint>> subjs;
        public static Color ColorPen = Color.DarkRed;
        public static Color ColorSubjs = Color.LightGray;
        public static Color ColorClips = Color.Red;
        public static Color ColorSolutions = Color.Gold;
        private int subjsAlpha = 10, clipsAlpha = 10, solutionAlpha = 50;
        private Graphics _graphic;
        private static int PlaneDim = 1000;

        public EntraDrawer(List<List<IntPoint>> subjs, List<List<IntPoint>> clips = null, List<List<IntPoint>> solution = null)
        {
            this.solution = solution;
            this.subjs = subjs;
            this.clips = clips;
        }


        public EntraDrawer(List<IntPoint> subjs, List<List<IntPoint>> clips = null, List<List<IntPoint>> solution = null)
        {
            this.solution = solution;
            this.subjs = new List<List<IntPoint>>() { subjs };
            this.clips = clips;
        }


        public void DrawSolutionIntoFile(string imageInput = null, bool isClipping = true)
        {
            EntraForm form = PrepareDrawerForm(imageInput, isClipping);
            form.SaveEntraOutputImageIntoHDD();
        }

        public void DrawSolutionIntoFile(string imageInput, bool isClipping, int counter)
        {
            EntraForm form = PrepareDrawerForm(imageInput, isClipping);
            form.SaveEntraOutputImageIntoHDD(counter);
        }

        private EntraForm PrepareDrawerForm(string imageInput, bool isClipping)
        {
            EntraForm form;
            if (String.IsNullOrEmpty(imageInput))
            {
                form = new EntraForm();
            }
            else
            {
                form = new EntraForm(imageInput);
            }

            this._graphic = form.Graphics;

            //if (subjs != null)
            //    SortClockwise(subjs);
            //if (clips != null)
            //    SortClockwise(clips);
            //if (solution != null)
            //    SortClockwise(solution);


            if (isClipping)
            {
                ClipPolygonsFromBorders(ref subjs);
                ClipPolygonsFromBorders(ref clips);
                ClipPolygonsFromBorders(ref solution);
            }

            if (subjs != null)
                DrawPolygons(subjs, ColorSubjs, subjsAlpha);
            if (clips != null)
                DrawPolygons(clips, ColorClips, clipsAlpha);
            if (solution != null)
                DrawPolygons(solution, ColorSolutions, solutionAlpha);

            return form;
        }

        private void SortClockwise(List<List<IntPoint>> polys)
        {
            for (int index = 0; index < polys.Count; index++)
            {
                polys[index] = new ClockwiseSorter(polys[index]).Sort();
            }
        }

        private void ClipPolygonsFromBorders(ref List<List<IntPoint>> polys)
        {
            if (polys != null)
            {
                if (polys.Count > 0)
                {
                    // Clip with four borders
                    for (int i = 0; i < 7; i += 2)
                    {
                        List<IntPoint> poly = GetPlanePolygon((Direction)i);
                        polys = EntraSolver.GetPolySolution(polys, poly, ClipType.ctDifference);
                    }
                }
            }
        }

        public static List<IntPoint> GetPlanePolygon(Direction dir)
        {
            int farHeight = StaticData.LevelFarHeight - 5;
            int farWidth = StaticData.LevelFarWidth - 5;
            switch (dir)
            {
                case Direction.East: // Up
                    return new List<IntPoint>()
                        {
                            new IntPoint(1, 1),
                            new IntPoint(farWidth, 1),
                            new IntPoint(farWidth, -PlaneDim),
                            new IntPoint(1, -PlaneDim),
                        };
                    break;
                case Direction.South: // Right
                    return new List<IntPoint>()
                        {
                            new IntPoint(farWidth, 1),
                            new IntPoint(farWidth + PlaneDim, 1),
                            new IntPoint(farWidth + PlaneDim,  farHeight),
                            new IntPoint(farWidth, farHeight)
                        };
                    break;
                case Direction.West: // Bottom
                    return new List<IntPoint>()
                        {
                            new IntPoint(-10, farHeight),
                            new IntPoint(farWidth + 10, farHeight),
                            new IntPoint(farWidth + 10, farHeight + PlaneDim),
                            new IntPoint(-10, farHeight + PlaneDim),
                        };
                    break;
                case Direction.North: // Left
                    return new List<IntPoint>()
                        {
                            new IntPoint(1, 1),
                            new IntPoint(1, farHeight),
                            new IntPoint(-PlaneDim, farHeight),
                            new IntPoint(-PlaneDim, 1),
                        };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
        }

        private void DrawPolygons(List<List<IntPoint>> polygons, Color colorBrush, int transparency)
        {
            Pen myPen = new Pen(ColorPen, (float)0.6);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(transparency, colorBrush));

            foreach (var poly in polygons)
            {
                GraphicsPath path = new GraphicsPath();
                PointF[] pts = PolygonToPointFArray(poly, 1);
                path.AddPolygon(pts);
                foreach (var point in poly)
                {
                    _graphic.FillPath(myBrush, path);
                    _graphic.DrawPath(myPen, path);
                }
            }
        }

        static private PointF[] PolygonToPointFArray(List<IntPoint> pg, int scale)
        {
            PointF[] result = new PointF[pg.Count];
            for (int i = 0; i < pg.Count; ++i)
            {
                result[i].X = (float)pg[i].X / scale;
                result[i].Y = (float)pg[i].Y / scale;
            }
            return result;
        }


        public static void DrawIntoFile(List<IntPoint> poly, bool withInputImage = true)
        {
            if (Tester.IsTestingEntra)
            {
                EntraDrawer d = new EntraDrawer(new List<List<IntPoint>>() { poly });
                if (withInputImage)
                {
                    d.DrawSolutionIntoFile(StaticData.EntraImageInput);
                }
                else
                {
                    d.DrawSolutionIntoFile();
                }
            }
        }


        public static void DrawIntoFile(List<List<IntPoint>> polys, bool withInputImage = true)
        {
            if (Tester.IsTestingEntra)
            {
                EntraDrawer d = new EntraDrawer(polys);
                if (withInputImage)
                {
                    d.DrawSolutionIntoFile(StaticData.EntraImageInput);
                }
                else
                {
                    d.DrawSolutionIntoFile();
                }
            }
        }

        public static void DrawIntoFileGeva(List<List<IntPoint>> polys, bool withInputImage)
        {
            EntraDrawer d = new EntraDrawer(polys);
            if (withInputImage)
            {
                d.DrawSolutionIntoFile(StaticData.EntraImageInput);
            }
            else
            {
                d.DrawSolutionIntoFile();
            }
        }

        public static void DrawIntoFileGeva(List<List<List<IntPoint>>> polys, bool withInputImage)
        {
            //var polys1 = polys[0];
            foreach (List<List<IntPoint>> poly in polys)
            {
                EntraDrawer d = new EntraDrawer(poly);
                if (withInputImage)
                {
                    d.DrawSolutionIntoFile(StaticData.EntraImageInput);
                }
                else
                {
                    d.DrawSolutionIntoFile();
                }   
            }
        }

        public static void DrawIntoFileGeva(List<List<IntPoint>> polys, bool withInputImage, int counter)
        {
            EntraDrawer d = new EntraDrawer(polys);
            if (withInputImage)
            {
                d.DrawSolutionIntoFile(StaticData.EntraImageInput, true, counter);
            }
            else
            {
                d.DrawSolutionIntoFile(null, true, counter);
            }
        }

        public static void DrawIntoFileTesting(List<List<IntPoint>> polys)
        {
            //if (false)
            {
                EntraDrawer d = new EntraDrawer(polys);
                {
                    d.DrawSolutionIntoFile(StaticData.EntraImageInput);
                }
            }
        }

        public static Bitmap GetPolyOnlyBitmap(EntraResult result)
        {
            EntraDrawer drawer = new EntraDrawer(result.ReachableSpace,  result.Frog,
                                     result.FrogCompsInter);
            return drawer.GetPolyOnlySolutionBitmap();
        }

        private Bitmap GetPolyOnlySolutionBitmap()
        {
            EntraForm form = PrepareDrawerForm(null, true);
            return form.SavePolyOnlyIntoBitmap();
        }
    }
}
