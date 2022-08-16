using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures.Shapes
{
    internal class LShape : BaseShape
    {
        //private uint minWidth = 15, sideMinWidth = 8;
        //private uint minHeight = 10, sideMinHeight = 5;
        //private uint minWidthDiff = 7;

        public LShape(Random random) : base(random) { }

        // Same as RectangleShape... Min. widths & heights are calculated with magic constants for now to have a basic prototype
        public override Point[] GenerateCorners(uint width, uint height)
        {
            int minWidth = (int)(width / 2);
            int minHeight = (int)(height / 10 * 4);
            int maxWidth = (int)(width / 10 * 8);
            int maxHeight = (int)(height / 10 * 6);

            // Width and height of main shape
            int w1 = Random.Next(minWidth, maxWidth);
            int h1 = Random.Next(minHeight, maxHeight);

            // Width and height of side shape
            int minSideWidth = w1 / 3;
            int minSideHeight = h1 / 4;
            int minWidthDiff = w1 / 5;

            int w2 = Random.Next(minSideWidth, w1 - minWidthDiff);
            int h2 = Random.Next(minSideHeight, h1 / 2);

            // Choose side for side shape
            Point[] points;
            if (Random.Next(2) == 0) { points = new Point[] { new Point(0, 0), new Point(w1, 0), new Point(w1, h1), new Point(w2, h1), new Point(w2, h1 + h2), new Point(0, h1 + h2) }; }
            else { points = new Point[] { new Point(0, 0), new Point(w1, 0), new Point(w1, h1 + h2), new Point(w1 - w2, h1 + h2), new Point(w1 - w2, h1), new Point(0, h1) }; }

            return points;
        }
    }
}
