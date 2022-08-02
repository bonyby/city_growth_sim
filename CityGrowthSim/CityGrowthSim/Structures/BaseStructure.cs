using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures
{
    internal class BaseStructure : IStructure
    {
        Point[] corners;
        public Point[] Corners
        {
            get => corners;
            set
            {
                corners = value;
                for (int i = 0; i < corners.Length; i++)
                {
                    if (corners[i].X < 0) { corners[i].X = 0; }
                    if (corners[i].Y < 0) { corners[i].Y = 0; }
                }
            }
        }

        public Point Position => throw new NotImplementedException();

        public int BoundingWidth => throw new NotImplementedException();

        public int BoundingHeight => throw new NotImplementedException();
    }
}
