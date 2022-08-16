using CityGrowthSim.Structures.Shapes;
using System;
using System.Drawing;

namespace CityGrowthSim.Structures
{
    internal class House : BaseStructure
    {
        IShape shape;

        public House(Point position, IShape shape) : base(position)
        {
            this.shape = shape;
            Corners = this.shape.GenerateCorners(50, 50);
        }
    }
}
