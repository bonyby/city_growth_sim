using CityGrowthSim.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures
{
    internal abstract class BaseStructure : IStructure
    {

        Point position;
        Point[] corners;

        public BaseStructure(Point position)
        {
            this.position = position;
        }

        public BaseStructure(Point position, Point[] corners)
        {
            this.position = position;
            Corners = corners;
        }

        public Point[] Corners
        {
            get => corners;
            set
            {
                corners = value;
                // Might need to revisit. Corners might have negative coordinates after rotating.
                // Maybe offset all other points?
                //for (int i = 0; i < corners.Length; i++)
                //{
                //    if (corners[i].X < 0) { corners[i].X = 0; }
                //    if (corners[i].Y < 0) { corners[i].Y = 0; }
                //}
            }
        }

        /// <summary>
        /// Calculate global corner positions based on structure position and relative corner positions
        /// </summary>
        public Point[] GlobalCorners
        {
            get
            {
                Point[] cs = new Point[corners.Length];
                corners.CopyTo(cs, 0);

                for (int i = 0; i < cs.Length; i++)
                {
                    cs[i].X += this.Position.X;
                    cs[i].Y += this.Position.Y;
                }

                return cs;
            }
        }

        public Point Position => position;

        public int BoundingWidth
        {
            get
            {
                Point left = Corners[0], right = Corners[0];

                for (int i = 0; i < Corners.Length; i++)
                {
                    if (Corners[i].X < left.X) { left = Corners[i]; continue; }
                    if (Corners[i].X > right.X) { right = Corners[i]; continue; }
                }

                return right.X - left.X;
            }
        }

        public int BoundingHeight
        {
            get
            {
                Point up = Corners[0], down = Corners[0];

                for (int i = 0; i < Corners.Length; i++)
                {
                    if (Corners[i].Y < up.Y) { up = Corners[i]; continue; }
                    if (Corners[i].Y > down.Y) { down = Corners[i]; continue; }
                }
                return down.Y - up.Y;
            }
        }

        public Point[] RotateCorners(int degrees)
        {
            Corners = PointUtility.RotatePointsAroundCentroid(Corners, degrees); // Points might have negative coordinates, so 
            return GlobalCorners;
        }
    }
}
