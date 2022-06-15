using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures
{
    internal class House : IStructure
    {
        Point[] corners;
        public Point[] Corners => corners;
        public Point Position => throw new NotImplementedException();
        public int BoundingWidth => throw new NotImplementedException();
        public int BoundingHeight => throw new NotImplementedException();

        public House()
        {
            this.corners = new Point[] { new Point(0, 0), new Point(100, 0), new Point(100, 100), new Point(0, 100) };
        }
    }
}
