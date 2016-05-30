using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlus;
using Point = Microsoft.Xna.Framework.Point;
using PolyNode = CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath.PolyNode;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath
{
    [Serializable]
    public class EntraPathAgent
    {
        private readonly EntraAgentSimple _entra;
        private readonly List<PolyLog> _log;
        public List<List<PolyLog>> AllPaths;
        private FrogEntityPoly _frog;
        private PolyLog _frogPolyLog;

        public EntraPathAgent(EntraAgentSimple entra)
        {
            _entra = entra;
            _log = this._entra.PolysLogger.GetLog();
            AllPaths = new List<List<PolyLog>>();
            _frog = new FrogEntityPoly(this._entra, this._entra.EngineState.FrogRB);
            InitializeFrogPoly();
            SetAllPaths();
        }

        private void InitializeFrogPoly()
        {
            _frogPolyLog = new PolyLog(_frog);
            foreach (PolyLog polyLog in _log)
            {
                foreach (APPair apPair in polyLog.ApPairs)
                {
                    if (EntraSolver.IsPolyOperation(_frog.GetDefPoly(), apPair.Poly, ClipType.ctIntersection))
                    {
                        _frogPolyLog.AddPoly(_frog.GetDefPoly(), polyLog.Comp);
                    }
                }
            }
        }

        public List<List<PolyLog>> SetAllPaths()
        {
            //BuildPath();

            PolyNode baseNode = new PolyNode(_frogPolyLog, null);
            ConstructAllPathTree(this._log, new PolyNode(null, null), ref baseNode);

            List<PolyNode> pathEnds = new List<PolyNode>();
            GetTreeEnds(baseNode, ref pathEnds);
            BuildTreePaths(pathEnds, ref AllPaths);
            DeleteSamePaths(ref AllPaths);
            ReAddFrogToAllPaths(_frogPolyLog, ref AllPaths);

            return AllPaths;
        }

        private void ConstructAllPathTree(List<PolyLog> logs, PolyNode parentOfParent, ref PolyNode parentNode)
        {
            //EntraDrawer.DrawIntoFileTesting(logs[1].Poly);
            var compParent = parentNode.PolyLog.Comp;
            var newChilds = GetIntersectedPoly(parentOfParent.PolyLog, parentNode.PolyLog, logs);
            parentNode.AddChilds(newChilds);
            for (int i = 0; i < parentNode.Childs.Count; i++)
            {
                PolyNode child = parentNode.Childs[i];
                var compChild = child.PolyLog.Comp;
                if (!IsInParent(child))
                {
                    // DEBUG
                    //FrogEntityPoly frog = new FrogEntityPoly(this._entra, this._entra.EngineState.FrogRB);
                    //var pathPoints = BuildPath(new PolyLog(frog, frog.GetDefPoly()), _allPaths[]);
                    //var form = new PathForm(pathPoints);
                    //form.DrawPathIntoOutput("pathDEBUG.jpg");

                    if (!(child.PolyLog.Comp is RopeEntityPoly))
                    {
                        ConstructAllPathTree(logs, parentNode, ref child);
                    }
                }
            }
        }

        public void DrawPaths()
        {
            String dirName = "PolysTesting";
            FrogEntityPoly frog = new FrogEntityPoly(this._entra, this._entra.EngineState.FrogRB);
            PolyLog frogPolyLog = new PolyLog(frog, frog.GetDefPoly(), null);

            for (int i = 0; i < AllPaths.Count; i++)
            {
                var pathPoints = BuildPath(frogPolyLog, AllPaths[i]);
                var form = new PathForm(pathPoints, true);
                form.DrawPathIntoOutput(dirName + @"\path " + i + ".jpg");
            }
        }

        public void DrawShortestPath(int counter)
        {
            String dirName = "PolysTesting";
            FrogEntityPoly frog = new FrogEntityPoly(this._entra, this._entra.EngineState.FrogRB);
            PolyLog frogPolyLog = new PolyLog(frog, frog.GetDefPoly(), null);
            var shortestP = EntraPlusUsageManager.GetShortestPath(this.AllPaths)[0];
            
            var pathPoints = BuildPath(frogPolyLog, shortestP);
            var form = new PathForm(pathPoints, true);
            form.DrawPathIntoOutput(dirName + @"\path " + counter + ".jpg");
        }

        public Bitmap GetPathBitmap(int indexPath, bool withInputImage)
        {
            FrogEntityPoly frog = new FrogEntityPoly(this._entra, this._entra.EngineState.FrogRB);
            PolyLog frogPolyLog = new PolyLog(frog, frog.GetDefPoly(), null);
            var pathPoints = BuildPath(frogPolyLog, AllPaths[indexPath]);
            var form = new PathForm(pathPoints, withInputImage);

            return form.GetPathBitmap();
        }

        private void BuildTreePaths(List<PolyNode> pathEnds, ref List<List<PolyLog>> allPaths)
        {
            foreach (PolyNode end in pathEnds)
            {
                List<PolyLog> path = GetPath(end);
                allPaths.Add(path);
            }
        }

        private List<PolyLog> GetPath(PolyNode end)
        {
            List<PolyLog> path = new List<PolyLog>();
            var nodeIter = end;
            while (nodeIter.Parent != null)
            {
                path.Add(nodeIter.PolyLog);
                nodeIter = nodeIter.Parent;
            }
            return path;
        }

        private void ReAddFrogToAllPaths(PolyLog frogPolyLog, ref List<List<PolyLog>> allPaths)
        {
            for (int i = 0; i < allPaths.Count; i++)
            {
                var path = allPaths[i];
                path.Add(frogPolyLog);
            }
        }

        private void GetTreeEnds(PolyNode parentNode, ref List<PolyNode> pathEnds)
        {
            foreach (PolyNode child in parentNode.Childs)
            {
                if (child.Childs.Count != 0)
                {
                    GetTreeEnds(child, ref pathEnds);
                }
                else
                {
                    if (child.PolyLog.Comp is RopeEntityPoly)
                    {
                        pathEnds.Add(child);
                    }
                }
            }
        }

        private void DeleteSamePaths(ref List<List<PolyLog>> allPaths)
        {
            var pathsToRemove = new List<List<PolyLog>>();
            for (int i = 0; i < allPaths.Count - 1; i++)
            {
                var pathI = allPaths[i];
                for (int j = i + 1; j < allPaths.Count; j++)
                {
                    var pathJ = allPaths[j];
                    if (IsSamePath(pathI, pathJ))
                    {
                        pathsToRemove.Add(pathJ);
                    }
                }
            }
            for (int i = 0; i < pathsToRemove.Count; i++)
            {
                allPaths.Remove(pathsToRemove[i]);
            }
        }

        private bool IsSamePath(List<PolyLog> pathI, List<PolyLog> pathJ)
        {
            if (pathI.Count != pathJ.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < pathI.Count; i++)
                {
                    if (pathI[i].Comp != pathJ[i].Comp)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void DeleteNonRopeEndsPaths(ref List<List<PolyLog>> allPaths)
        {
            var pathsToRemove = new List<List<PolyLog>>();
            for (int i = 0; i < allPaths.Count; i++)
            {
                var path = allPaths[i];
                if (!(path[path.Count - 1].Comp is RopeEntityPoly))
                {
                    pathsToRemove.Add(path);
                }
            }
            for (int i = 0; i < pathsToRemove.Count; i++)
            {
                allPaths.Remove(pathsToRemove[i]);
            }
        }

        private bool IsInParent(PolyNode child)
        {
            var childIter = child.Parent;

            while (childIter != null)
            {
                if (child.PolyLog.Comp == childIter.PolyLog.Comp)
                {
                    return true;
                }
                childIter = childIter.Parent;
            }
            return false;
        }

        private int RopeLastSorter(PolyLog x, PolyLog y)
        {

            if (x.Comp is RopeEntityPoly || y.Comp is RopeEntityPoly)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private void BuildParallelPaths(List<PolyLog> intersectedLogs, ref List<List<PolyLog>> allPaths, int pathCounter)
        {
            if (intersectedLogs.Count == 1)
            {
                if (allPaths.Count - 1 < pathCounter)
                {
                    allPaths.Add(new List<PolyLog>());
                }
            }
            else
            {
                int initCount = allPaths.Count - 1;
                for (int i = 1; i < intersectedLogs.Count; i++)
                {
                    int counter = initCount + i;
                    if (allPaths.Count - 1 < counter)
                    {
                        allPaths.Add(new List<PolyLog>());
                    }
                    allPaths[counter].AddRange(allPaths[pathCounter]);
                    
                    //foreach (PolyLog polyLogs in allPaths[counter])
                    //{
                    //    if (!allPaths[counter + i].Contains(polyLogs))
                    //        allPaths[counter + i].Add(polyLogs);
                    //}
                }
            }
        }

        private List<PolyLog> GetIntersectedPoly(PolyLog parentOfParentLog, PolyLog parentLog, List<PolyLog> log)
        {
            List<PolyLog> res = new List<PolyLog>();
            foreach (PolyLog polyLog in log)
            {
                if (parentLog != polyLog)
                {
                    //EntraDrawer.DrawIntoFileTesting(polyLog.PolysUnion);
                    //EntraDrawer.DrawIntoFileTesting(parentLog.PolysUnion);
                    //EntraDrawer.DrawIntoFileTesting(parentOfParentLog.PolysUnion);
                    
                    if (polyLog.IsOperation(parentOfParentLog, parentLog, ClipType.ctIntersection))
                    //if (EntraSolver.IsPolyOperation(parentLog.Poly, polyLog.Poly, ClipType.ctIntersection))
                    {
                        if (polyLog.Comp is RopeEntityPoly)
                        {
                            if (res.Count == 0)
                            {
                                res.Insert(0, polyLog);
                            }
                            else
                            {
                                res.Insert(res.Count - 1, polyLog);
                            }
                        }
                        else
                        {
                            res.Insert(0, polyLog);
                        }
                    }
                }
            }
            return res;
        }

        private static List<Point> BuildPath(PolyLog frogPolyLog, List<PolyLog> path)
        {
            //path.Insert(0, frogPolyLog);
            List<Point> pathPoints = new List<Point>();
            for (int i = 0; i < path.Count; i++)
            {
                PolyLog entity = path[i];
                if (entity.Comp is RopeEntityPoly)
                {
                    pathPoints.Add(VectorToPoint(StaticData.EngineManager.CookieRB.PositionXNACenter2D));
                }
                else
                {
                    pathPoints.Add(VectorToPoint(entity.Comp.PositionXNACenter2D));
                }
            }
            return pathPoints;
        }

        private static Point VectorToPoint(Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }

        public int GetFitnessValue(List<Point> designerPath, bool isOrdering)
        {
            int bestFit = Int32.MaxValue;
            List<PolyLog> nPath = null;
            foreach (List<PolyLog> path in AllPaths)
            {
                int ind = AllPaths.IndexOf(path);
                var pathPts = BuildPath(this._frogPolyLog, path);
                int dis = GetFitness(pathPts, designerPath, isOrdering);
                //if (ind == 18)
                //{
                //    int kkk = 0;
                //}
                if (dis < bestFit)
                {
                    bestFit = dis;
                    //nPath = path;
                }
            }
            //int index = AllPaths.IndexOf(nPath);
            return bestFit;
        }

        private int GetFitness(List<Point> path, List<Point> designerPath, bool isOrdering)
        {
             //path = new List<Point>()
             //   {
             //       new Point(460, 400),
             //       new Point(460, 40),
             //       new Point(200, 400),
             //       new Point(200, 40)
             //   };
            List<int> distances = new List<int>();
            int lnPIndexInConst = -1;
            bool firstTime = true;
            Point lnP = new Point(-100, -100);
            List<Point> constPath = new List<Point>(path);
            int seqPCounter = 0;
            int dlengthCounter = 0;
            int pBiasedCounter = 0;

            foreach (Point designerPoint in designerPath)
            {
                if (path.Count > 0)
                {
                    Point nP = FindNearestPoint(path, designerPoint);
                    if (isOrdering)
                    {
                        if (lnPIndexInConst + 1 < constPath.Count)
                        {
                            if (nP != constPath[lnPIndexInConst + 1])
                            {
                                seqPCounter += 1;
                            }
                        }
                        else
                        {
                            pBiasedCounter += 1;
                        }
                    }
                    else
                    {
                        // NO ORDERING
                    }
                    Vector2 v1 = new Vector2(designerPoint.X, designerPoint.Y);
                    Vector2 v2 = new Vector2(nP.X, nP.Y);
                    int dist = (int) (v1 - v2).Length();
                    distances.Add(dist);
                    lnP = nP;
                    lnPIndexInConst = constPath.IndexOf(lnP);
                    path.Remove(nP);
                }
                else
                {
                    dlengthCounter += 1;
                }
            }
            float fitness = 0;

            //Prepare data
            int nrPPointsUsed = distances.Count;
            int diffLength = Math.Abs(designerPath.Count - nrPPointsUsed);

            //float avg = MathHelperModule.CalcAvg(distances);
            float stdDist = MathHelperModule.CalcStd(distances);
            float maxStdDist = CalcStdMax(distances);

            //Calculate data
            float seqPCounterN = MathHelperModule.Normalize(seqPCounter, designerPath.Count, 0);
            //float dlengthCounterN = MathHelperModule.Normalize(dlengthCounter, designerPath.Count, 0);
            //float pBiasedCounterN = MathHelperModule.Normalize(pBiasedCounter, path.Count, 0);
            //float diffLengthN = MathHelperModule.Normalize(diffLength, 1, 0);
            float stdDistN = MathHelperModule.Normalize(stdDist, maxStdDist, 1);

            fitness = 35*seqPCounterN
                      //+ 10 * dlengthCounterN 
                      //+ 10*pBiasedCounterN
                      //+ 13*diffLengthN
                      + 65*stdDistN;

            if(float.IsNaN(fitness))
            {
                fitness = 0;
            }

            StreamWriter sw = new StreamWriter(@"C:\CTREngine\EntraPathFitnessParams.txt", true);
            sw.WriteLine(
                String.Format("{0:0.00}", fitness)
                + "\t" + String.Format("{0:0.00}", seqPCounterN)
                //+ "\t" + String.Format("{0:0.00}", dlengthCounterN)
                //+ "\t" + String.Format("{0:0.00}", pBiasedCounterN)
                //+ "\t" + String.Format("{0:0.00}", diffLengthN)
                + "\t" + String.Format("{0:0.00}", stdDistN)

                + "\t"
                + "\t"
                + "\t"

                + "\t" + String.Format("{0:0.00}", seqPCounter)
                //+ "\t" + String.Format("{0:0.00}", dlengthCounter)
                //+ "\t" + String.Format("{0:0.00}", pBiasedCounter)
                //+ "\t" + String.Format("{0:0.00}", diffLength)
                + "\t" + String.Format("{0:0.00}", stdDist));
            sw.Flush();
            sw.Close();
            return (int)Math.Truncate(fitness);
        }

        private float CalcStdMax(List<int> distances)
        {
            var newDists = CreateBiasedDistances(distances);
            double avg = MathHelperModule.CalcAvg(newDists);

            double diffs = 0;
            newDists.ForEach(i => diffs += Math.Pow((i - avg), 2));

            double div = diffs / avg;

            float res = (float)Math.Sqrt(div);

            return res;
        }

        private List<int> CreateBiasedDistances(List<int> distances)
        {
            List<int> newDists = new List<int>();
            double total = MathHelperModule.CalcTotal(distances);
            int nrToAddLast = 0;
            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i] > 20)
                {
                    newDists.Add(20);
                    nrToAddLast += distances[i] - 20;
                }
                else
                {
                    newDists.Add(0);
                    nrToAddLast += distances[i];
                }
            }
            newDists[newDists.Count - 1] = nrToAddLast + newDists[newDists.Count - 1];
            return newDists;
        }

        private Point FindNearestPoint(List<Point> path, Point point)
        {
            Vector2 vNearestPoint = new Vector2(path[0].X, path[0].Y);
            Point nearestPoint = path[0];
            Vector2 vRef = new Vector2(point.X, point.Y);
            int nearestDis = (int)(vNearestPoint - vRef).Length();
            
            foreach (Point p in path)
            {
                Vector2 vNewP = new Vector2(p.X, p.Y);
                var newDis = (int)(vRef - vNewP).Length();
                if (newDis < nearestDis)
                {
                    vNearestPoint = vNewP;
                    nearestDis = newDis;
                    nearestPoint = p;
                }
            }
            return nearestPoint;
        }
    }
}

