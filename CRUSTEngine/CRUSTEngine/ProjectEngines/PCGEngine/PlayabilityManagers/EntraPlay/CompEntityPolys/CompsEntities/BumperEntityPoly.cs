using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities
{
    [Serializable]
    public class BumperEntityPoly : CompEntityPoly
    {
        public override Vector2 PositionXNA2D
        {
            get { return ((BumpRigid)this.CompObj).PositionXNA2D; }
        }

        public override Vector2 PositionXNACenter2D
        {
            get { return ((BumpRigid)this.CompObj).PositionXNACenter2D; }
        }

        public List<IntPoint> DelPoly {
            get
            {
                return AreaCompPolyHandler.GetBumperDeletionPoly((BumpRigid)CompObj);
            }
        }

        private List<CompEntityPoly> _adders;
        public BumperEntityPoly(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple, object compObj)
            : base(entraAgentSimple, compObj)
        {
            _adders = new List<CompEntityPoly>();
        }

        public override List<List<IntPoint>> GetDefPoly()
        {
            return new List<List<IntPoint>>() {this.EntraAgentSimple.DefCompPolyHandler.GetDefBumperPoly((BumpRigid) CompObj)};
        }

        public override List<List<IntPoint>> GetAreaPoly()
        {
            return this.GetCoverageSoFar();
        }

        public List<List<IntPoint>> GetAreaPoly(CompEntityPoly adder)
        {
            var polys = new List<List<IntPoint>>();
            if (adder is BubbleEntityPoly)
            {
                BumpersHandler.AddBumpsAreaForBubble(ref polys, this, adder as BubbleEntityPoly);
            }
            else
            {
                polys = new List<List<IntPoint>>()
                    {
                        AreaCompPolyHandler.GetBumperPoly((BumpRigid) CompObj, adder.PositionXNACenter2D)
                    };
            }
            //EntraDrawer.DrawIntoFileTesting(polys);
            return polys;
        }

        public List<List<IntPoint>> ApplyEffectPref(List<List<IntPoint>> spaceSoFar, 
                                                    CompEntityPoly newAdder, 
                                                    bool withAdd = true)
        {
            return ApplyMe(spaceSoFar, new List<CompEntityPoly>() { newAdder }, withAdd);
        }

        public List<List<IntPoint>> ApplyEffectPref(List<List<IntPoint>> spaceSoFar,
                                              List<CompEntityPoly> newAdders,
                                              bool withAdd = true)
        {
            return ApplyMe(spaceSoFar, newAdders , withAdd);
        }

        public override List<List<IntPoint>> ApplyEffect(List<List<IntPoint>> spaceSoFar, CompEntityPoly newAdder)
        {
            return ApplyMe(spaceSoFar, new List<CompEntityPoly>(){newAdder}, true);
        }

        private List<List<IntPoint>> ApplyMe(List<List<IntPoint>> spaceSoFar,
                             List<CompEntityPoly> newAdders,
                             bool withAdd = true)
        {
            List<List<IntPoint>> result = spaceSoFar;
            EntraDrawer.DrawIntoFile(result);

            AddNewAdders(newAdders);

            var bumperDelPolys = this.GetDelPolys(_adders) ?? new List<List<IntPoint>>();
            EntraDrawer.DrawIntoFile(bumperDelPolys);

            ApplyDelPolysForAdders(ref result, bumperDelPolys);
            EntraDrawer.DrawIntoFile(result);

            ApplyAddPolysForAdders(ref result, bumperDelPolys, withAdd);
            EntraDrawer.DrawIntoFile(result);

            return result;
        }

        private void AddNewAdders(List<CompEntityPoly> newAdders)
        {
            foreach (CompEntityPoly newAdder in newAdders)
            {
                if (!_adders.Contains(newAdder) && newAdder != this)
                    _adders.Add(newAdder);
            }
        }

        private void ApplyAddPolysForAdders(ref List<List<IntPoint>> result, List<List<IntPoint>> bumperDelPolys,
            bool withAdd)
        {
            if (withAdd)
            {
                foreach (CompEntityPoly adder in _adders)
                {
                    var bumpAddPoly = this.GetAreaPoly(adder);
                    EntraDrawer.DrawIntoFile(bumpAddPoly);
                    bumpAddPoly = EntraSolver.GetPolySolution(bumpAddPoly, bumperDelPolys, ClipType.ctDifference);
                    AddToLog(bumpAddPoly, adder);
                    EntraDrawer.DrawIntoFile(bumpAddPoly);
                    result = EntraSolver.GetPolySolution(result, bumpAddPoly, ClipType.ctUnion);
                }
            }
        }

        private void ApplyDelPolysForAdders(ref List<List<IntPoint>> result, List<List<IntPoint>> bumperDelPolys)
        {
            // Fix log
            RemoveDelPolyFromEveryLogInSpace(bumperDelPolys, _adders);

            result = EntraSolver.GetPolySolution(result, bumperDelPolys, ClipType.ctDifference);
        }

        private void RemoveDelPolyFromEveryLogInSpace(List<List<IntPoint>> bumperDelPolys, List<CompEntityPoly> adders)
        {
            List<PolyLog> polys = this.EntraAgentSimple.PolysLogger.GetLog();
            foreach (PolyLog polyLog in polys)
            {
                foreach (CompEntityPoly adder in adders)
                {
                    if (polyLog.Comp == adder)
                    {
                        foreach (APPair apPair in polyLog.ApPairs)
                        {
                            apPair.Poly = EntraSolver.GetPolySolution(apPair.Poly, bumperDelPolys, ClipType.ctDifference);

                        }
                    }
                }
            }
        }

        private void AddToLog(List<List<IntPoint>> initialPoly, CompEntityPoly adder)
        {
            List<PolyLog> polys = this.EntraAgentSimple.PolysLogger.GetLog();
            APPair addedAPPair = null;
            foreach (PolyLog polyLog in polys)
            {
                if (polyLog.Comp == this)
                {
                    foreach (APPair apPair in polyLog.ApPairs)
                    {
                        if (apPair.AdderComp == adder)
                        {
                            addedAPPair = apPair;
                        }
                    }
                    if (addedAPPair != null)
                    {
                        polyLog.ApPairs.Remove(addedAPPair);
                    }
                }
            }
            this.EntraAgentSimple.PolysLogger.Log(new PolyLog(this, initialPoly, adder));
        }

        public List<List<IntPoint>> GetDelPolys(List<CompEntityPoly> compsToInspect)
        {
            return GetDelPolysOfMe(this, compsToInspect);
        }

        public List<List<IntPoint>> GetDelPolys(CompEntityPoly compsToInspect)
        {
            return GetDelPolysOfMe(this, new List<CompEntityPoly>() { compsToInspect });
        }

        public static List<List<IntPoint>> GetDelPolysOfMe(BumperEntityPoly me, List<CompEntityPoly> adders)
        {
            List<List<IntPoint>> allDels = new List<List<IntPoint>>();

            if (adders == null)
            {
                return allDels;
            }
            if (adders.Count == 0)
            {
                return allDels;
            }

            foreach (CompEntityPoly adder in adders)
            {
                if (adder != me)
                {
                    //if (!(compEntityPoly is BumperEntityPoly))
                    {
                        List<List<IntPoint>> newDelPolys = new List<List<IntPoint>>();

                        if (adder is BumperEntityPoly)
                        {
                            var adderBump = adder as BumperEntityPoly;
                            if (EntraSolver.IsPolyOperation(me.GetDefPoly(),
                                                            adderBump.GetAreaPoly(),
                                                            ClipType.ctIntersection))
                            {
                                newDelPolys =
                                    me.EntraAgentSimple.ProjectionHandler.ProjectCompOntoBumper(
                                        adderBump.PositionXNACenter2D, me);
                                EntraDrawer.DrawIntoFile(newDelPolys);
                            }
                        }
                        else
                        {
                            if (EntraSolver.IsPolyOperation(me.GetDefPoly(),
                                                            adder.GetAreaPoly(), ClipType.ctIntersection))
                            {
                                newDelPolys =
                                    me.EntraAgentSimple.ProjectionHandler.ProjectCompOntoBumper(
                                        adder.PositionXNACenter2D, me);
                                EntraDrawer.DrawIntoFile(newDelPolys);

                                if (adder is RocketEntityPoly)
                                {
                                    var rocket = adder as RocketEntityPoly;
                                    ProjectionHandler.ReAddRocketTrajectoryMissingAreas(
                                        rocket,
                                        me, ref newDelPolys);
                                    var dir = (rocket.CompObj as RocketCarrierService).Dir;
                                    if (dir == Direction.North ||
                                        dir == Direction.NorthEast ||
                                        dir == Direction.NorthWest)
                                    {
                                        newDelPolys = new List<List<IntPoint>>();
                                    }
                                }
                                else
                                {
                                    if (adder is RopeEntityPoly)
                                    {
                                        List<BumperEntityPoly> allBumps =
                                            me.EntraAgentSimple.AllCompsEntities.FindAll(delegate(CompEntityPoly obj)
                                                { return obj is BumperEntityPoly; })
                                                   .ConvertAll(input => input as BumperEntityPoly);

                                        List<List<BumperEntityPoly>> groups =
                                            BumpersHandler.GroupizeCloseBumpers(allBumps);

                                        if (IsBumperCloseToAnother(me, allBumps))
                                        {
                                            if (IsOnCutSide(me, adder as RopeEntityPoly, groups) || groups.Count == 0)
                                            {
                                                ProjectionHandler.ReAddBlowerBubbleRopeTrajectoryMissingAreas(
                                                    adder.PositionXNACenter2D,
                                                    me, ref newDelPolys);
                                            }
                                            else

                                            {
                                                var bumperCutSide = FindCutSideBumper(me,
                                                                                      adder as
                                                                                      RopeEntityPoly,
                                                                                      groups);
                                                if (bumperCutSide != null)
                                                {
                                                    ProjectionHandler.ReAddInverseCutPolygon(
                                                        adder.PositionXNACenter2D,
                                                        bumperCutSide, ref newDelPolys);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ProjectionHandler.ReAddBlowerBubbleRopeTrajectoryMissingAreas(
                                                adder.PositionXNACenter2D,
                                                me, ref newDelPolys);
                                        }
                                    }
                                    else
                                    {
                                        ProjectionHandler.ReAddBlowerBubbleRopeTrajectoryMissingAreas(
                                            adder.PositionXNACenter2D,
                                            me, ref newDelPolys);
                                    }
                                }
                            }
                        }
                        EntraDrawer.DrawIntoFile(newDelPolys);
                        allDels.AddRange(newDelPolys);
                        //result = BumpersHandler.GetDelPolysIntersection(result);
                        //if (result == null)
                        //{
                        //    result = new List<List<IntPoint>>(); 
                        //}
                    }
                }
            }
            allDels = BumpersHandler.GetDelPolysIntersection(allDels) ?? new List<List<IntPoint>>();
            return allDels;
        }

        private static BumperEntityPoly FindCutSideBumper(BumperEntityPoly bumpRef,  RopeEntityPoly ropeEntityPoly,
            List<List<BumperEntityPoly>> groups)
        {
            var posRope = ropeEntityPoly.PositionXNACenter2D;
            var posBump = bumpRef.PositionXNACenter2D;
            foreach (List<BumperEntityPoly> group in groups)
            {
                if (group.Contains(bumpRef))
                {
                    if (posRope.X < posBump.X)
                    {
                        return group[group.Count - 1];
                    }
                    else
                    {
                        return group[0];
                    }
                }
            }
            return null;
        }

        private static bool IsOnCutSide(BumperEntityPoly bumpRef, RopeEntityPoly ropeEntityPoly,
            List<List<BumperEntityPoly>> groups)
        {
            var posRope = ropeEntityPoly.PositionXNACenter2D;
            var posBump = bumpRef.PositionXNACenter2D;
            foreach (List<BumperEntityPoly> group in groups)
            {
                if (posRope.X < posBump.X)
                {
                    if (bumpRef == group[group.Count - 1])
                    {
                        return true;
                    }
                }
                else
                {
                    if (bumpRef == group[0])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsBumperCloseToAnother(BumperEntityPoly bumperRef, List<BumperEntityPoly> allBumps)
        {
            foreach (BumperEntityPoly bumper in allBumps)
            {
                if (bumper != bumperRef)
                {
                    int dis = (int)Math.Abs(bumperRef.PositionXNACenter2D.X - bumper.PositionXNACenter2D.X);
                    if (dis < 100)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
