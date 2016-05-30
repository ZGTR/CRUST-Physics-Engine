using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.CompEntityPolys.CompsEntities;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlus
{
    class EntraPlusUsageManager
    {
        private readonly List<List<PolyLog>> _allPaths;
        private readonly bool _isShortestPathOnlyComparsion;
        public int UsedRockets = 0;
        public int UsedBumps = 0;
        public int UsedBlowers = 0;
        public int UsedBubbles = 0;
        public int UsedRopes = 0;
        private readonly List<CompEntityPoly> _alreadyUsed;

        public EntraPlusUsageManager(List<List<PolyLog>> allPaths, bool isShortestPathOnlyComparsion)
        {
            _allPaths = allPaths;
            _isShortestPathOnlyComparsion = isShortestPathOnlyComparsion;
            _alreadyUsed = new List<CompEntityPoly>();
        }

        public void DoAnalysis()
        {
            if (_isShortestPathOnlyComparsion)
                DoAnalysisForPaths(GetShortestPath(_allPaths));
            else
                DoAnalysisForPaths(_allPaths);
        }

        public static List<List<PolyLog>> GetShortestPath(List<List<PolyLog>> allPaths)
        {
            var res = new List<List<PolyLog>>();
            if (allPaths != null && allPaths.Count > 0)
            {
                int minCount = allPaths.Min(p => p.Count);
                var shortestP = (from p in allPaths
                                 where p.Count == minCount
                                 select p).First();
                res.Add(shortestP);
            }
            return res;
        }

        private void DoAnalysisForPaths(List<List<PolyLog>> allPaths)
        {
            foreach (List<PolyLog> polyLogs in allPaths)
            {
                foreach (PolyLog polyLog in polyLogs)
                {
                    if (!_alreadyUsed.Contains(polyLog.Comp))
                    {
                        AddUsage(polyLog.Comp);
                        _alreadyUsed.Add(polyLog.Comp);
                    }
                }
            }
        }

        private void AddUsage(CompEntityPoly comp)
        {
            if (comp is RocketEntityPoly)
            {
                UsedRockets++;
            }
            if (comp is RopeEntityPoly)
            {
                UsedRopes++;
            }
            if (comp is BlowerEntityPoly)
            {
                UsedBlowers++;
            }
            if (comp is BubbleEntityPoly)
            {
                UsedBubbles++;
            }
            if (comp is BumperEntityPoly)
            {
                UsedBumps++;
            }
        }

        public int GetUsedCompsCount()
        {
            return this.UsedRockets +
                   this.UsedRopes +
                   this.UsedBlowers +
                   this.UsedBubbles +
                   this.UsedBumps;
        }

        public int GetUsedCompsCountNoRopes()
        {
            return GetUsedCompsCount() - this.UsedRopes;
        }
    }
}
