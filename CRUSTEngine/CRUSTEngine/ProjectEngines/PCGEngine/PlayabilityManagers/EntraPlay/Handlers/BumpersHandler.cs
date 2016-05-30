using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers
{
    [Serializable]
    public class BumpersHandler
    {
        private readonly CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple _entraAgentSimple;
        //public List<BumperEntityPoly> processedBumps;
        private bool firstTime = true;

        public BumpersHandler(CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.EntraAgentSimple entraAgentSimple)
        {
            _entraAgentSimple = entraAgentSimple;
            //processedBumps = new List<BumperEntityPoly>();
        }

        public List<List<IntPoint>> SetProperBumpersEffect(List<List<IntPoint>> spaceSoFar,
            List<CompEntityPoly> adders, List<CompEntityPoly> allEntitiesList, bool withAdd)
        {
            //foreach (CompEntityPoly adder in adders)
            {
                List<BumperEntityPoly> processedBumps = new List<BumperEntityPoly>();
                while (true)
                {
                    List<BumperEntityPoly> allRechableBumpers = GetReachableBumpers(spaceSoFar, allEntitiesList);
                    allRechableBumpers.Sort(BumpersComparator);
                    List<BumperEntityPoly> rBumpersNew = GetNewBumps(allRechableBumpers, processedBumps);
                    rBumpersNew.Sort(BumpersComparator);

                    //foreach (BumperEntityPoly rBump in rBumpersNew)
                    {
                        if (rBumpersNew.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            BumperEntityPoly rBumper = //rBump;// 
                                rBumpersNew.First();
                            processedBumps.Add(rBumper);

                            spaceSoFar = rBumper.ApplyEffectPref(spaceSoFar, adders, withAdd);
                            //EntraDrawer.DrawIntoFileTesting(spaceSoFar);
                        }
                    }
                }
            }
            return spaceSoFar;
        }

        //public List<List<IntPoint>> SetProperBumpersEffectWithRopes(List<List<IntPoint>> spaceSoFar,
        //    List<CompEntityPoly> allEntitiesList)
        //{
        //    List<List<IntPoint>> bumpersDelPolys = new List<List<IntPoint>>();
        //    List<BumperEntityPoly> allRechableBumpers = new List<BumperEntityPoly>();
        //    List<BumperEntityPoly>  processedBumps = new List<BumperEntityPoly>();

        //    while (true)
        //    {
        //        allRechableBumpers = GetReachableBumpers(spaceSoFar, allEntitiesList);
        //        allRechableBumpers.Sort(BumpersComparator);

        //        EntraDrawer.DrawIntoFile(spaceSoFar);

        //        List<BumperEntityPoly> rBumpersNew = GetNewBumps(allRechableBumpers, processedBumps);
        //        rBumpersNew.Sort(BumpersComparator);
        //        //EntraDrawer.DrawIntoFile(new Polygons(){rBumpersNew.First().DelPoly});

        //        if (rBumpersNew.Count == 0)
        //        {
        //            break;
        //        }

        //        BumperEntityPoly rBumper = rBumpersNew.First();
        //        processedBumps.Add(rBumper);
        //        {
        //            // Temp for debug
        //            var bumpR = (rBumper.CompObj as BumpRigid);
        //            Vector2 pos = bumpR.PositionXNA2D;

        //            // Get bumper del polys
        //            List<List<IntPoint>> ropesBumpDelPolys = rBumper.GetDelPolys(spaceSoFar,
        //                                                                      this._entraAgentSimple.ropesEntityList);
        //            List<List<IntPoint>> delIntersection = GetDelPolysIntersection(ropesBumpDelPolys);
        //            EntraDrawer.DrawIntoFile(delIntersection);

        //            if (delIntersection != null)
        //                if (delIntersection.Count > 0)
        //                    spaceSoFar = EntraSolver.GetPolySolution(spaceSoFar, delIntersection,
        //                                                             ClipType.ctDifference);

        //            EntraDrawer.DrawIntoFile(spaceSoFar);
        //        }
        //    }
        //    return spaceSoFar;
        //}

        //public List<List<IntPoint>> SetProperBumpersEffect(List<List<IntPoint>> spaceSoFar,
        //    List<CompEntityPoly> allEntitiesList)
        //{
        //    List<List<IntPoint>> bumpersDelPolys = new List<List<IntPoint>>();
        //    List<BumperEntityPoly> allRechableBumpers = new List<BumperEntityPoly>();


        //    while (true)
        //    {
        //        allRechableBumpers = GetReachableBumpers(spaceSoFar, allEntitiesList);
        //        allRechableBumpers.Sort(BumpersComparator);

        //        EntraDrawer.DrawIntoFile(spaceSoFar);

        //        List<BumperEntityPoly> rBumpersNew = GetNewBumps(allRechableBumpers, processedBumps);
        //        rBumpersNew.Sort(BumpersComparator);
        //        //EntraDrawer.DrawIntoFile(new Polygons(){rBumpersNew.First().DelPoly});

        //        if (rBumpersNew.Count == 0)
        //        {
        //            break;
        //        }

        //        BumperEntityPoly rBumper = rBumpersNew.First();
        //        processedBumps.Add(rBumper);
        //        {
        //            // Temp for debug
        //            var bumpR = (rBumper.CompObj as BumpRigid);
        //            Vector2 pos = bumpR.PositionXNA2D;

        //            // Get bumper del polys
        //            var bumperDelPolys = new List<List<IntPoint>>();
        //            //List<List<IntPoint>> ropesBumpDelPolys = rBumper.GetDelPolys(spaceSoFar,
        //            //                                                             this._entraAgentSimple.ropesEntityList);

        //            List<List<IntPoint>> compsBumpDelPolys = rBumper.GetDelPolys(spaceSoFar,
        //                                                                         this._entraAgentSimple.rCompsAll);

        //            bumperDelPolys.AddRange(compsBumpDelPolys);
        //            bumperDelPolys.AddRange(compsBumpDelPolys);
                    

        //            //EntraDrawer.DrawIntoFile(ropesBumpDelPolys);
        //            EntraDrawer.DrawIntoFile(compsBumpDelPolys);
        //            EntraDrawer.DrawIntoFile(bumperDelPolys);

        //            // Get bumper del polys intersection
        //            //List<List<IntPoint>> delRopeIntersection = GetDelPolysIntersection(ropesBumpDelPolys);
        //            //List<List<IntPoint>> delCompsIntersection = GetDelPolysIntersection(compsBumpDelPolys);
        //            List<List<IntPoint>> delIntersection = GetDelPolysIntersection(bumperDelPolys);

        //            //Polygons delIntersection = new Polygons();
        //            ////if (delRopeIntersection != null)
        //            ////    delIntersection.AddRange(delRopeIntersection);
        //            //if (delCompsIntersection != null)
        //            //    delIntersection.AddRange(delCompsIntersection);

        //            //// Remove del polys intersection from all polys
        //            //if (delIntersection != null)
        //            //    if (delIntersection.Count > 0)
        //            //        spaceSoFar = EntraSolver.GetPolySolution(spaceSoFar, delIntersection,
        //            //                                                 ClipType.ctDifference);

        //            //delIntersection = GetDelPolysIntersection(compsBumpDelPolys);
        //            //EntraDrawer.DrawIntoFile(delIntersection);

        //            //EntraDrawer.DrawIntoFile(spaceSoFar);

        //            //// Remove del polys intersection from all polys
        //            if (delIntersection != null)
        //                if (delIntersection.Count > 0)
        //                    spaceSoFar = EntraSolver.GetPolySolution(spaceSoFar, delIntersection,
        //                                                             ClipType.ctDifference);

        //            EntraDrawer.DrawIntoFile(spaceSoFar);

        //            bumpersDelPolys.AddRange(bumperDelPolys);
        //        }
        //    }

        //    //List<List<IntPoint>> delIntersectionBetweenBumpers = GetDelPolysIntersection(bumpersDelPolys);

        //    // Remove all bumpers' del polys intersection from all polys
        //    //if (delIntersectionBetweenBumpers != null)
        //    //    if (delIntersectionBetweenBumpers.Count > 0)
        //    //        spaceSoFar = EntraSolver.GetPolySolution(spaceSoFar, delIntersectionBetweenBumpers,
        //    //                                                 ClipType.ctDifference);


        //    // GroupFix 
        //    //List<BumperEntityPoly> bumpersOfGroups;
        //    //spaceSoFar = ApplyCloseBumpersFix(rBumpers, spaceSoFar, out bumpersOfGroups);

        //    // Re Apply Individuals
        //    //List<BumperEntityPoly> individualsBumpers =
        //    //    rBumpers.FindAll(delegate(BumperEntityPoly b) { return !bumpersOfGroups.Contains(b); });
        //    //foreach (BumperEntityPoly rBumper in individualsBumpers)
        //    //{
        //    //    spaceSoFar = rBumper.ApplyEffect(spaceSoFar);
        //    //}
        //    return spaceSoFar;
        //}

        public List<BumperEntityPoly> GetReachableBumpers(List<List<IntPoint>> spaceSoFar,
                                                          List<CompEntityPoly> allEntitiesList)
        {
            var rCompsAllNew = _entraAgentSimple.AreaCompPolyHandler.GetReachServicePolys(spaceSoFar, allEntitiesList);
            var rBumpers = rCompsAllNew.Where(comp => comp is BumperEntityPoly).Cast<BumperEntityPoly>().ToList();

            return rBumpers;
        }

        public List<BumperEntityPoly> GetReachableBumpers(List<List<IntPoint>> spaceSoFar,
                                                  CompEntityPoly adder)
        {
            var rCompsAllNew = _entraAgentSimple.AreaCompPolyHandler.GetReachServicePolys(spaceSoFar, new List<CompEntityPoly>() {adder});
            var rBumpers = rCompsAllNew.Where(comp => comp is BumperEntityPoly).Cast<BumperEntityPoly>().ToList();

            return rBumpers;
        }

        private List<BumperEntityPoly> GetNewBumps(List<BumperEntityPoly> allRechableBumps, 
                                                    List<BumperEntityPoly> processedBumps)
        {
            List<BumperEntityPoly> newBumps = new List<BumperEntityPoly>();
            if (processedBumps.Count > 0)
            {
                foreach (BumperEntityPoly reachableBump in allRechableBumps)
                {
                    var obj = processedBumps.Find(b => b.PositionXNA2D == reachableBump.PositionXNA2D);
                    if (obj == null)
                    {
                        if (!newBumps.Contains(reachableBump))
                            newBumps.Add(reachableBump);
                    }
                }
            }
            else
            {
                newBumps = allRechableBumps;
            }
            return newBumps;
        }

        //private List<List<IntPoint>> GetDelPolysIntersection(List<List<IntPoint>> allDelPolys)
        //{
        //    if (allDelPolys.Count > 0)
        //    {
        //        List<List<IntPoint>> interPoly = new List<List<IntPoint>>();
        //        var first = allDelPolys.First();
        //        if (allDelPolys.Count == 1)
        //        {
        //            return new List<List<IntPoint>>() { first };
        //        }
        //        else
        //        {
        //            allDelPolys.RemoveAt(0);
        //            //EntraDrawer.DrawIntoFile(new Polygons(){first});
        //            //EntraDrawer.DrawIntoFile(allDelPolys);
        //            var unions = EntraSolver.GetPolyUnions(allDelPolys);
        //            interPoly = EntraSolver.GetPolySolution(first, unions, ClipType.ctIntersection);
        //            return interPoly;
        //        }
        //    }
        //    return null;
        //}

        public static List<List<IntPoint>> GetDelPolysIntersection(List<List<IntPoint>> allDelPolys)
        {
            if (allDelPolys.Count > 1)
            {
                List<List<IntPoint>> interPoly = new List<List<IntPoint>>();
                var first = allDelPolys.First();
                allDelPolys.RemoveAt(0);
                var unions = EntraSolver.GetPolyUnions(allDelPolys);
                interPoly = EntraSolver.GetPolySolution(first, unions, ClipType.ctIntersection);
                return interPoly;
            }
            else
            {
                if (allDelPolys.Count == 1)
                {
                    return new List<List<IntPoint>>() {allDelPolys.First()};
                }
                else
                {
                    return null;       
                }
            }
        }

        public List<List<IntPoint>> ApplyCloseBumpersFix(List<BumperEntityPoly> rBumpers, List<List<IntPoint>> spaceSoFar,
            out List<BumperEntityPoly> bumpersOfGroups)
        {
            bumpersOfGroups = new List<BumperEntityPoly>();
            List<List<BumperEntityPoly>> closeGroups = GroupizeCloseBumpers(rBumpers);
            foreach (List<BumperEntityPoly> group in closeGroups)
            {
                foreach (BumperEntityPoly bumper in group)
                {
                    spaceSoFar = EntraSolver.GetPolySolution(spaceSoFar, ((BumperEntityPoly)bumper).DelPoly, ClipType.ctDifference);
                }
            }
            List<BumperEntityPoly> groups = new List<BumperEntityPoly>();
            closeGroups.ForEach(groups.AddRange);
            bumpersOfGroups = groups;
            return spaceSoFar;
        }

        public static List<List<BumperEntityPoly>> GroupizeCloseBumpers(List<BumperEntityPoly> rBumpers)
        {
            List<List<BumperEntityPoly>> result = new List<List<BumperEntityPoly>>();
            foreach (BumperEntityPoly bumper1 in rBumpers)
            {
                List<BumperEntityPoly> listOfCloseToBumper1 = new List<BumperEntityPoly>() { bumper1 };
                foreach (BumperEntityPoly bumper2 in rBumpers)
                {
                    if (bumper1 != bumper2)
                    {
                        BumpRigid b1 = bumper1.CompObj as BumpRigid;
                        BumpRigid b2 = bumper2.CompObj as BumpRigid;
                        if (RigidsHelperModule.IsCloseEnough(b1, b2, 90))
                        {
                            AddToProperList(bumper1, bumper2, ref listOfCloseToBumper1, ref result);
                            //listOfCloseToBumper1.Add(bumper2);
                        }
                    }
                }
                if (listOfCloseToBumper1.Count > 1)
                    result.Add(listOfCloseToBumper1);
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[0].Sort(GroupSorter);
            }
            return result;
        }

        private static int GroupSorter(BumperEntityPoly b1, BumperEntityPoly b2)
        {
            if (b1.PositionXNACenter2D.X > b2.PositionXNACenter2D.X)
            {
                return 1;
            }
            else
            {
                if (b1.PositionXNACenter2D.X == b2.PositionXNACenter2D.X)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }

        private static void AddToProperList(BumperEntityPoly bRef, BumperEntityPoly bNew, ref List<BumperEntityPoly> bRefList,
                                     ref List<List<BumperEntityPoly>> result)
        {
            var list = result.Find(delegate(List<BumperEntityPoly> bList) { return bList.Contains(bRef); });
            if (list == null)
            {
                list = result.Find(delegate(List<BumperEntityPoly> bList) { return bList.Contains(bNew); });
                if (list == null)
                {
                    if (!bRefList.Contains(bNew))
                        bRefList.Add(bNew);
                    return;
                }
            }
            if (!list.Contains(bNew))
                list.Add(bNew);
        }

        public static int BumpersComparator(CompEntityPoly e1, CompEntityPoly e2)
        {
            BumpRigid b1 = e1.CompObj as BumpRigid;
            BumpRigid b2 = e2.CompObj as BumpRigid;
            if (b1.PositionXNACenter2D.Y > b2.PositionXNACenter2D.Y)
            {
                return 1;
            }
            else
            {
                if (b1.PositionXNACenter2D.Y < b2.PositionXNACenter2D.Y)
                {
                    return -1;
                }
            }
            return 0;
        }

        //private static void RemoveNewlyAddedBumpsDelPolys(ref List<List<IntPoint>> initialPoly, CompEntityPoly entity)
        //{
        //    //List<BumperEntityPoly> rBumpers = entity.EntraAgentSimple.BumpersHandler.GetReachableBumpers(initialPoly,
        //    //                                                                                       entity.EntraAgentSimple
        //    //                                                                                             .AllCompsEntities);
        //    var coveredBumps = GetCoveredBumpers(entity, initialPoly);
        //    foreach (BumperEntityPoly bumper in coveredBumps)
        //    {
        //        //this.EntraAgentSimple.BumpersHandler.processedBumps.Remove(bumperEntityPoly);
        //        List<List<IntPoint>> bumperDelPolys = bumper.GetDelPolys(initialPoly,
        //                                                                 new List<CompEntityPoly>() { entity });
        //        EntraDrawer.DrawIntoFile(bumperDelPolys);
        //        List<List<IntPoint>> delIntersection = BumpersHandler.GetDelPolysIntersection(bumperDelPolys);
        //        EntraDrawer.DrawIntoFile(delIntersection);

        //        //var originalAreaToBeAdded = EntraSolver.GetPolySolution(result, delIntersection, ClipType.ctIntersection);
        //        EntraDrawer.DrawIntoFile(bumperDelPolys);
        //        if (delIntersection != null)
        //        {
        //            initialPoly = EntraSolver.GetPolySolution(initialPoly, delIntersection, ClipType.ctDifference);
        //            EntraDrawer.DrawIntoFile(initialPoly);
        //            if (!entity.EntraAgentSimple.BumpersHandler.processedBumps.Contains(bumper))
        //            {
        //                entity.EntraAgentSimple.BumpersHandler.processedBumps.Add(bumper);
        //            }
        //        }
        //    }
        //}

        //public static void ReAddNewlyAddedBumpsAreas(ref List<List<IntPoint>> initialPoly, CompEntityPoly adder)
        //{
        //    List<BumperEntityPoly> allBumpers =
        //        adder.EntraAgentSimple.AllCompsEntities.Where(e => e is BumperEntityPoly).Cast<BumperEntityPoly>().ToList();
        //    List<BumperEntityPoly> rBumpers = adder.EntraAgentSimple.BumpersHandler.GetReachableBumpers(initialPoly,
        //                                                                                           adder.EntraAgentSimple
        //                                                                                                 .AllCompsEntities);
        //    EntraDrawer.DrawIntoFile(initialPoly);
        //    var coveredBumps = GetCoveredBumpers(adder, initialPoly);
        //    foreach (BumperEntityPoly bumper in coveredBumps)
        //    {
        //        // for debug
        //        var pos = adder.PositionXNACenter2D;
        //        var b = bumper.CompObj as BumpRigid;
        //        var vPos = b.PositionXNACenter2D;

        //        var bumperCoveringPoly = bumper.GetAreaPoly(adder.PositionXNACenter2D);
        //        EntraDrawer.DrawIntoFile(bumperCoveringPoly);

        //        // GO RECURSION for Coverd bumpers by the covered bumper
        //        List<BumperEntityPoly> coveredByBumpBumpers = GetAnotherBumperIsCovered(bumperCoveringPoly, bumper,
        //                                                                               allBumpers);

        //        List<CompEntityPoly> covered = coveredByBumpBumpers.Cast<CompEntityPoly>().ToList();
        //        coveredByBumpBumpers = GetBumperCloseOnYToOthers(bumper, coveredByBumpBumpers);
        //        if (coveredByBumpBumpers.Count > 0)
        //        {
        //            foreach (CompEntityPoly entityPoly in covered)
        //            {
        //                ReAddNewlyAddedBumpsAreas(ref initialPoly, entityPoly);
        //            }
        //        }
        //        //    //List<BumperEntityPoly> coveredByBumpBumpers =  GetBumperCloseOnYToOthers(bumper, allBumpers);
        //        //    List<Polygon> delPolys = new Polygons();
        //        //    foreach (BumperEntityPoly compEntityPoly in covered)
        //        //    {
        //        //        delPolys.AddRange(BumperEntityPoly.GetDelPolys(compEntityPoly, bumperCoveringPoly,
        //        //                                                       new List<CompEntityPoly>() {bumper}));
        //        //    }

        //        //    EntraDrawer.DrawIntoFile(delPolys);
        //        //    delPolys = BumpersHandler.GetDelPolysIntersection(delPolys);
        //        //    EntraDrawer.DrawIntoFile(delPolys);
        //        //    if (delPolys != null)
        //        //        bumperCoveringPoly = EntraSolver.GetPolySolution(bumperCoveringPoly, delPolys, ClipType.ctDifference);
        //        //    PolysHelper.RemoveNewlyAddedBumpsDelPolys(ref bumperCoveringPoly, entity);
        //        //    entity.EntraAgentSimple.PolysLogger.Log(new PolyLog(bumper, bumperCoveringPoly));
        //        //    initialPoly = EntraSolver.GetPolySolution(initialPoly, bumperCoveringPoly, ClipType.ctUnion);
        //        //}
        //        //else
        //        {
        //            //EntraDrawer.DrawIntoFileTesting(bumperCoveringPoly);
        //            initialPoly = EntraSolver.GetPolySolution(initialPoly, bumperCoveringPoly, ClipType.ctUnion);
        //            BumpersHandler.RemoveNewlyAddedBumpsDelPolys(ref initialPoly, adder);
        //            EntraDrawer.DrawIntoFile(initialPoly);
        //            //EntraDrawer.DrawIntoFileTesting(bumperCoveringPoly);

        //            BumpersHandler.RemoveNewlyAddedBumpsDelPolys(ref bumperCoveringPoly, adder);
        //            EntraDrawer.DrawIntoFile(bumperCoveringPoly);
        //            adder.EntraAgentSimple.PolysLogger.Log(new PolyLog(bumper, bumperCoveringPoly, adder));

        //            //initialPoly = EntraSolver.GetPolySolution(initialPoly, bumperCoveringPoly, ClipType.ctUnion);
        //        }
        //    }
        //}

        //private static List<BumperEntityPoly> GetAnotherBumperIsCovered(List<List<IntPoint>> coveringArea, BumperEntityPoly bRef,
        //    List<BumperEntityPoly> allBumpers)
        //{
        //    List<BumperEntityPoly> result = new List<BumperEntityPoly>();
        //    foreach (BumperEntityPoly b in allBumpers)
        //    {
        //        if (bRef != b)
        //        {
        //            if (EntraSolver.IsPolyOperation(coveringArea, b.GetDefPoly(), ClipType.ctIntersection))
        //            {
        //                result.Add(b);
        //            }
        //        }
        //    }
        //    result.Sort(BumpersHandler.BumpersComparator);
        //    return result;
        //}

        private static List<BumperEntityPoly> GetBumperCloseOnYToOthers(BumperEntityPoly bRef,
            List<BumperEntityPoly> allBumpers)
        {
            List<BumperEntityPoly> result = new List<BumperEntityPoly>();
            foreach (BumperEntityPoly b in allBumpers)
            {
                if (bRef != b)
                {
                    var bRefPos = bRef.PositionXNACenter2D;
                    var bPos = b.PositionXNACenter2D;
                    //if (Vector2.Distance(bRefPos, bPos) < 120)
                    if (Math.Abs(bRefPos.Y - bPos.Y) < 30)
                    {
                        result.Add(b);
                    }
                }
            }
            return result;
        }

        public static List<BumperEntityPoly> GetCoveredBumpers(CompEntityPoly entity, List<List<IntPoint>> initialPoly)
        {
            List<BumperEntityPoly> coveredBumps = new List<BumperEntityPoly>();
            List<BumperEntityPoly> rBumpers = entity.EntraAgentSimple.BumpersHandler.GetReachableBumpers(initialPoly,
                                                                                                   entity.EntraAgentSimple
                                                                                                         .AllCompsEntities);
            foreach (BumperEntityPoly bumper in rBumpers)
            {
                if (EntraSolver.IsPolyOperation(initialPoly, bumper.GetDefPoly(),
                                                ClipType.ctIntersection))
                {
                    coveredBumps.Add(bumper);
                }
            }
            return coveredBumps;
        }

        public static void AddBumpsAreaForBubble(ref List<List<IntPoint>> initialPoly, BumperEntityPoly bumper,
            BubbleEntityPoly bubble)
        {
            BubbleService bubbleService = bubble.CompObj as BubbleService;
            Vector2 bubblePos = bubbleService.PositionXNACenter;
            //var coveredBumps = BumpersHandler.GetCoveredBumpers(bubble, initialPoly);

            int fR = AreaCompPolyHandler.fR;
            int nR = AreaCompPolyHandler.nR;
            int dfR = AreaCompPolyHandler.dfR;
            //foreach (BumperEntityPoly bumper in coveredBumps)
            {
                BumpRigid bump = bumper.CompObj as BumpRigid;
                Vector2 bPos = bump.PositionXNACenter2D;
                CookieDirection abDir = AreaCompPolyHandler.GetABCookieDirection(bubblePos, bPos);
                CookieDirection rlDir = AreaCompPolyHandler.GetRLCookieDirection(bubblePos, bPos);

                switch (bump.Dir)
                {
                    case Direction.East:
                    case Direction.West:
                        if (abDir == CookieDirection.FromBottom)
                        {
                            if (rlDir == CookieDirection.FromRight)
                            {
                                initialPoly = EntraSolver.GetPolySolution(initialPoly,
                                                                          new List<IntPoint>()
                                                                      {
                                                                          new IntPoint((int) bPos.X, (int) bPos.Y + 900),
                                                                          new IntPoint((int) bPos.X - fR, (int) bPos.Y + 900),
                                                                          new IntPoint((int) bPos.X - fR,(int) bPos.Y - 900),
                                                                          new IntPoint((int) bPos.X, (int) bPos.Y - 900)
                                                                      },
                                                                          ClipType.ctUnion);
                            }
                            else
                            {
                                //initialPoly = EntraSolver.GetPolySolution(initialPoly,
                                //                                          new Polygon()
                                //                                              {
                                //                                                  new IntPoint((int) bPos.X,
                                //                                                               (int) bPos.Y - 900),
                                //                                                  new IntPoint((int) bPos.X + fR,
                                //                                                               (int) bPos.Y - 900),
                                //                                                  new IntPoint((int) bPos.X + fR,
                                //                                                               (int) bPos.Y + nR),
                                //                                                  new IntPoint((int) bPos.X - 20,
                                //                                                               (int) bPos.Y + nR),
                                //                                                  new IntPoint((int) bPos.X - 20,
                                //                                                               (int) bPos.Y + 20),
                                //                                              },
                                //                                          ClipType.ctUnion);
                                initialPoly = EntraSolver.GetPolySolution(initialPoly,
                                                                          new List<IntPoint>()
                                                                              {
                                                                                  new IntPoint((int) bPos.X,
                                                                                               (int) bPos.Y - 900),
                                                                                  new IntPoint((int) bPos.X + fR,
                                                                                               (int) bPos.Y - 900),
                                                                                  new IntPoint((int) bPos.X + fR,
                                                                                               (int) bPos.Y + 900),
                                                                                  new IntPoint((int) bPos.X,
                                                                                               (int) bPos.Y + 900)
                                                                              },
                                                                          ClipType.ctUnion);
                            }
                        }
                        else
                        {
                            // the bubble is above the bumps so don't add anything
                        }
                        break;
                    case Direction.South:
                    case Direction.North:
                        break;
                    case Direction.NorthWest:
                    case Direction.SouthEast:
                        if (abDir == CookieDirection.FromBottom) // From Right and Left are the same
                        {
                            initialPoly = EntraSolver.GetPolySolution(initialPoly,
                                                                      new List<IntPoint>()
                                                                          {
                                                                              new IntPoint((int) bPos.X, 
                                                                                           (int) bPos.Y + 900),
                                                                              new IntPoint((int) bPos.X - fR,
                                                                                           (int) bPos.Y + 900),
                                                                              new IntPoint((int) bPos.X - fR,
                                                                                           (int) bPos.Y - 900),
                                                                              new IntPoint((int) bPos.X,
                                                                                           (int) bPos.Y - 900)
                                                                          },
                                                                      ClipType.ctUnion);
                        }
                        break;
                    case Direction.NorthEast:
                    case Direction.SouthWest:
                        if (abDir == CookieDirection.FromBottom) // From Right and Left are the same
                        {
                            //var poly = new Polygon()
                            //    {
                            //        new IntPoint((int) bPos.X, (int) bPos.Y - 900),
                            //        new IntPoint((int) bPos.X + fR,
                            //                     (int) bPos.Y - 900),
                            //        new IntPoint((int) bPos.X + fR, (int) bPos.Y),
                            //        new IntPoint((int) bPos.X, (int) bPos.Y),
                            //    };
                            //EntraDrawer.DrawIntoFile(poly);
                            //initialPoly = EntraSolver.GetPolySolution(initialPoly,
                            //                                            poly,
                            //                                              ClipType.ctUnion);
                            var poly = new List<IntPoint>()
                                                                              {
                                                                                  new IntPoint((int) bPos.X,
                                                                                               (int) bPos.Y - 900),
                                                                                  new IntPoint((int) bPos.X + fR,
                                                                                               (int) bPos.Y - 900),
                                                                                  new IntPoint((int) bPos.X + fR,
                                                                                               (int) bPos.Y + 900),
                                                                                  new IntPoint((int) bPos.X,
                                                                                               (int) bPos.Y + 900),
                                                                              };
                            initialPoly = EntraSolver.GetPolySolution(initialPoly, poly, ClipType.ctUnion);
                            EntraDrawer.DrawIntoFile(poly);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }
    }
}
