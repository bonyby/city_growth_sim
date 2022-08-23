using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CityGrowthSim.Utility
{
    /// <summary>
    /// A class implementing the GJK algorithm for checking intersection between two convex polygons (does not work for polygons with arcs (e.g. circles) as of now. Can be extended).
    /// Implementation based on: https://www.youtube.com/watch?v=ajv46BSqcK4&ab_channel=Reducible
    /// </summary>
    internal static class GJK
    {
        public static bool PolygonsIntersecting(PointF[] poly1, PointF[] poly2)
        {
            PointF poly1Cent = PointUtility.CalculateCentroid(poly1);
            PointF poly2Cent = PointUtility.CalculateCentroid(poly2);
            PointF initDir = PointUtility.NormalizePointF(PointUtility.Subtract(poly2Cent, poly1Cent)); // Initial direction to look for intersection

            PointF[] simplex = new PointF[] { CalculateSupportPoint(poly1, poly2, initDir) }; // Simplex

            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the triple cross product (A X B) X C
        /// </summary>
        /// <param name="A">PointF representing Vector A</param>
        /// <param name="B">PointF representing Vector B</param>
        /// <param name="C">PointF representing Vector C</param>
        /// <returns>PointF representing cross product (A X B) X C</returns>
        private static PointF TripleCrossProduct(PointF A, PointF B, PointF C)
        {
            Vector3D vecA = new Vector3D(A.X, A.Y, 0);
            Vector3D vecB = new Vector3D(B.X, B.Y, 0);
            Vector3D vecC = new Vector3D(C.X, C.Y, 0);

            Vector3D res = Vector3D.CrossProduct(Vector3D.CrossProduct(vecA, vecB), vecC);

            return new PointF((float)res.X, (float)res.Y);
        }

        /// <summary>
        /// Calculates the support point of poly1, poly2 and the direction. The support point
        /// is on the boundary of the Minkowski difference.
        /// </summary>
        /// <param name="poly1">Polygon 1</param>
        /// <param name="poly2">Polygon 2</param>
        /// <param name="dir">Direction to look for support point</param>
        /// <returns>The support point on the boundary of the Minkowski difference</returns>
        private static PointF CalculateSupportPoint(PointF[] poly1, PointF[] poly2, PointF dir)
        {
            PointF furthest1 = FurthestPointInDirection(poly1, dir);
            PointF furthest2 = FurthestPointInDirection(poly2, PointUtility.Negate(dir)); // negative direction to get Minkowski difference and not sum

            PointF supportPoint = PointUtility.Subtract(furthest1, furthest2);
            return supportPoint;
        }

        /// <summary>
        /// Calculates the point furthest in a given direction of the polygon
        /// </summary>
        /// <param name="poly">Polygon's points to check</param>
        /// <param name="dir">Direction to look in</param>
        /// <returns>Point in the polygon furthest away in the given direction</returns>
        private static PointF FurthestPointInDirection(PointF[] poly, PointF dir)
        {
            PointF furthest = poly[0];
            double val = double.MinValue;

            foreach (PointF point in poly)
            {
                double v = PointUtility.Dot(point, dir);

                if (v > val)
                {
                    val = v;
                    furthest = point;
                }
            }

            return furthest;
        }
    }
}
