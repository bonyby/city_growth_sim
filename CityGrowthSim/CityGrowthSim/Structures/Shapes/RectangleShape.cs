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
        public RectangleShape(Random random) : base(random) { }

        public override Point[] GenerateCorners(uint width, uint height)
        {
            // Just to ensure a minimum of the allotted space is used.
            // Should find a better solution in the future...
            uint minWidth = width / 2;
            uint minHeight = height / 2;

            if (width <= minWidth) { width = minWidth; }
            if (height <= minHeight) { height = minHeight; }

            int w = Random.Next((int)minWidth, (int)width + 1);
            int h = Random.Next((int)minHeight, (int)height + 1);

            return new Point[] { new Point(0, 0), new Point(w, 0), new Point(w, h), new Point(0, h) };
        }
    }
}
