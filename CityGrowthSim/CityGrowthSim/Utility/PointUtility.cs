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
        /// <summary>
        /// Rotates the given points around the given point by the given degrees.
        /// </summary>
        /// <param name="points">Points to rotate</param>
        /// <param name="degrees">Degrees to rotate</param>
        /// <param name="rotatePoint">Point to rotate around</param>
        /// <returns>Rotated points</returns>
        public static PointF[] RotatePointsAroundPointF(PointF[] points, double degrees, PointF rotatePoint)
        {
            if (points == null || points.Length == 0) { return new PointF[0]; }

            // 'Move' rotate point to origo relative to each point and rotate
            PointF[] newPs = new PointF[points.Length];
            double rad = degrees * (Math.PI / 180); // convert degrees to radians
            for (int i = 0; i < newPs.Length; i++)
            {
                PointF p = points[i];
                PointF newP = new PointF(p.X - rotatePoint.X, p.Y - rotatePoint.Y);

                // Rotate based on rotation matrix R = [cosθ sinθ, -sinθ cosθ]
                // and move back to original rotate point
                double cos = Math.Cos(rad);
                double sin = Math.Sin(rad);
                double newX = newP.X * cos + newP.Y * sin + rotatePoint.X;
                double newY = -newP.X * sin + newP.Y * cos + rotatePoint.Y;
                newP.X = (float)newX;
                newP.Y = (float)newY;

                //Console.WriteLine("newX (double): " + newX + " newX (float): " + newP.X);
                //Console.WriteLine("newY (double): " + newY + " newY (float): " + newP.Y);

                newPs[i] = newP;
            }

            return newPs;
        }

        /// <summary>
        /// Rotates the given points around the centroid of them by the given degrees.
        /// </summary>
        /// <param name="points">Points to rotate</param>
        /// <param name="degrees">Degrees to rotate</param>
        /// <returns>Rotated points</returns>
        public static PointF[] RotatePointsAroundCentroidPrecise(PointF[] points, double degrees)
        {
            PointF c = CalculateCentroid(points);

            return RotatePointsAroundPointF(points, degrees, c);
        }

        /// <summary>
        /// Rotates the given points around the centroid of them by the given degrees.
        /// </summary>
        /// <param name="points">Points to rotate</param>
        /// <param name="degrees">Degrees to rotate</param>
        /// <returns>Rotated points</returns>
        public static Point[] RotatePointsAroundCentroid(Point[] points, double degrees)
        {
            PointF[] ps = ConvertPointsToPointFs(points);

            ps = RotatePointsAroundCentroidPrecise(ps, degrees);

            return ConvertPointFsToPoints(ps);
        }

        /// <summary>
        /// Convert a Point[] to PointF[]
        /// </summary>
        /// <param name="points">Point[] to convert</param>
        /// <returns>Converted PointF[]</returns>

        public static PointF[] ConvertPointsToPointFs(Point[] points)
        {
            PointF[] newPs = new PointF[points.Length];
            for (int i = 0; i < newPs.Length; i++)
            {
                newPs[i] = ConvertPointToPointF(points[i]);
            }

            return newPs;
        }

        public static PointF ConvertPointToPointF(Point p)
        {
            return new PointF(p.X, p.Y);
        }

        /// <summary>
        /// Convert a PointF[] to Point[]
        /// </summary>
        /// <param name="points">PointF[] to convert</param>
        /// <returns>Converted Point[]</returns>
        public static Point[] ConvertPointFsToPoints(PointF[] points)
        {
            //Console.WriteLine("PointF:");
            //foreach (PointF pf in points)
            //{
            //    Console.WriteLine(pf);
            //}
            //Console.WriteLine("--");

            Point[] newPs = new Point[points.Length];
            for (int i = 0; i < newPs.Length; i++)
            {
                newPs[i] = ConvertPointFToPoint(points[i]);
            }

            return newPs;
        }

        public static Point ConvertPointFToPoint(PointF p)
        {
            return new Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        /// <summary>
        /// Calculates the minimum bounding box of the points.
        /// This bounding box is not necessarily axis-aligned and is thus the true minimum bounding box (compared to the world bounding box, which is axis aligned and might not yield the smallest bounding box)
        /// Assumes |points| >= 3. Undefined behaviour for |points| < 3.
        /// DISCLAIMER & TODO: Seems to sometimes (rarely) give an incorrect MBBox.. Don't know why (maybe some rotation error of candidates). Not important right now as it's rare, but should be looked at at some point.
        /// </summary>
        /// <param name="points">Points to generate minimum bounding box of</param>
        /// <returns>Points describing minimum bounding box</returns>
        public static Point[] GetMinimumBoundingBox(Point[] points)
        {
            // Takes inspiration from: https://github.com/cansik/LongLiveTheSquare

            Point[] convexHull = GetConvexHull(points);
            PointF[] convexHullF = ConvertPointsToPointFs(convexHull);
            PointF centroid = CalculateCentroid(convexHullF);

            // Find minimum bounding box from convex hull
            Segment[] segments = ConvertConvexHullToLineSegments(convexHull);

            // For each segment s, rotate figure s.t. s is parallel with x-axis and find minX, maxX, minY, maxY
            // and create axis-aligned bounding box. Save if best area seen so far
            double bestArea = double.MaxValue;
            PointF[] minBBox = null;
            foreach (Segment s in segments)
            {
                Point sVector = new Point(s.E2.X - s.E1.X, s.E2.Y - s.E1.Y);
                double polarAngle = Math.Atan2(sVector.Y, sVector.X) / (Math.PI / 180); // in degrees

                //Console.WriteLine("New candidate");
                PointF[] candidate = RotatePointsAroundPointF(convexHullF, polarAngle, centroid);
                (PointF[] cand, double area) = GetCandidateBBoxAndArea(candidate);

                if (area < bestArea)
                {
                    //Console.WriteLine("Found new best");
                    minBBox = RotatePointsAroundPointF(cand, -polarAngle, centroid);
                }
            }

            return ConvertPointFsToPoints(minBBox);

            (PointF[], double) GetCandidateBBoxAndArea(PointF[] candidate)
            {
                float minX = candidate[0].X, maxX = candidate[0].X, minY = candidate[0].Y, maxY = candidate[0].Y;

                foreach (PointF p in candidate)
                {
                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }

                PointF[] MBBox = new PointF[] { new PointF(minX, minY), new PointF(minX, maxY), new PointF(maxX, maxY), new PointF(maxX, minY) };
                double area = (maxX - minX) * (maxY - minY);
                return (MBBox, area);
            }
        }

        /// <summary>
        /// Moves all points by the given offset.
        /// </summary>
        /// <param name="points">Points to move</param>
        /// <param name="offset">Offset to move</param>
        /// <returns>New points moved by the specified offset</returns>
        public static Point[] Move(Point[] points, Point offset)
        {
            PointF[] ps = ConvertPointsToPointFs(points);
            PointF off = ConvertPointToPointF(offset);
            return ConvertPointFsToPoints(Move(ps, off));
        }

        /// <summary>
        /// Moves all points by the given offset.
        /// </summary>
        /// <param name="points">Points to move</param>
        /// <param name="offset">Offset to move</param>
        /// <returns>New points moved by the specified offset</returns>
        public static PointF[] Move(PointF[] points, PointF offset)
        {
            PointF[] temp = new PointF[points.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = new PointF(points[i].X + offset.X, points[i].Y + offset.Y);
            }

            return temp;
        }

        /// <summary>
        /// Rotates the given point around origo by the specified degrees as if it was a vector.
        /// </summary>
        /// <param name="point">Point to rotate</param>
        /// <param name="degrees">Degrees to rotate</param>
        /// <returns>The rotate point</returns>
        public static PointF Rotate(PointF point, double degrees)
        {
            PointF newP = new PointF(point.X, point.Y);
            double rad = degrees * (Math.PI / 180);
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);
            double newX = newP.X * cos + newP.Y * sin;
            double newY = -newP.X * sin + newP.Y * cos;
            newP.X = (float)newX;
            newP.Y = (float)newY;

            return newP;
        }

        /// <summary>
        /// Normalizes the PointF as if it was a vector describing the given point
        /// </summary>
        /// <param name="point">PointF to normalize</param>
        /// <returns>The normalized PointF</returns>
        public static PointF Normalize(PointF point)
        {
            double len = Math.Sqrt(point.X * point.X + point.Y * point.Y);
            return new PointF((float)(point.X / len), (float)(point.Y / len));
        }

        /// <summary>
        /// Calculates the distance between points p1 and p2
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>The distance between p1 and p2</returns>
        public static double Distance(PointF p1, PointF p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        /// Subtracts the second point from the first coordinate wise
        /// </summary>
        /// <param name="p1">Point to subtract from</param>
        /// <param name="p2">Point to subtract</param>
        /// <returns>PointF describing p1-p2</returns>
        public static PointF Subtract(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }

        /// <summary>
        /// Adds the second point to the first coordinate wise
        /// </summary>
        /// <param name="p1">Point to add to</param>
        /// <param name="p2">Point to add</param>
        /// <returns>PointF describing p1+p2</returns>
        public static PointF Add(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        /// <summary>
        /// Calculates the dot product between the points as if they were vectors.
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Dot product between p1 and p2</returns>
        public static double Dot(PointF p1, PointF p2)
        {
            return (p1.X * p2.X + p1.Y * p2.Y);
        }

        /// <summary>
        /// Negates the coordinates of a point (essentially flips the direction of the corresponding vector)
        /// </summary>
        /// <param name="point">Point to negate</param>
        /// <returns>Negated point</returns>
        public static PointF Negate(PointF point)
        {
            return new PointF(-point.X, -point.Y);
        }

        /// <summary>
        /// Multiplies each of the coordinates of the PointF by the supplied factor
        /// </summary>
        /// <param name="point">Point to multiply</param>
        /// <param name="a">Multiplication factor</param>
        /// <returns>New PointF with factor a multiplied onto the coordinates of point</returns>
        public static PointF Multiply(PointF point, float a)
        {
            return new PointF(point.X * a, point.Y * a);
        }

        /// <summary>
        /// Returns a new normalized PointF describing the direction from A to B
        /// </summary>
        /// <param name="A">The 'from' PointF</param>
        /// <param name="B">The 'to' PointF</param>
        /// <returns>A normal vector (PointF) from A to B</returns>
        public static PointF DirectionTo(PointF A, PointF B)
        {
            return Normalize(new PointF(B.X - A.X, B.Y - A.Y));
        }

        /// <summary>
        /// Calculates the angle (in degrees) of a given point relative to the x-axis as if the point is a vector.
        /// </summary>
        /// <param name="point">Point/vector</param>
        /// <returns>Angle between the point and the x-axis in degrees</returns>
        public static double Angle(PointF point)
        {
            return Math.Atan2(point.Y, point.X) * (180 / Math.PI);
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

        /// <summary>
        /// Calculates the centroid of the shape described by the points
        /// </summary>
        /// <param name="points">Points describing shape</param>
        /// <returns>Centroid of shape as PointF</returns>
        public static PointF CalculateCentroid(PointF[] points)
        {
            PointF c = new PointF(0, 0);
            foreach (PointF p in points)
            {
                c.X += p.X;
                c.Y += p.Y;
            }

            c.X = c.X / points.Length;
            c.Y = c.Y / points.Length;

            //Console.WriteLine("Centroid: " + c);
            return c;
        }

        /// <summary>
        /// Calculates whether or not the two polygons described by the Point's intersect each other.
        /// </summary>
        /// <param name="poly1">First polygon</param>
        /// <param name="poly2">Second polygon</param>
        /// <returns>true if the polygons intersect, false if not</returns>
        public static bool CheckPolygonsIntersecting(Point[] poly1, Point[] poly2)
        {
            return CheckPolygonsIntersecting(ConvertPointsToPointFs(poly1), ConvertPointsToPointFs(poly2));
        }

        /// <summary>
        /// Calculates whether or not the two polygons described by the PointF's intersect each other.
        /// </summary>
        /// <param name="poly1">First polygon</param>
        /// <param name="poly2">Second polygon</param>
        /// <returns>true if the polygons intersect, false if not</returns>
        public static bool CheckPolygonsIntersecting(PointF[] poly1, PointF[] poly2)
        {
            return GJK.PolygonsIntersecting(poly1, poly2);
        }

        /// <summary>
        /// Calculates the point furthest in the given direction of the supplied points
        /// </summary>
        /// <param name="points">Points to compare</param>
        /// <param name="dir">Direction to find the furthest point in</param>
        /// <returns>The point furthest in the specified direction</returns>
        public static Point FurthestPointInDirection(Point[] points, PointF dir)
        {
            return ConvertPointFToPoint(FurthestPointInDirection(ConvertPointsToPointFs(points), dir));
        }

        /// <summary>
        /// Calculates the point furthest in the given direction of the supplied points
        /// </summary>
        /// <param name="points">Points to compare</param>
        /// <param name="dir">Direction to find the furthest point in</param>
        /// <returns>The point furthest in the specified direction</returns>
        public static PointF FurthestPointInDirection(PointF[] points, PointF dir)
        {
            return GJK.FurthestPointInDirection(points, dir);
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
