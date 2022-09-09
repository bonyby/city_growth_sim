using CityGrowthSim.City.Structures;
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
        /// Tries to push the house towards the edge of the plot in the alignment direction.
        /// </summary>
        /// <param name="plot">The plot where the house should be created. Assumed to be a rectangle</param>
        /// <param name="alignmentDir">The direction to push the house as far as possible (inside of the plot)</param>
        /// <returns>The house</returns>
        public IStructure CreateHouse(Point[] plot, PointF alignmentDir)
        {
            // Calculate the width and height of the plot.
            // The plot might be rotated, so can't take simply take the difference for each axis
            PointF normAlignDir = PointUtility.Normalize(alignmentDir);
            Point p_0 = PointUtility.FurthestPointInDirection(plot, normAlignDir); // Define p_0 to be the point furthest in the alignment direction
            Point p_1 = PointUtility.FurthestPointInDirection(plot, PointUtility.Rotate(normAlignDir, 90));  // Define p_1 to be the point furthest in orthogonal direction to the alignment direction (to the right)
            Point p_2 = PointUtility.FurthestPointInDirection(plot, PointUtility.Negate(normAlignDir));   // Define p_2 to be the point furthest in the opposite direction of the alignment direction
            double width = PointUtility.Distance(p_0, p_1);
            double height = PointUtility.Distance(p_1, p_2);

            if (width == 0 || height == 0) return null;

            // Create a shape within the boundaries of the plot
            IShape shape = shapeFact.CreateShape("random");
            PointF[] shapeCorners = PointUtility.ConvertPointsToPointFs(shape.GenerateCorners((uint)width, (uint)height));
            
            PointF p_0p_1 = PointUtility.DirectionTo(p_0, p_1);
            double angle = PointUtility.Angle(p_0p_1); 
            shapeCorners = PointUtility.RotatePointsAroundCentroidPrecise(shapeCorners, -angle);

            // Find the anchor of the house (the corner furthest in the direction of alignment).
            // Move the shape such that the anchor aligns perfectly with the corner of the plot
            PointF anchor = PointUtility.FurthestPointInDirection(shapeCorners, normAlignDir);
            PointF offset = PointUtility.Negate(anchor);
            shapeCorners = PointUtility.Move(shapeCorners, offset);

            return new House(p_0, shape, PointUtility.ConvertPointFsToPoints(shapeCorners));
        }
    }
}
