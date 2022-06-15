using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Structures
{
    public interface IStructure
    {
        /// <summary>
        /// Array containing all corner points of the structure itself relative to its position
        /// </summary>
        Point[] Corners { get; }
        
        /// <summary>
        /// Position of the top left corner of the bounding box
        /// </summary>
        Point Position { get; }

        /// <summary>
        /// The width of the bounding box containing the structure/corners
        /// </summary>
        int BoundingWidth { get; }

        /// <summary>
        /// The height of the bounding box containing the structure/corners
        /// </summary>
        int BoundingHeight { get; }
    }
}
