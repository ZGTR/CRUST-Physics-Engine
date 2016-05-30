using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay
{
    class ClockwiseSorter
    {
        private readonly List<IntPoint> _poly;
        private IntPoint _center;

        public ClockwiseSorter(List<IntPoint> poly)
        {
            _poly = poly;
        }

        public ClockwiseSorter(List<Vector2> list)
        {
            _poly = PolysHelper.BuildPolygon(list);
        }

        public List<IntPoint> Sort()
        {
            _center.X = (int)this._poly.Average(p => p.X);
            _center.Y = (int)this._poly.Average(p => p.Y);
            _poly.Sort(Sorter);
            return _poly;
        }

        public int Sorter(IntPoint a, IntPoint b)
        {
            if (a.X - _center.X >= 0 && b.X - _center.X < 0)
                return 1;
            if (a.X - _center.X == 0 && b.X - _center.X == 0)
            {
                if (a.Y - _center.Y >= 0 || b.Y - _center.Y >= 0)
                    return 1;// a.Y > b.Y;
                return -1; //b.Y > a.Y;
            }

            // compute the cross product of vectors (center -> a) x (center -> b)
            int det = (int)((a.X - _center.X) * (b.Y - _center.Y) - (b.X - _center.X) * (a.Y - _center.Y));
            if (det < 0)
                return 1;
            if (det > 0)
                return -1;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            int d1 = (int)((a.X - _center.X) * (a.X - _center.X) + (a.Y - _center.Y) * (a.Y - _center.Y));
            int d2 = (int)((b.X - _center.X) * (b.X - _center.X) + (b.Y - _center.Y) * (b.Y - _center.Y));
            return 1;// d1 > d2;
        }
    }
}
