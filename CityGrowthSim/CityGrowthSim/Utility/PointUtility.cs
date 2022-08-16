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

            Console.WriteLine("Centroid: " + "(" + c.X + ", " + c.Y + ")");

            // 'Move' centroid to origo relative to each point and rotate
            Point[] newPs = new Point[points.Length];
            double rad = degrees * (Math.PI / 180); // convert degrees to radians
            for (int i = 0; i < newPs.Length; i++)
            {
                Point p = points[i];
                Point newP = new Point(p.X - c.X, p.Y - c.Y);
                Console.WriteLine("Point " + i + ":" + newP);

                // Rotate based on rotation matrix R = [cosθ sinθ, -sinθ cosθ]
                // and move back to original centroid
                double cos = Math.Cos(rad);
                double sin = Math.Sin(rad);
                int newX = (int)(Math.Floor(newP.X * cos + newP.Y * sin + c.X));
                int newY = (int)(Math.Floor(-newP.X * sin + newP.Y * cos + c.Y));
                newP.X = newX;
                newP.Y = newY;

                newPs[i] = newP;
                Console.WriteLine("New point " + i + ":" + newP);
            }

            return newPs;
        }
    }
}
