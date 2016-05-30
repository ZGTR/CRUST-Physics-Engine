using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.HelperModules;

using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers
{
    [Serializable]
    public class ProjectionHandler
    {
        private readonly CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple _entraAgentSimple;

        public ProjectionHandler(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple)
        {
            _entraAgentSimple = entraAgentSimple;
        }

        public List<List<IntPoint>> ProjectCompOntoBumper(Vector2 compPos, BumperEntityPoly bumperEntity)
        {
            Vector2 bP1, bP2;
            FindBumperTwoBorderPoints(bumperEntity, out bP1, out bP2);
            
            // Intersection with all planes
            var result = IntersectWithPlanes(compPos, bP1, bP2);
            return result;
        }

        private List<List<IntPoint>> IntersectWithPlanes(Vector2 compPos, Vector2 rP1, Vector2 rP2)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();
            Vector2 rPosCenter = new Vector2((rP1.X + rP2.X)/ 2, (rP1.Y + rP2.Y)/2);
            for (int i = 0; i < 7; i += 2)
            {
                var planeDir = (Direction)i;
                if (CompIsOnDirToBump(compPos, rPosCenter, planeDir))
                {
                    Vector2 planeP1, planeP2;
                    GetPlanePoints(planeDir, out planeP1, out planeP2);
                    List<Vector2> shadowPoints = GetIntersection(compPos, rP1, rP2, planeP1, planeP2);
                    
                    SetCenterPoint(shadowPoints);
                    shadowPoints.Sort(ClockwiseSorter);
                    //if (IsGoodPoly(compPos, cDir))
                    var poly = PolysHelper.BuildPolygon(shadowPoints);
                    EntraDrawer.DrawIntoFile(new List<List<IntPoint>>(){poly});
                    result.Add(poly);
                }
            }
            return result;
        }

        private static bool CompIsOnDirToBump(Vector2 compPos, Vector2 refPosCenter, Direction planeDir)
        {
            switch (planeDir)
            {
                case Direction.East:    // Up
                    return compPos.Y - 20 > refPosCenter.Y;
                    break;
                case Direction.South:   // Right
                    return compPos.X + 20 < refPosCenter.X;
                    break;
                case Direction.West:    // Bottom
                    return compPos.Y + 20< refPosCenter.Y;
                    break;
                case Direction.North:   // Left
                    return compPos.X > refPosCenter.X + 20;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private void SetCenterPoint(List<Vector2> shadowPoints)
        {
            float avgX = shadowPoints.Average(p => p.X);
            float avgY = shadowPoints.Average(p => p.Y);
            center.X = avgX;
            center.Y = avgY;
        }

        private Vector2 center;
        public int ClockwiseSorter(Vector2 a, Vector2 b)
        {
            if (a.X- center.X>= 0 && b.X- center.X< 0)
                return 1;
            if (a.X- center.X== 0 && b.X- center.X== 0)
            {
                if (a.Y - center.Y >= 0 || b.Y - center.Y >= 0)
                    return 1;// a.Y > b.Y;
                return -1; //b.Y > a.Y;
            }

            // compute the cross product of vectors (center -> a) x (center -> b)
            int det = (int)((a.X- center.X) * (b.Y - center.Y) - (b.X- center.X) * (a.Y - center.Y));
            if (det < 0)
                return 1;
            if (det > 0)
                return -1;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            int d1 = (int)((a.X- center.X) * (a.X- center.X) + (a.Y - center.Y) * (a.Y - center.Y));
            int d2 = (int)((b.X- center.X) * (b.X- center.X) + (b.Y - center.Y) * (b.Y - center.Y));
            return 1;// d1 > d2;
        }

        public static void GetPlanePoints(Direction dirPlane, out Vector2 plane1, out Vector2 plane2)
        {
            switch (dirPlane)
            {
                case Direction.East: // Up
                    plane1 = new Vector2(0, 0);
                    plane2 = new Vector2(StaticData.LevelFarWidth, 0);
                    break;
                case Direction.South: // Right
                    plane1 = new Vector2(StaticData.LevelFarWidth, 0);
                    plane2 = new Vector2(StaticData.LevelFarWidth, StaticData.LevelFarHeight);
                    break;
                case Direction.West: // Bottom
                    plane1 = new Vector2(0, StaticData.LevelFarHeight);
                    plane2 = new Vector2(StaticData.LevelFarWidth, StaticData.LevelFarHeight);
                    break;
                case Direction.North: // Left
                    plane1 = new Vector2(0, 0);
                    plane2 = new Vector2(0, StaticData.LevelFarHeight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
        }

        private List<Vector2> GetIntersection(Vector2 compPos, Vector2 bP1, Vector2 bP2,
            Vector2 planeP1, Vector2 planeP2)
        {
            Vector2 pInter1, pInter2;
            MathHelperModule.FindIntersection(compPos, bP1, planeP1, planeP2, out pInter1);
            MathHelperModule.FindIntersection(compPos, bP2, planeP1, planeP2, out pInter2);
            List<Vector2> shadowPoints = new List<Vector2>() { bP1, bP2, pInter1, pInter2 };
            return shadowPoints;
        }

        private int PolygonsPointSorter(Vector2 p1, Vector2 p2)
        {
            if (p1.X> p2.X)
            {
                return 1;
            }
            else
            {
                if (p1.X== p2.X)
                {
                    return 0;
                }
                else
                {
                    if (p2.Y > p1.Y)
                    {
                        return 1;
                    }
                    else
                    {
                        if (p2.Y == p1.Y)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            return -1;
        }

        private int PolygonsPointSorterOnY(Vector2 p1, Vector2 p2)
        {
            if (p1.Y > p2.Y)
            {
                return 1;
            }
            else
            {
                if (p1.Y == p2.Y)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static void FindBumperTwoBorderPoints(BumperEntityPoly bumperEntity, out Vector2 pb1, out Vector2 pb2)
        {
            BumpRigid bump = (bumperEntity.CompObj as BumpRigid);
            Vector2 bXNA = bump.PositionXNA2D;
            Vector2 bXNACenter = bump.PositionXNACenter2D;
            int halfBumpWidth = bump.Width / 2 - 5;

            switch (bump.Dir)
            {
                case Direction.East:
                case Direction.West:
                    pb1 = bXNA + new Vector2(0, bump.Height / 2);
                    pb2 = bXNA + new Vector2(bump.Width, bump.Height / 2);
                    break;
                case Direction.SouthEast:
                case Direction.NorthWest:
                    pb1 = bXNACenter + new Vector2(-halfBumpWidth, -halfBumpWidth);
                    pb2 = bXNACenter + new Vector2(+halfBumpWidth, +halfBumpWidth);
                    break;
                case Direction.South:
                case Direction.North:
                    pb1 = bXNACenter + new Vector2(0, -halfBumpWidth);
                    pb2 = bXNACenter + new Vector2(0, bump.Width - halfBumpWidth);
                    break;
                case Direction.SouthWest:
                case Direction.NorthEast:
                    pb1 = bXNACenter + new Vector2(-halfBumpWidth, +halfBumpWidth);
                    pb2 = bXNACenter + new Vector2(+halfBumpWidth, -halfBumpWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void FindRocketTwoBorderPoints(RocketEntityPoly rocketEntity, out Vector2 p1, out Vector2 p2)
        {
            RocketCarrierService rocket = (rocketEntity.CompObj as RocketCarrierService);
            FindRocketTwoBorderPoints(rocket, out p1, out p2);
        }

        public static void FindRocketTwoBorderPoints(RocketCarrierService rocket, out Vector2 p1, out Vector2 p2)
        {
            Vector2 bXNA = rocket.PositionXNA2D;
            Vector2 bXNACenter = rocket.PositionXNACenter2D;
            int halfWidth = rocket.Width / 2 - 5;

            switch (rocket.Dir)
            {
                case Direction.West:
                case Direction.East:
                    p1 = bXNA + new Vector2(0, rocket.Height / 2);
                    p2 = bXNA + new Vector2(rocket.Width, rocket.Height / 2);
                    break;
                case Direction.NorthWest:
                case Direction.SouthEast:
                    p1 = bXNACenter + new Vector2(-halfWidth, -halfWidth);
                    p2 = bXNACenter + new Vector2(+halfWidth, +halfWidth);
                    break;
                case Direction.North:
                case Direction.South:
                    p1 = bXNACenter + new Vector2(0, -halfWidth);
                    p2 = bXNACenter + new Vector2(0, rocket.Width - halfWidth);
                    break;
                case Direction.NorthEast:
                case Direction.SouthWest:
                    p1 = bXNACenter + new Vector2(-halfWidth, +halfWidth);
                    p2 = bXNACenter + new Vector2(+halfWidth, -halfWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void RemoveRocketsPolysIntersectionWithPlanes(RocketCarrierService rocket,
            ref List<List<IntPoint>> poly)
        {
            Vector2 rP1, rP2;
            ProjectionHandler.FindRocketTwoBorderPoints(rocket, out rP1, out rP2);

            Vector2 rPosCenter = new Vector2((rP1.X + rP2.X)/2, (rP1.Y + rP2.Y)/2);
            for (int i = 0; i < 7; i += 2)
            {
                var cDir = (Direction)i;
                //if (CompIsOnDirToBump(compPos, rPosCenter, cDir))
                //{
                    Vector2 planeP1, planeP2;
                    GetPlanePoints(cDir, out planeP1, out planeP2);

                    Vector2 interP;
                    MathHelperModule.FindIntersection(planeP1, planeP2, rP1, rP2, out interP);

                    if (interP.X <= StaticData.LevelFarWidth && interP.X >= 0
                        && interP.Y <= StaticData.LevelFarHeight && interP.Y >= 0)
                    {
                        // Rocket is not on a collision course with the bump, 
                        // so cut down the del poly area vertically
                        List<IntPoint> polyInverseCutVertical = GetInverseCutPolygon(interP, rocket.PositionXNACenter2D);

                        EntraDrawer.DrawIntoFile(new List<List<IntPoint>>() {polyInverseCutVertical});

                        poly = EntraSolver.GetPolySolution(poly, polyInverseCutVertical,
                                                                ClipType.ctIntersection);
                        EntraDrawer.DrawIntoFile(poly);
                    }
               // }
            }
        }

        public static void ReAddRocketTrajectoryMissingAreas(RocketEntityPoly rocketEntityPoly,
                                                             BumperEntityPoly bumperEntity,
                                                             ref List<List<IntPoint>> delPolys)
        {
            var bumpRigid = bumperEntity.CompObj as BumpRigid;
            var bumpPosXNA = bumpRigid.PositionXNA2D;
            var bumpPosCenter = bumpRigid.PositionXNACenter2D;

            Vector2 bP1, bP2;
            ProjectionHandler.FindBumperTwoBorderPoints(bumperEntity, out bP1, out bP2);
            Vector2 rP1, rP2;
            ProjectionHandler.FindRocketTwoBorderPoints(rocketEntityPoly, out rP1, out rP2);

            Vector2 interP;
            MathHelperModule.FindIntersection(bP1, bP2, rP1, rP2, out interP);

            Vector2 cutP = GetBumperCutStartingPoint(bumpRigid.Dir, bP1, bP2,
                (rocketEntityPoly.CompObj as RocketCarrierService).PositionXNACenter2D);
            if (!RigidsHelperModule.IsCloseEnough(interP,
                (bumperEntity.CompObj as BumpRigid).PositionXNACenter2D,
                20))
            // if the bump is not on the collision course with the rocket
            {
                // Rocket is not on a collision course with the bump, so cut down the del poly area vertically
                List<IntPoint> polyInverseCutVertical = GetInverseCutPolygon(cutP, rocketEntityPoly); 

                EntraDrawer.DrawIntoFile(new List<List<IntPoint>>(){polyInverseCutVertical});

                delPolys = EntraSolver.GetPolySolution(delPolys, polyInverseCutVertical, ClipType.ctIntersection);
                EntraDrawer.DrawIntoFile(delPolys);
            }
        }

        public static void ReAddBlowerBubbleRopeTrajectoryMissingAreas(Vector2 posComp,
                                                             BumperEntityPoly bumperEntity,
                                                             ref List<List<IntPoint>> delPolys)
        {
            var bumpRigid = bumperEntity.CompObj as BumpRigid;
            var bumpPosXNA = bumpRigid.PositionXNA2D;
            var bumpPosCenter = bumpRigid.PositionXNACenter2D;

            Vector2 bP1, bP2;
            ProjectionHandler.FindBumperTwoBorderPoints(bumperEntity, out bP1, out bP2);
            Vector2 cP1 = posComp, cP2 = posComp;

            Vector2 interP;
            MathHelperModule.FindIntersection(bP1, bP2, cP1, cP2, out interP);

            Vector2 cutP = GetBumperCutStartingPoint(bumpRigid.Dir, bP1, bP2, posComp);
            if (!RigidsHelperModule.IsCloseEnough(interP,
                (bumperEntity.CompObj as BumpRigid).PositionXNACenter2D,
                20))
            {
                List<IntPoint> polyInverseCutVertical = GetInverseCutPolygon(cutP, posComp);
                EntraDrawer.DrawIntoFile(new List<List<IntPoint>>() { polyInverseCutVertical });
                EntraDrawer.DrawIntoFile(delPolys);

                delPolys = EntraSolver.GetPolySolution(delPolys, polyInverseCutVertical, ClipType.ctIntersection);
                EntraDrawer.DrawIntoFile(delPolys);
            }
        }

        public static void ReAddInverseCutPolygon(Vector2 posComp, BumperEntityPoly bumperEntity, ref List<List<IntPoint>> delPolys)
        {
            var bumpRigid = bumperEntity.CompObj as BumpRigid;
            var bumpPosXNA = bumpRigid.PositionXNA2D;
            var bumpPosCenter = bumpRigid.PositionXNACenter2D;

            Vector2 bP1, bP2;
            ProjectionHandler.FindBumperTwoBorderPoints(bumperEntity, out bP1, out bP2);
            Vector2 cP1 = posComp, cP2 = posComp;

            Vector2 interP;
            MathHelperModule.FindIntersection(bP1, bP2, cP1, cP2, out interP);

            Vector2 cutP = GetBumperCutStartingPoint(bumpRigid.Dir, bP1, bP2, posComp);
            if (!RigidsHelperModule.IsCloseEnough(interP,
                (bumperEntity.CompObj as BumpRigid).PositionXNACenter2D,
                20))
            // if the bump is not on the collision course with the rocket
            {
                // Rocket is not on a collision course with the bump, so cut down the del poly area vertically
                List<IntPoint> polyInverseCutVertical = GetInverseCutPolygon(cutP, posComp);
                delPolys = EntraSolver.GetPolySolution(delPolys, polyInverseCutVertical, ClipType.ctIntersection);
            }
        }

        private static List<IntPoint> GetInverseCutPolygon(Vector2 cutP, RocketEntityPoly rocketEntityPoly)
        {
            RocketCarrierService rocket = rocketEntityPoly.CompObj as RocketCarrierService;
            return GetInverseCutPolygon(cutP, rocket.PositionXNACenter2D);
        }

        private static List<IntPoint> GetInverseCutPolygon(Vector2 cutP, Vector2 rPos)
        {
            if (cutP.X > rPos.X)
            {
                return  new List<IntPoint>()
                    {
                        new IntPoint(0, 0),
                        new IntPoint((int) cutP.X, 0),
                        new IntPoint((int) cutP.X, 1000),
                        new IntPoint(0, 1000)
                    };
            }
            else
            {
                return new List<IntPoint>()
                    {
                        new IntPoint((int) cutP.X, 0),
                        new IntPoint((int) cutP.X + 1000, 0),
                        new IntPoint((int) cutP.X + 1000, 1000),
                        new IntPoint((int) cutP.X, 1000)
                    };
            }
        }

        public static Vector2 GetBumperCutStartingPoint(Direction dir, Vector2 bP1, Vector2 bP2, 
            Vector2 rocketPosCenter)
        {
            //if ((rocketPosCenter - bP1).Length() > (rocketPosCenter - bP2).Length())
            //{
            //    return bP1;
            //}
            if (Math.Abs(rocketPosCenter.X - bP1.X) > Math.Abs(rocketPosCenter.X - bP2.X))
            {
                return bP1;
            }
            return bP2;
            //switch (dir)
            //{
            //    case Direction.East:
            //        return bP2;
            //        break;
            //    case Direction.SouthEast:
            //        return bP1;
            //        break;
            //    case Direction.South:
            //        return bP2;
            //        break;
            //    case Direction.SouthWest:
            //        return bP2;
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }
    }
}
