using CityGrowthSim.Structures.Shapes;
using System.Drawing;

namespace CityGrowthSim.Structures
{
    internal class House : BaseStructure
    {
        IShape shape;

        public House(Point position) : base(position)
        {
            shape = SelectShape();
            Corners = shape.GenerateCorners(50, 50);
        }

        /// <summary>
        /// Select a fitting shape of the house for the current position
        /// </summary>
        /// <returns>A Shape object</returns>
        private IShape SelectShape()
        {
            return new RectangleShape();
        }
    }
}
