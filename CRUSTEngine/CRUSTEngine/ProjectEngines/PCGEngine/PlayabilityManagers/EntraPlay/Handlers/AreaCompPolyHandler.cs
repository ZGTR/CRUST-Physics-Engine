using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers
{
    [Serializable]
    public class AreaCompPolyHandler
    {
        public EngineManager _engineState;
        // Lengths
        private static int halfHeadLength = 15;
        protected static long BumperDelArea = 100;

        // Rope Lengths
        private static int ropeNearWidth = 2;
        private static int ropelongtitLength = 550;


        public const int nR = 50;
        public const int fR = 100;
        public const int dfR = 170;

        public AreaCompPolyHandler(EngineManager engineState)
        {
            _engineState = engineState;
        }

        public List<List<IntPoint>> GetBumpersDelPolys()
        {
            List<List<IntPoint>> allPolys = new List<List<IntPoint>>();
            List<BumpRigid> bumpers = _engineState.RigidsManagerEngine.
                                                   ListOfBoxRigids.Where(item => item is BumpRigid).Cast<BumpRigid>()
                                                  .ToList();
            foreach (var service in bumpers)
            {
                List<IntPoint> poly = null;
                poly = GetBumperDeletionPoly(service);
                allPolys.Add(poly);
            }
            return allPolys;
        }

        public static CookieDirection GetABCookieDirection(CompEntityPoly entity, BumperEntityPoly bumper)
        {
            var refPos = entity.PositionXNACenter2D;
            var bumpPos = bumper.PositionXNACenter2D;
            if (refPos.Y < bumpPos.Y)
            {
                return CookieDirection.FromAbove;
            }
            else
            {
                return CookieDirection.FromBottom;
            }
        }

        public static CookieDirection GetABCookieDirection(Vector2 ePos, Vector2 bPos)
        {
            if (ePos.Y < bPos.Y)
            {
                return CookieDirection.FromAbove;
            }
            else
            {
                return CookieDirection.FromBottom;
            }
        }

        public static CookieDirection GetRLCookieDirection(Vector2 ePos, Vector2 bPos)
        {
            if (ePos.X > bPos.X)
            {
                return CookieDirection.FromRight;
            }
            else
            {
                return CookieDirection.FromLeft;
            }
        }

        public static List<IntPoint> GetBumperPoly(BumpRigid me, Vector2 ePos)
        {
            Vector2 posCenter = me.PositionXNACenter2D;
            float xUL, xUR, xBL, xBR, yUL, yUR, yBL, yBR;
            xUL = xUR = xBL = xBR = yUL = yUR = yBL = yBR = 0;
            CookieDirection abDir = GetABCookieDirection(ePos, posCenter);
            CookieDirection rlDir = GetRLCookieDirection(ePos, posCenter);
            switch (me.Dir)
            {
                case Direction.East:
                case Direction.West:
                    if (abDir == CookieDirection.FromAbove)
                    {
                        if (rlDir == CookieDirection.FromLeft)
                        {
                            return new List<IntPoint>()
                                {
                                    new IntPoint((int) posCenter.X , (int) posCenter.Y - fR),
                                    new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y - fR + 10),
                                    new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X + nR, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X + nR, (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X, (int) posCenter.Y),
                                };
                        }
                        else
                        {
                            return new List<IntPoint>()
                                {
                                    new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y - fR + 10),
                                    new IntPoint((int) posCenter.X , (int) posCenter.Y - fR),
                                    new IntPoint((int) posCenter.X , (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X - nR, (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X - nR, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y + 900),
                                };
                        }
                    }
                    else  // From Bottom
                    {
                        if (rlDir == CookieDirection.FromLeft)
                        {
                            return new List<IntPoint>()
                                {
                                    new IntPoint((int) posCenter.X, (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y + 30),
                                    new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X, (int) posCenter.Y + 900),
                                };
                        }
                        else
                        {
                            return new List<IntPoint>()
                                {
                                    new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y + 30),
                                    new IntPoint((int) posCenter.X, (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y + 900),
                                };
                        }
                    }
                    break;
                
                case Direction.SouthEast:
                case Direction.NorthWest:
                    if (abDir == CookieDirection.FromAbove)
                    {
                        return new List<IntPoint>()
                            {
                                new IntPoint((int) posCenter.X - 20, (int) posCenter.Y - 20),
                                new IntPoint((int) posCenter.X + 20, (int) posCenter.Y - fR),
                                new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y - nR),
                                new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y + 900),
                                new IntPoint((int) posCenter.X + 20, (int) posCenter.Y + 900),
                                new IntPoint((int) posCenter.X + 20, (int) posCenter.Y + 20),
                            };
                    }
                    else
                    {
                        //if (abDir == CookieDirection.FromBottom)
                        {
                            return new List<IntPoint>()
                                {
                                    new IntPoint((int) posCenter.X - dfR/2, (int) posCenter.Y + 900),
                                    new IntPoint((int) posCenter.X - dfR/2, (int) posCenter.Y),
                                    new IntPoint((int) posCenter.X - 20, (int) posCenter.Y - 20),
                                    new IntPoint((int) posCenter.X + nR, (int) posCenter.Y + nR),
                                    new IntPoint((int) posCenter.X + nR, (int) posCenter.Y + 900),
                                };
                        }
                    }
                    break;
                case Direction.South:
                case Direction.North:
                    return new List<IntPoint>(){};
                    break;


                case Direction.SouthWest:
                case Direction.NorthEast:
                    if (abDir == CookieDirection.FromAbove)
                    {
                        return new List<IntPoint>()
                            {
                                new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y - fR),
                                new IntPoint((int) posCenter.X , (int) posCenter.Y - fR),
                                new IntPoint((int) posCenter.X + 20, (int) posCenter.Y - 20),
                                new IntPoint((int) posCenter.X - 20, (int) posCenter.Y + 20),
                                new IntPoint((int) posCenter.X - 20, (int) posCenter.Y + 900),
                                new IntPoint((int) posCenter.X - dfR, (int) posCenter.Y + 900)
                            };
                    }
                    else
                    {
                        return new List<IntPoint>()
                            {
                                new IntPoint((int) posCenter.X - 20, (int) posCenter.Y + 20),
                                new IntPoint((int) posCenter.X + 20, (int) posCenter.Y - 20),
                                new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y),
                                new IntPoint((int) posCenter.X + dfR, (int) posCenter.Y + 900),
                                new IntPoint((int) posCenter.X - 20, (int) posCenter.Y + 900),
                            };
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static List<IntPoint> GetBumperDeletionPoly(BumpRigid bump)
        {
            Vector2 posCenter = bump.PositionXNACenter2D;
            float xUL, xUR, xBL, xBR, yUL, yUR, yBL, yBR;
            xUL = xUR = xBL = xBR = yUL = yUR = yBL = yBR = 0;
            return new List<IntPoint>()
                        {
                            new IntPoint((int) posCenter.X - nR, (int) posCenter.Y),
                            new IntPoint((int) posCenter.X + nR, (int) posCenter.Y),
                            new IntPoint((int) posCenter.X + nR, (int) posCenter.Y + AreaCompPolyHandler.BumperDelArea),
                            new IntPoint((int) posCenter.X - nR, (int) posCenter.Y + AreaCompPolyHandler.BumperDelArea),
                        };
        }

        public static List<IntPoint> GetBubblePoly(BubbleService service)
        {
            Vector2 posCenter = service.PositionXNACenter;
            float xUL, xUR, xBL, xBR, yUL, yUR, yBL, yBR;

            xUL = posCenter.X - 20;
            yUL = posCenter.Y - 900;

            xUR = posCenter.X + 30;
            yUR = posCenter.Y - 900;

            xBR = posCenter.X + 30;
            yBR = posCenter.Y + 900;

            xBL = posCenter.X - 30;
            yBL = posCenter.Y + 900;

            return new List<IntPoint>()
                {
                    new IntPoint((int)xUL, (int)yUL),
                    new IntPoint((int)xUR, (int)yUR),
                    new IntPoint((int)xBR, (int)yBR),
                    new IntPoint((int)xBL, (int)yBL)
                };
        }

        public static List<IntPoint> GetBlowerPoly(BlowerService service, int distEffect = StaticData.BubbleWithBlowerRange)
        {
            Vector2 posCenter = service.PositionXNACenter;
            float xUL, xUR, xBL, xBR, yUL, yUR, yBL, yBR;
            xUL = xUR = xBL = xBR = yUL = yUR = yBL = yBR = 0;
            int verticalGravity = 30;
            int closeBorder = 30;
            switch (service.Dir)
            {
                case Direction.East:
                    xUL = posCenter.X;
                    yUL = posCenter.Y;

                    xUR = posCenter.X + distEffect;
                    yUR = posCenter.Y + verticalGravity;

                    xBR = xUR;
                    yBR = yUR + StaticData.LevelFarHeight;

                    xBL = xUL;
                    yBL = yUL + StaticData.LevelFarHeight;
                    break;
                case Direction.West:
                    xUL = posCenter.X - distEffect;
                    yUL = posCenter.Y + verticalGravity;

                    xUR = posCenter.X;
                    yUR = posCenter.Y;

                    xBR = xUR;
                    yBR = yUR + StaticData.LevelFarHeight;

                    xBL = xUL;
                    yBL = yUL + StaticData.LevelFarHeight;
                    break;
                default:
                    break;
            }

            return new List<IntPoint>()
                {
                    new IntPoint((int)xUL, (int)yUL),
                    new IntPoint((int)xUR, (int)yUR),
                    new IntPoint((int)xBR, (int)yBR),
                    new IntPoint((int)xBL, (int)yBL)
                };
        }

        public static List<List<IntPoint>> GetRocketPoly(RocketCarrierService service)
        {
            Vector2 posCenter = service.PositionXNACenter2D;
            float xUL, xUR, xBL, xBR, yUL, yUR, yBL, yBR;
            xUL = xUR = xBL = xBR = yUL = yUR = yBL = yBR = 0;

            switch (service.Dir)
            {
                case Direction.East:
                    xUL = posCenter.X;
                    yUL = posCenter.Y;

                    xUR = posCenter.X + 900;
                    yUR = posCenter.Y;

                    xBR = posCenter.X + 900;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.SouthEast:
                    xUL = posCenter.X;
                    yUL = posCenter.Y;

                    xUR = posCenter.X + 900;
                    yUR = posCenter.Y + 900;

                    xBR = posCenter.X + 900;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.South:
                    xUL = posCenter.X - 20;
                    yUL = posCenter.Y - 20;

                    xUR = posCenter.X + 20;
                    yUR = posCenter.Y - 20;

                    xBR = posCenter.X + 20;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X - 20;
                    yBL = posCenter.Y + 900;

                    break;
                case Direction.SouthWest:
                    xUL = posCenter.X;
                    yUL = posCenter.Y;

                    xUR = posCenter.X;
                    yUR = posCenter.Y;

                    xBR = posCenter.X;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X - 900;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.West:
                    xUL = posCenter.X - 900;
                    yUL = posCenter.Y;

                    xUR = posCenter.X;
                    yUR = posCenter.Y;

                    xBR = posCenter.X;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X - 900;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.NorthWest:
                    xUL = posCenter.X - 900;
                    yUL = posCenter.Y - 900;

                    xUR = posCenter.X;
                    yUR = posCenter.Y;

                    xBR = posCenter.X;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X - 900;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.North:
                    xUL = posCenter.X - 20;
                    yUL = posCenter.Y - 900;

                    xUR = posCenter.X + 20;
                    yUR = posCenter.Y - 900;

                    xBR = posCenter.X + 20;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X - 20;
                    yBL = posCenter.Y + 900;
                    break;
                case Direction.NorthEast:
                    xUL = posCenter.X;
                    yUL = posCenter.Y;

                    xUR = posCenter.X + 900;
                    yUR = posCenter.Y - 900;

                    xBR = posCenter.X + 900;
                    yBR = posCenter.Y + 900;

                    xBL = posCenter.X;
                    yBL = posCenter.Y + 900;
                    break;
            }
            List<List<IntPoint>> polys = new List<List<IntPoint>>()
                {
                    new List<IntPoint>()
                        {
                            new IntPoint((int) xUL, (int) yUL),
                            new IntPoint((int) xUR, (int) yUR),
                            new IntPoint((int) xBR, (int) yBR),
                            new IntPoint((int) xBL, (int) yBL)
                        }
                };
            ProjectionHandler.RemoveRocketsPolysIntersectionWithPlanes(service, ref polys);
            return polys;
        }

        public List<List<IntPoint>> GetRopesPoly()
        {
            List<List<IntPoint>> allPolys = new List<List<IntPoint>>();
            foreach (var service in _engineState.SpringsManagerEngine.ListOfServices)
            {
                List<List<IntPoint>> ropePoly = GetRopePoly(service, _engineState);
                allPolys.AddRange(ropePoly);
            }
            return allPolys;
        }

        public static List<List<IntPoint>> GetRopePoly(SpringService service, EngineManager engineManager)
        {
            int verticalLineLength, halfFarWidth;
            SetProperHalfFarWidth(service, engineManager.CookieRB, out verticalLineLength, out halfFarWidth);
            var polySemiCircle = PolysHelper.GetShapeSemiCirclePoly(service.Masses[0].PositionXNACenter2D,
                                                                    ropeNearWidth,
                                                                    verticalLineLength,
                                                                    halfFarWidth);
            List<IntPoint> polyAdded = GetAddedGravityRectanglePoly(service, engineManager);
            var polyFinal = EntraSolver.GetPolySolution(polySemiCircle, polyAdded, ClipType.ctUnion);

            // Cut upper rectangle
            int cY = (int)StaticData.EngineManager.CookieRB.PositionXNACenter2D.Y;
            List<IntPoint> poly = new List<IntPoint>()
                {
                    new IntPoint(0, 0),
                    new IntPoint(StaticData.LevelFarWidth, 0),
                    new IntPoint(StaticData.LevelFarWidth, cY),
                    new IntPoint(0, cY),
                };

            polyFinal = EntraSolver.GetPolySolution(polyFinal, poly, ClipType.ctDifference);
            return polyFinal;
        }

        private static List<IntPoint> GetAddedGravityRectanglePoly(SpringService service, EngineManager engineManager)
        {
            List<IntPoint> polyAdded;
            Vector2 pinP = service.Masses[0].PositionXNACenter2D;
            Vector2 cPReal = engineManager.CookieRB.PositionXNACenter2D;
            //Vector2 cPImg = engineManager.CookieRB.PositionXNACenter2D;
            //cPImg.X = pinP.X - (cPReal.X - pinP.X);
            Vector2 pWithPlane1 = new Vector2(1000, 1000);

            IntersectionWithThreePlanes(pinP, cPReal, out pWithPlane1);

            Vector2 pRefWithPlane2, pWithPlane2, interP;
            int halfRectWidth = (int)Math.Abs(pWithPlane1.X - pinP.X);
            int rectWidth = halfRectWidth * 2;

            if (pinP.X > pWithPlane1.X)
            {
                pRefWithPlane2 = new Vector2(pWithPlane1.X + rectWidth, pWithPlane1.Y);
            }
            else
            {
                pRefWithPlane2 = new Vector2(pWithPlane1.X - rectWidth, pWithPlane1.Y);
            }
            IntersectionWithThreePlanes(pinP, pRefWithPlane2, out pWithPlane2);

            int rectLength;
            if (pWithPlane1.Y > pWithPlane2.Y)
            {
                interP = pWithPlane1;
                rectLength = (int)Math.Abs(interP.X - pWithPlane2.X);
            }
            else
            {
                interP = pWithPlane2;
                rectLength = (int)Math.Abs(interP.X - pWithPlane1.X);
            }
            

            if (pinP.X > interP.X)
            {
                return new List<IntPoint>()
                    {
                        new IntPoint((int)interP.X, (int)interP.Y),
                        new IntPoint((int)interP.X + rectLength, (int)interP.Y),
                        new IntPoint((int)interP.X + rectLength, (int)interP.Y + 900),
                        new IntPoint((int)interP.X, (int)interP.Y + 900)
                    };
            }
            else
            {
                return new List<IntPoint>()
                    {
                        new IntPoint((int)interP.X - rectLength, (int)interP.Y),
                        new IntPoint((int)interP.X, (int)interP.Y),
                        new IntPoint((int)interP.X, (int)interP.Y + 900),
                        new IntPoint((int)interP.X - rectLength, (int)interP.Y + 900)
                    };
            }


            //var poly = new ClockwiseSorter(pointsArea).Sort();
            //EntraDrawer.DrawIntoFile(new Polygons() { poly });
        }

        public static void IntersectionWithThreePlanes(Vector2 pinP, Vector2 cP, out Vector2 pWithPlane)
        {
            var pinCookieVec = Vector2.Subtract(cP, pinP);
            pinCookieVec.Normalize();
            
            pWithPlane  = new Vector2(1000,1000);
            for (int i = 2; i < 7; i += 2)
            {
                var planeDir = (Direction)i;
                Vector2 pP1, pP2, pInter;
                ProjectionHandler.GetPlanePoints(planeDir, out pP1, out pP2);
                MathHelperModule.FindIntersection(pinP, cP, pP1, pP2, out pInter);
                if (pInter.X >= 0 && pInter.X <= StaticData.LevelFarWidth
                    && pInter.Y >= 0 && pInter.Y <= StaticData.LevelFarHeight)
                {
                    // if in the same direction with the pin-cookie vector
                    //Vector2 vec = refP1 - pInter;
                    //vec.Normalize();
                    //if (MathHelperModule.GetAngleBetweenTwoVectors(pinCookieVec, vec) <= 90)
                    if (pinP.X < cP.X)
                    {
                        if (cP.X < 900)
                        {
                            if (pInter.X > cP.X)
                                pWithPlane = pInter;
                        }
                        else
                        {
                            if (pInter.X < cP.X)
                                pWithPlane = pInter;
                        }

                    }
                    else
                    {
                        if (cP.X > 0)
                        {
                            if (pInter.X < cP.X)
                                pWithPlane = pInter;
                        }
                        else
                        {
                            if (pInter.X > cP.X)
                                pWithPlane = pInter;
                        }
                    }
                }
            }
        }

        public static void SetProperHalfFarWidth(SpringService rope, CookieRB cookieRb,
            out int verticalLineLength,
            out int halfFarWidth)
        {
            var ropeVec = rope.Masses[0].PositionXNACenter2D;
            var cookieVec = cookieRb.PositionXNACenter2D;
            var hypotenuseVec = Vector2.Subtract(cookieVec, ropeVec);
            hypotenuseVec.Normalize();
            Vector2 verticalLine = new Vector2(0, 1);
            float angleRopeHead =
                MathHelper.ToDegrees(MathHelperModule.GetAngleBetweenTwoVectors(verticalLine, hypotenuseVec)) - 9;
            angleRopeHead = angleRopeHead < 0 ? 1 : angleRopeHead;
            float angleElse = 180 - 90 - angleRopeHead;
            int hypotenuseVecLength = 2000;
            verticalLineLength = (int)(hypotenuseVecLength * (angleElse / 90));
            halfFarWidth = (int)Math.Sqrt(Math.Pow(hypotenuseVecLength, 2) - Math.Pow(verticalLineLength, 2));
        }

        #region Old reachable functions
        //public Polygons GetReachBumpersPolys(Polygons allCompsPolys)
        //{
        //    Polygons allPolys = new Polygons();
        //    List<BumpRigid> bumpers = _engineState.RigidsManagerEngine.
        //        ListOfBoxRigids.Where(item => item is BumpRigid).Cast<BumpRigid>()
        //        .ToList();
        //    foreach (var service in bumpers)
        //    {
        //        Polygon poly = null;
        //        if (DefinitiveCompsPolysHandler.IsDefComponentIntersection(
        //            service.PositionXNACenter2D, service.Width, allCompsPolys))
        //            poly = GetBumperPoly(service);
        //        allPolys.Add(poly);
        //    }
        //    return allPolys;
        //}

        //public Polygons GetReachBlowersPolys(Polygons allCompsPolys)
        //{
        //    Polygons allPolys = new Polygons();
        //    foreach (var service in _engineState.BlowerManagerEngine.ListOfServices)
        //    {
        //        Polygon poly = GetBlowerPoly(service);
        //        if (EntraSolver.IsPolyOperation(new Polygons() { poly }, allCompsPolys, ClipType.ctIntersection))
        //        {
        //            allPolys.Add(poly);
        //        }
        //    }
        //    return allPolys;
        //}

        //public Polygons GetReachBubblesPolys(Polygons allCompsPolys)
        //{
        //    Polygons allPolys = new Polygons();
        //    foreach (var service in _engineState.BubbleManagerEngine.ListOfServices)
        //    {
        //        if (DefinitiveCompsPolysHandler.IsDefComponentIntersection(
        //            service.PositionXNACenter, service.Width, allCompsPolys))
        //        {
        //            Polygon poly = GetBubblePoly(service);
        //            allPolys.Add(poly);
        //        }
        //    }
        //    return allPolys;
        //}

        //public Polygons GetReachRocketsPolys(Polygons allCompsPolys)
        //{
        //    Polygons allPolys = new Polygons();
        //    foreach (var service in _engineState.RocketsCarrierManagerEngine.GetListOfServices())
        //    {
        //        if (DefinitiveCompsPolysHandler.IsDefComponentIntersection(
        //            service.PositionXNACenter2D, service.Width, allCompsPolys))
        //        {
        //            Polygon poly = GetRocketPoly(service);
        //            allPolys.Add(poly);
        //        }
        //    }
        //    return allPolys;
        //}
        #endregion

        public List<CompEntityPoly> GetReachServicePolys(List<List<IntPoint>> spaceSoFar, List<CompEntityPoly> entityList)
        {
            List<CompEntityPoly> allPolys = new List<CompEntityPoly>();
            //EntraDrawer.DrawIntoFileTesting(spaceSoFar);
            foreach (var entity in entityList)
            {
                //EntraDrawer.DrawIntoFileTesting(entity.GetDefPoly());
                //EntraDrawer.DrawIntoFile(entity.GetDefPoly());
                //EntraDrawer.DrawIntoFile(allCompsPolys);
                //var p = EntraSolver.GetPolySolution(entity.GetDefPoly(), allCompsPolys, ClipType.ctIntersection);
                //EntraDrawer.DrawIntoFile(p);
                if (EntraSolver.IsPolyOperation(entity.GetDefPoly(), spaceSoFar, ClipType.ctIntersection))
                {
                    allPolys.Add(entity);
                }
            }
            return allPolys;
        }

        public static List<List<IntPoint>> GetDefBlowerPoly(BlowerService blower)
        {
            //return PolysHelper.GetShapeSquarePoly(blower.PositionXNACenter, (blower.Height + 50)/2);
            var res = AreaCompPolyHandler.GetBlowerPoly((BlowerService) blower, StaticData.BlowerEffectAreaRadius);
            
            List<IntPoint> polyCut = new List<IntPoint>()
                {new IntPoint(-1000, (int)blower.PositionXNA.Y + StaticData.BlowerEffectAreaRadius),
                new IntPoint(+1000, (int)blower.PositionXNA.Y + StaticData.BlowerEffectAreaRadius),
                new IntPoint(+1000, 1000),
                new IntPoint(-1000, 1000),
                };

            return EntraSolver.GetPolySolution(res, polyCut, ClipType.ctDifference);
        }
    }
}