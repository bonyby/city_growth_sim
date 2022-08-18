using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Structures
{
    /// <summary>
    /// Interface for structure. Consider using the BaseStructure for inheritance instead of implementing this directly to ensure all criteria is satisfied
    /// </summary>
    public interface IStructure
    {
        /// <summary>
        /// Array containing all corner points of the structure itself relative to its position
        /// Only positive values allowed. Negative values would mean either left to or above (or both)
        /// compared to the position, which in turn would change the position. Negative values should
        /// be set to 0
        /// </summary>
        Point[] Corners { get; set; }

        /// <summary>
        /// Global position of the corners
        /// </summary>
        Point[] GlobalCorners { get; }

        /// <summary>
        /// Rotates and updates the corners of the structure based on the supplied degrees.
        /// Rotates around corner centroid (center of corners), NOT center of bounding box.
        /// A positive value rotates clockwise, a negative value rotates counterclockwise.
        /// </summary>
        /// <param name="degrees">Desired rotation in degrees</param>
        /// <returns>Point[] array containing updated global corner positions</returns>
        Point[] RotateCorners(int degrees);

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
