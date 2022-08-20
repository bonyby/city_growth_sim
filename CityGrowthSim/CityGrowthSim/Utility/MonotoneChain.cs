using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Utility
{
    internal static class MonotoneChain
    {
        /// <summary>
        /// Calculates the convex hull of points using the Andrew's Monotone Chain algorithm (a variant of Graham Scan): https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
        /// Assumes |points| >= 3 and that not all points are collinear.
        /// </summary>
        /// <param name="points">Points to generate convex hull of</param>
        /// <returns>Points of convex hull in counter clockwise order</returns>
        public static Point[] GetConvexHull(Point[] points)
        {
            Point[] ps = new Point[points.Length];
            points.CopyTo(ps, 0);
            Array.Sort(ps, (p1, p2) => { return p1.X != p2.X ? p1.X - p2.X : p1.Y - p2.Y; }); // Sort points based on x-val. y-val if same x-val.

            List<Point> L = new List<Point>(), U = new List<Point>();

            // Generate upper hull
            for (int i = ps.Length-1; i >= 0; i--)
            {
                // Remove the last point in U as long as adding p_i constitutes a nonleft turn
                while(U.Count >= 2 && Cross(U[U.Count - 2], U[U.Count - 1], ps[i]) <= 0)
                {
                    U.RemoveAt(U.Count - 1);
                }

                U.Add(ps[i]);
            }

            // Generate lower hull
            for (int i = 0; i < ps.Length; i++)
            {
                // Remove the last point in L as long as adding p_i constitutes a nonleft turn
                while (L.Count >= 2 && Cross(L[L.Count - 2], L[L.Count - 1], ps[i]) <= 0)
                {
                    L.RemoveAt(L.Count - 1);
                }

                L.Add(ps[i]);
            }

            // Remove the last point added to U & L as both of these occur in both lists.
            // U is build left -> right and L right -> left, 
            U.RemoveAt(U.Count - 1);
            L.RemoveAt(L.Count - 1);

            return L.Concat(U).ToArray();
        }

        /// <summary>
        /// Calculates the z-coordinate of the cross product between point_o1 and point_o2.
        /// If > 0 indicates a left turn, = 0 indicates collinearity and < 0 indicates a right turn (all for points arranged in counter clockwise order)
        /// </summary>
        /// <param name="a">Point </param>
        /// <param name="b"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        private static float Cross(Point a, Point b, Point o)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (b.X - o.X) * (a.Y - o.Y);
        }
    }
}
