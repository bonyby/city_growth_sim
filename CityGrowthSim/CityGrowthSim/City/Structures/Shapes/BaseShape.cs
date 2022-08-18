using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Structures.Shapes
{
    internal abstract class BaseShape : IShape
    {
        public Random Random { get; }

        public BaseShape(Random random)
        {
            Random = random;
        }

        public abstract Point[] GenerateCorners(uint width, uint height);
    }
}
