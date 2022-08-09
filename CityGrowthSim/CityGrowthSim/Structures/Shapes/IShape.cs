using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures.Shapes
{
    
    internal interface IShape
    {
        /// <summary>
        /// Generates an array of corners complying with the given shape.
        /// Points are ordered based on drawing order.
        /// </summary>
        /// <param name="width">Width of the bounding box. Is set to min. required width if less</param>
        /// <param name="height">Height og the bounding box. Is set to min. required height if less</param>
        /// <returns>Array of Point objects</returns>
        Point[] GenerateCorners(uint width, uint height);
    }
}
