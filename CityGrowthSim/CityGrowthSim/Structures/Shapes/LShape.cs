using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures.Shapes
{
    internal class LShape : IShape
    {
        private uint minWidth = 15, sideMinWidth = 8;
        private uint minHeight = 10, sideMinHeight = 5;
        private uint minWidthDiff = 7;

        public Point[] GenerateCorners(uint width, uint height)
        {
            if (width <= minWidth) { width = minWidth; }
            if (height <= minHeight) { height = minHeight; }

            Random random = new Random();
            
            // Width and height of main shape
            int w1 = random.Next((int)minWidth, (int)(width / 2));
            int h1 = random.Next((int)minHeight, (int)(height / 2));

            // Width and height of side shape
            int w2 = random.Next((int)sideMinWidth, (int)Math.Max(sideMinWidth, w1 - minWidthDiff));
            int h2 = random.Next((int)sideMinHeight, (int)(height / 2));

            // Choose side for side shape
            Point[] points;
            if (random.Next(2) == 0) { points = new Point[] { new Point(0, 0), new Point(w1, 0), new Point(w1, h1), new Point(w2, h1), new Point(w2, h1 + h2), new Point(0, h1 + h2) }; }
            else { points = new Point[] { new Point(0, 0), new Point(w1, 0), new Point(w1, h1 + h2), new Point(w1 - w2, h1 + h2), new Point(w1 - w2, h1), new Point(0, h1) }; }

            // Rotate shape
            // TODO

            return points;
        }
    }
}
