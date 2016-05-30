using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using CRUSTEngine.ProjectEngines;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    [Serializable]
    public class EntraAgentSimple
    {
        public EngineManager EngineState;

        public AreaCompPolyHandler AreaCompPolyHandler;
        public DefinitiveCompPolyHandler DefCompPolyHandler;
        public ProjectionHandler ProjectionHandler;
        public BumpersHandler BumpersHandler;
        private RocketsHandler _rocketsHandler;
        private EntityBuilder _entityBuilder;
        public PolysLogger PolysLogger;

        public List<CompEntityPoly> ropesEntityList;
        public List<CompEntityPoly> rocketsEntityList;
        public List<CompEntityPoly> blowersEntityList;
        public List<CompEntityPoly> bubblesEntityList;
        public List<CompEntityPoly> bumpersEntityList;
        public List<CompEntityPoly> rCompsAll;  
        public List<CompEntityPoly> rCompsNoBumps;
        private List<BumperEntityPoly> shouldBeProcessedBumps;
        public List<CompEntityPoly> ProcessedEntities;

        public EntraAgentSimple()
        {
            ProcessedEntities = new List<CompEntityPoly>();
            PolysLogger = new PolysLogger();
        }

        public List<CompEntityPoly> AllCompsEntities
        {
            get
            {
                List<CompEntityPoly> allComps = new List<CompEntityPoly>();
                allComps.AddRange(ropesEntityList);
                allComps.AddRange(rocketsEntityList);
                allComps.AddRange(blowersEntityList);
                allComps.AddRange(bubblesEntityList);
                allComps.AddRange(bumpersEntityList);
                return allComps;
            }
        }

        private void SetMembers(EngineManager engineState)
        {
            this.EngineState = engineState;
            AreaCompPolyHandler = new AreaCompPolyHandler(EngineState);
            DefCompPolyHandler = new DefinitiveCompPolyHandler(EngineState);
            _entityBuilder = new EntityBuilder(this);
            BumpersHandler = new BumpersHandler(this);
            _rocketsHandler = new RocketsHandler(this);
            ProjectionHandler = new ProjectionHandler(this);
        }

        public EntraResult CheckPlayability(EngineManager engineState)
        {
            SetMembers(engineState);
            List<List<IntPoint>> reachableSpace = new List<List<IntPoint>>();
            FindWorkingSpace(ref reachableSpace);
            var frogEntity = new FrogEntityPoly(this, StaticData.EngineManager.FrogRB);
            List<List<IntPoint>> frogCompsInter = EntraSolver.GetPolySolution(reachableSpace, frogEntity.GetDefPoly(),
                                                             ClipType.ctIntersection);

            if (frogCompsInter.Count > 0)
            {
                //if (CanCookieFitInIntersection(StaticData.EngineManager.CookieRB, solution))
                //MessageBox.Show("The level is playable, recognized by Entra Agent.");
                return new EntraResult(StaticData.EngineManager.FrogRB.PositionXNACenter2D,
                                       frogEntity.GetDefPoly(),
                                       frogCompsInter,
                                       reachableSpace,
                                       true);
            }
            return new EntraResult(StaticData.EngineManager.FrogRB.PositionXNACenter2D,
                                   frogEntity.GetDefPoly(),
                                   frogCompsInter,
                                   reachableSpace,
                                   false);
        }


        private void FindWorkingSpace(ref List<List<IntPoint>> spaceSoFar)
        {
            ropesEntityList = _entityBuilder.GetRopesEntities();
            rocketsEntityList = _entityBuilder.GetRocketsEntities();
            blowersEntityList = _entityBuilder.GetBlowersEntities();
            bubblesEntityList = _entityBuilder.GetBubblesEntities();
            bumpersEntityList = _entityBuilder.GetBumpersEntities();

            List<CompEntityPoly> allEntitiesList = new List<CompEntityPoly>();
            allEntitiesList.AddRange(rocketsEntityList);
            allEntitiesList.AddRange(blowersEntityList);
            allEntitiesList.AddRange(bubblesEntityList);
            allEntitiesList.AddRange(bumpersEntityList);

            spaceSoFar = GetRopesAreaPolys(ropesEntityList.Cast<RopeEntityPoly>().ToList());
            spaceSoFar = BumpersHandler.SetProperBumpersEffect(spaceSoFar, ropesEntityList, allEntitiesList, false);


            
            ExploreSearchSpace(ref spaceSoFar, allEntitiesList, ropesEntityList[0], true, 0);

            List<List<IntPoint>> result = new List<List<IntPoint>>();
            foreach (PolyLog polyLog in this.PolysLogger.Logs)
            {
                result = EntraSolver.GetPolySolution(result, polyLog.PolysUnion, ClipType.ctUnion);
            }
            spaceSoFar = result;
        }


        private void ExploreSearchSpace(ref List<List<IntPoint>> spaceSoFar,
                                        List<CompEntityPoly> allEntitiesList, CompEntityPoly lastAdder, bool firstTime,
                                        int counter)
        {
            if (counter > 30)
            {
                return;
            }
            else
            {
                if (firstTime)
                {
                    spaceSoFar = BumpersHandler.SetProperBumpersEffect(spaceSoFar,
                                                                       ropesEntityList,
                                                                       allEntitiesList,
                                                                       false);
                }
                else
                {
                    //spaceSoFar = BumpersHandler.SetProperBumpersEffect(spaceSoFar,
                    //                                                   new List<CompEntityPoly>() {lastAdder},
                    //                                                   allEntitiesList,
                    //                                                   false);
                }

                rCompsAll = AreaCompPolyHandler.GetReachServicePolys(spaceSoFar, allEntitiesList);
                rCompsAll = SortBumpsFirst(rCompsAll);
            }

            if (rCompsAll.Count > 0)
            {
                foreach (CompEntityPoly newCovered in rCompsAll)
                {
                    if (newCovered != lastAdder)
                    {
                        if (EntraSolver.IsPolyOperation(lastAdder.GetCoverageSoFar(),
                                                        newCovered.GetDefPoly(), ClipType.ctIntersection))
                        {
                            //if (!ProcessedEntities.Contains(newCovered))
                            if (IsOkToProcess(lastAdder, newCovered))
                            {
                                //IncrementProcessed(newCovered);
                                //if (false)
                                //{
                                //    //EntraDrawer.DrawIntoFileTesting(spaceSoFar);
                                //}
                                //EntraDrawer.DrawIntoFileTesting(spaceSoFar);
                                spaceSoFar = newCovered.ApplyEffect(spaceSoFar, lastAdder);
                                //EntraDrawer.DrawIntoFile(spaceSoFar);

                                //allEntitiesList.Remove(newCovered);

                                ExploreSearchSpace(ref spaceSoFar, allEntitiesList, newCovered, false, counter + 1);
                            }
                        }
                    }
                }
            }

            //spaceSoFar = BumpersHandler.SetProperBumpersEffect(spaceSoFar,
            //                                                   new List<CompEntityPoly>() {lastAdder},
            //                                                   allEntitiesList,
            //                                                   false);

        }

        private Dictionary<CompEntityPoly, List<CompEntityPoly>> _processedComps =
            new Dictionary<CompEntityPoly, List<CompEntityPoly>>();
        private bool IsOkToProcess(CompEntityPoly lastAdder, CompEntityPoly newCovered)
        {
            if (!_processedComps.Keys.Contains(lastAdder))
            {
                _processedComps.Add(lastAdder, new List<CompEntityPoly>() { newCovered });
                return true;
            }

            if (!_processedComps[lastAdder].Contains(newCovered))
            {
                _processedComps[lastAdder].Add(newCovered);
                return true;
            }
            return false;
        }

        //private void IncrementProcessed(CompEntityPoly newCovered)
        //{
        //    _processedComps[newCovered]++;
        //}

        private List<CompEntityPoly> SortBumpsFirst(List<CompEntityPoly> compEntityPolys)
        {
            List<CompEntityPoly> res = new List<CompEntityPoly>();
            List<BumperEntityPoly> bumps = compEntityPolys.Where(c => c is BumperEntityPoly).Cast<BumperEntityPoly>().ToList();
            bumps.Sort(BumpersHandler.BumpersComparator);
            bumps.ForEach(res.Add);
            compEntityPolys.Where(c => !(c is BumperEntityPoly)).ToList().ForEach(res.Add);
            return res;
        }

        private List<List<IntPoint>> GetRopesAreaPolys(List<RopeEntityPoly> ropes)
        {
            List<List<IntPoint>> spaceSoFar = new List<List<IntPoint>>();
            //List<List<List<IntPoint>>> listOfPolygons = entities.Select(e => e.GetAreaPoly()).Where(e => e != null).ToList();
            //listOfPolygons.ForEach(result.AddRange);
            foreach (RopeEntityPoly ropeEntityPoly in ropes)
            {
                spaceSoFar = ropeEntityPoly.ApplyEffect(spaceSoFar, null);
            }
            return spaceSoFar;
        }

        public void AddShouldBeProcessedBumper(BumperEntityPoly bumperEntityPoly)
        {
            shouldBeProcessedBumps.Add(bumperEntityPoly);
        }
    }
}
