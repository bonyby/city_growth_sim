﻿using CityGrowthSim.City.Structures;
using CityGrowthSim.City.Structures.Shapes;
using CityGrowthSim.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Factories
{
    internal class StructureFactory
    {
        private Random random;
        private ShapeFactory shapeFact;
        private Size defaultHouseSize = new Size(50, 50);

        public StructureFactory(Random random, ShapeFactory shapeFact)
        {
            this.random = random;
            this.shapeFact = shapeFact;
        }

        /// <summary>
        /// Creates a house with a random shape with the top-left corner in the supplied position with the default size.
        /// </summary>
        /// <param name="position">The position of the top-left corner of the house created</param>
        /// <returns>The house</returns>
        public IStructure CreateHouse(Point position)
        {
            //Console.WriteLine("Position: " + position);
            //House h = new House(new Point(random.Next(750), random.Next(500)), shapeFact.CreateShape("random"));
            House h = new House(position, shapeFact.CreateShape("random"), defaultHouseSize);
            return h;
        }

        /// <summary>
        /// Creates a house with a random shape within the plot supplied.
        /// </summary>
        /// <param name="plot">The plot where the house should be created. Assumed to be a rectangle</param>
        /// <returns>The house</returns>
        public IStructure CreateHouse(Point[] plot)
        {
            // Calculate the width and height of the plot.
            // The plot might be rotated, so can't take simply take the difference for each axis
            Point p_0 = PointUtility.FurthestPointInDirection(plot, PointUtility.Normalize(new PointF(-1, -1))); // Define p_0 to be the point furthest up in the left corner
            Point p_1 = PointUtility.FurthestPointInDirection(plot, PointUtility.Normalize(new PointF(1, -1)));  // Define p_1 to be the point furthest up in the right corner (to the right of p_0)
            Point p_2 = PointUtility.FurthestPointInDirection(plot, PointUtility.Normalize(new PointF(1, 1)));   // Define p_2 to be the point furthest down in the right corner (under p_1)
            double width = PointUtility.Distance(p_0, p_1);
            double height = PointUtility.Distance(p_1, p_2);

            if (width == 0 && height == 0) return null;

            Console.WriteLine("--Plot--");
            foreach (Point point in plot)
            {
                Console.WriteLine(point);
            }

            // Create a shape within the boundaries of the plot
            PointF dirp_0p_1 = PointUtility.DirectionTo(p_0, p_1);
            double angle = PointUtility.Angle(dirp_0p_1);
            Console.WriteLine("Angle: " + angle);
            IShape shape = shapeFact.CreateShape("random");
            PointF[] shapeCorners = PointUtility.ConvertPointsToPointFs(shape.GenerateCorners((uint)width, (uint)height));
            shapeCorners = PointUtility.RotatePointsAroundPointPrecise(shapeCorners, angle, PointUtility.ConvertPointToPointF(p_0));

            return new House(p_0, shape, PointUtility.ConvertPointFsToPoints(shapeCorners));
        }
    }
}
