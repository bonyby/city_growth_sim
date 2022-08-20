using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Utility
{
    static internal class PointUtility
    {
        public static Point[] RotatePointsAroundCentroid(Point[] points, double degrees)
        {
            if (points == null || points.Length == 0) { return new Point[0]; }

            // Calculate centroid
            Point c = new Point(0, 0);
            foreach (Point p in points)
            {
                c.X += p.X;
                c.Y += p.Y;
            }

            c.X = (int)Math.Floor((double)c.X / points.Length);
            c.Y = (int)Math.Floor((double)c.Y / points.Length);

            // 'Move' centroid to origo relative to each point and rotate
            Point[] newPs = new Point[points.Length];
            double rad = degrees * (Math.PI / 180); // convert degrees to radians
            for (int i = 0; i < newPs.Length; i++)
            {
                Point p = points[i];
                Point newP = new Point(p.X - c.X, p.Y - c.Y);

                // Rotate based on rotation matrix R = [cosθ sinθ, -sinθ cosθ]
                // and move back to original centroid
                double cos = Math.Cos(rad);
                double sin = Math.Sin(rad);
                int newX = (int)(Math.Floor(newP.X * cos + newP.Y * sin + c.X));
                int newY = (int)(Math.Floor(-newP.X * sin + newP.Y * cos + c.Y));
                newP.X = newX;
                newP.Y = newY;

                newPs[i] = newP;
            }

            return newPs;
        }

        /// <summary>
        /// Calculates the minimum bounding box of the points.
        /// This bounding box is not necessarily axis-aligned and is thus the true minimum bounding box (compared to the world bounding box, which is axis aligned and might not yield the smallest bounding box)
        /// Assumes |points| >= 3. Undefined behaviour for |points| < 3.
        /// </summary>
        /// <param name="points">Points to generate minimum bounding box of</param>
        /// <returns>Points describing minimum bounding box</returns>
        public static Point[] GetMinimumBoundingBox(Point[] points)
        {
            // Takes inspiration from: https://github.com/cansik/LongLiveTheSquare

            Point[] convexHull = GetConvexHull(points);

            // Find minimum bounding box from convex hull
            Segment[] segments = ConvertConvexHullToLineSegments(convexHull);

            // For each segment s, rotate figure s.t. s is parallel with x-axis and find minX, maxX, minY, maxY
            // and create axis-aligned bounding box. Save if best area seen so far
            double bestArea = double.MaxValue;
            Point[] minBBox = null;
            foreach (Segment s in segments)
            {
                Point sVector = new Point(s.E2.X - s.E1.X, s.E2.Y - s.E1.Y);
                double polarAngle = Math.Atan2(sVector.Y, sVector.X) / (Math.PI / 180); // in degrees

                Point[] candidate = RotatePointsAroundCentroid(convexHull, polarAngle);
                (Point[] cand, double area) = GetCandidateBBoxAndArea(candidate);

                if (area < bestArea)
                {
                    minBBox = RotatePointsAroundCentroid(cand, -polarAngle);
                }
            }

            return minBBox;

            (Point[], double) GetCandidateBBoxAndArea(Point[] candidate)
            {
                int minX = candidate[0].X, maxX = candidate[0].X, minY = candidate[0].Y, maxY = candidate[0].Y;

                foreach (Point p in candidate)
                {
                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }

                Point[] MBBox = new Point[] { new Point(minX, minY), new Point(minX, maxY), new Point(maxX, maxY), new Point(maxX, minY) };
                double area = (maxX - minX) * (maxY - minY);
                return (MBBox, area);
            }
        }

        /// <summary>
        /// Converts the convex hull from Point[] to Segment[]
        /// </summary>
        /// <param name="convexHull">Convex hull to convert</param>
        /// <returns>Segments of convex hull</returns>
        private static Segment[] ConvertConvexHullToLineSegments(Point[] convexHull)
        {
            Segment[] segments = new Segment[convexHull.Length];
            for (int i = 1; i < segments.Length; i++)
            {
                segments[i - 1] = new Segment(convexHull[i - 1], convexHull[i]);
            }
            segments[segments.Length - 1] = new Segment(convexHull[convexHull.Length - 1], convexHull[0]); // Add segment to close the gap from last to first point in convex hull
            return segments;
        }

        /// <summary>
        /// Calculates the convex hull of the provided points.
        /// </summary>
        /// <param name="points">Points to generate convex hull of</param>
        /// <returns>Points describing convex hull</returns>
        public static Point[] GetConvexHull(Point[] points)
        {
            return MonotoneChain.GetConvexHull(points);
        }
    }

    /// <summary>
    /// A line segment
    /// </summary>
    struct Segment
    {
        public Segment(Point e1, Point e2)
        {
            E1 = e1;
            E2 = e2;
        }

        public Point E1 { get; }
        public Point E2 { get; }
    }
}
