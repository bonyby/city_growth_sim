using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures.Shapes
{
    internal class RectangleShape : BaseShape
    {
        private uint minWidth = 5;
        private uint minHeight = 5;

        public RectangleShape(Random random) : base(random) { }

        public override Point[] GenerateCorners(uint width, uint height)
        {
            if (width <= minWidth) { width = minWidth; }
            if (height <= minHeight) { height = minHeight; }

            Random random = new Random();
            int w = random.Next((int)minWidth, (int)width + 1);
            int h = random.Next((int)minHeight, (int)height + 1);

            return new Point[] { new Point(0, 0), new Point(w, 0), new Point(w, h), new Point(0, h) };
        }
    }
}
