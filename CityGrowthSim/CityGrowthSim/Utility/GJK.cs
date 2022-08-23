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
        /// <summary>
        /// Calculates whether or not two polygons are intersecting.
        /// </summary>
        /// <param name="poly1">First polygon</param>
        /// <param name="poly2">Second polygon</param>
        /// <returns>true if the polygons are intersecting, false if not</returns>
        public static bool PolygonsIntersecting(PointF[] poly1, PointF[] poly2)
        {
            // -- Basic idea is to find simplexes with corner points on the Minkowski difference of poly1 and poly2.
            // -- The polygons intersect if atleast one point in each are equal to eachother: p1=p2 => p1-p2=0.
            // -- If a simplex is found which contains origo, then poly1 and poly2 are intersecting. If no such simplex exists, then no intersection is happening.

            PointF poly1Cent = PointUtility.CalculateCentroid(poly1);
            PointF poly2Cent = PointUtility.CalculateCentroid(poly2);
            PointF dir = PointUtility.Normalize(PointUtility.Subtract(poly2Cent, poly1Cent)); // Initial direction to look for intersection

            List<PointF> simplex = new List<PointF>();
            PointF support = CalculateSupportPoint(poly1, poly2, dir);
            simplex.Add(support); // Initial corner of simplex
            dir = PointUtility.Normalize(PointUtility.Negate(simplex[0])); // New direction towards origo

            while (true)
            {
                PointF vecA = CalculateSupportPoint(poly1, poly2, dir);

                if (PointUtility.Dot(vecA, dir) < 0) return false; // the new point (A) did not cross the origo if the dot-product is negative. Since this is the furthest point towards origo, no intersection finds place
            
                simplex.Add(vecA);

                if (HandleSimplex(ref simplex, ref dir)) return true; // The simplex contains origo = intersection
            }
        }

        /// <summary>
        /// Handles the two different cases for the simplex (either a line or a triangle).
        /// Updates the direction variable sent as a reference for the next iteration of the algorithm.
        /// </summary>
        /// <param name="simplex">The simplex to handle</param>
        /// <param name="dir">The direction object to update based on the simplex</param>
        /// <returns>true if the simplex contains origo, false if not</returns>
        private static bool HandleSimplex(ref List<PointF> simplex, ref PointF dir)
        {
            if (simplex.Count == 2) return LineCase(simplex, ref dir);
            else return TriangleCase(ref simplex, ref dir);

            bool LineCase(List<PointF> s, ref PointF d)
            {
                PointF A = s[1], B = s[0];
                PointF AO = PointUtility.Negate(A); // Vector from A to Origo
                PointF AB = PointUtility.Subtract(B, A);
                PointF ABcross = TripleCrossProduct(AB, AO, AB); // (AB X AO) X AB. Gives the new direction (the normal of AB towards Origo).
                d = PointUtility.Normalize(ABcross);
                
                return false; // Origo can't be contained in a line
            }

            bool TriangleCase(ref List<PointF> s, ref PointF d)
            {
                PointF A = s[2], B = s[1], C = s[0];
                PointF AB = PointUtility.Subtract(B, A);
                PointF AC = PointUtility.Subtract(C, A);
                PointF AO = PointUtility.Negate(A);
                PointF ABcross = TripleCrossProduct(AC, AB, AB);
                PointF ACcross = TripleCrossProduct(AB, AC, AC);

                // Check region AB and AC. If either contains Origo, then the simplex does not.
                // If neither contains origo, then that implies the origo lies within the simplex (meaning there's an intersection).
                return CheckRegion(ref s, ref d, ABcross, AO) || CheckRegion(ref s, ref d, ACcross, AO);
            }

            bool CheckRegion(ref List<PointF> s, ref PointF d, PointF cross, PointF AO)
            {
                if (PointUtility.Dot(cross, AO) > 0)
                {
                    s.RemoveAt(0);
                    d = cross;
                    return false;
                }
                else return true;
            }
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
