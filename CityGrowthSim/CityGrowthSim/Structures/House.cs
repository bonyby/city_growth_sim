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
        public House()
        {
            Corners = new Point[] { new Point(0, 0), new Point(100, 0), new Point(100, 100), new Point(0, 100) };
        }
    }
}
