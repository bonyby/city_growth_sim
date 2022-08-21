using CityGrowthSim.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Structures
{
    internal abstract class BaseStructure : IStructure
    {

        Point position;
        Point[] corners;
        Point[] minBoundBox = null;
        bool minBoundBoxDirty = true;

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
                    cs[i].X += Position.X;
                    cs[i].Y += Position.Y;
                }

                return cs;
            }
        }

        public Point Position => position;

        public Point[] MinimumBoundingBox
        {
            get
            {
                if (minBoundBoxDirty) // Needs updating?
                {
                    minBoundBox = PointUtility.GetMinimumBoundingBox(GlobalCorners);
                    minBoundBoxDirty = false;
                }

                return minBoundBox;
            }
        }

        public Point[] RotateCorners(int degrees)
        {
            minBoundBoxDirty = true;
            Corners = PointUtility.RotatePointsAroundCentroid(Corners, degrees); // Points might have negative coordinates, so 
            return GlobalCorners;
        }
    }
}
