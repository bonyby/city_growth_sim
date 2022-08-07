using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures
{
    internal class House : BaseStructure
    {
        public House(Point position, Point[] corners) : base(position, corners) { }
    }
}
