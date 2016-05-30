using System;
using System.Collections.Generic;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath
{
    [Serializable]
    public class PolyNode
    {
        public readonly PolyLog PolyLog;
        public readonly PolyNode Parent;
        public List<PolyNode> Childs;

        public PolyNode(PolyLog polyLog, PolyNode parent)
        {
            PolyLog = polyLog;
            Parent = parent;
            Childs = new List<PolyNode>();
        }

        public void AddChilds(List<PolyLog> getIntersectedPoly)
        {
            foreach (PolyLog polyLog in getIntersectedPoly)
            {
                this.Childs.Add(new PolyNode(polyLog, this));
            }
        }
    }
}
