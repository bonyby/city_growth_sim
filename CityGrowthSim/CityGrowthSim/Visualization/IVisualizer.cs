using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Visualization
{
    public interface IVisualizer
    {
        /// <summary>
        /// Draws the world in its current state. Includes landscape, structures etc...
        /// </summary>
        void DrawWorld();
    }
}
