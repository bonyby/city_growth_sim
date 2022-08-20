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
        /// </summary>
        /// <param name="points">Points to generate minimum bounding box of</param>
        /// <returns>Points describing minimum bounding box</returns>
        public static Point[] GetMinimumBoundingBox(Point[] points)
        {
            // Takes inspiration from: https://github.com/cansik/LongLiveTheSquare

            throw new NotImplementedException();
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
}
