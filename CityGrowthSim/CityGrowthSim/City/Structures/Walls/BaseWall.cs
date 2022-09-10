using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Structures.Walls
{
    internal abstract class BaseWall : BaseStructure
    {
        protected BaseWall(Point position, float width) : base(position)
        {
            this.Width = width;
        }

        protected BaseWall(Point position, Point[] corners, float width) : base(position, corners)
        {
            this.Width = width;
        }

        public float Width { get; }
    }
}
