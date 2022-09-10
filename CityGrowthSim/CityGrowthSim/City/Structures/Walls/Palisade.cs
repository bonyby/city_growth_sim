using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Structures.Walls
{
    internal class Palisade : BaseWall
    {
        public Palisade(Point position, float width) : base(position, width) { }

        public Palisade(Point position, Point[] corners, float width) : base(position, corners, width) { }
    }
}
