using CityGrowthSim.City.Structures.Shapes;
using System;
using System.Drawing;

namespace CityGrowthSim.City.Structures
{
    internal class House : BaseStructure
    {
        IShape shape;

        public House(Point position, IShape shape, Size size) : base(position)
        {
            this.shape = shape;
            Corners = this.shape.GenerateCorners((uint)size.Width, (uint)size.Height);
        }

        public House(Point position, IShape shape, Point[] corners) : base(position)
        {
            this.shape = shape;
            Corners = corners;
        }
    }
}
