using System;
using System.Collections.Generic;
using ClipperLib;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    [Serializable]
    public class EntraResult
    {
        public List<List<IntPoint>> ReachableSpace { set; get; }
        public List<List<IntPoint>> Frog { set; get; }
        public List<List<IntPoint>> FrogCompsInter { set; get; }
        public bool IsPlayable { set; get; }
        public Vector2 NearestPointToFrog;
        public float MinDistToFrog;

        public EntraResult(Vector2 frogPos, List<List<IntPoint>> frog, List<List<IntPoint>> frogCompsInter, List<List<IntPoint>> reachableSpace, bool isPlayable)
        {
            Frog = frog;
            FrogCompsInter = frogCompsInter;
            ReachableSpace = reachableSpace;
            IsPlayable = isPlayable;
            NearestPointToFrog = FindNearestPoint(frogPos, reachableSpace);
            MinDistToFrog = (NearestPointToFrog - frogPos).Length();
        }

        private Vector2 FindNearestPoint(Vector2 frogPos, List<List<IntPoint>> reachableSpace)
        {
            float minDist = float.MaxValue;
            Vector2 nearestPoint = Vector2.Zero;
            foreach (List<IntPoint> poly in reachableSpace)
            {
                foreach (IntPoint intPoint in poly)
                {
                    Vector2 point = new Vector2(intPoint.X, intPoint.Y);
                    float dist = (point - frogPos).Length();
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestPoint = point;
                    }
                }
            }
            return nearestPoint;
        }
    }
}