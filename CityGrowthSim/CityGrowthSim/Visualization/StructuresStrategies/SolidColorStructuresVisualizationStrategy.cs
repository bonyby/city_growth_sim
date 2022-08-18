using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Visualization.StructuresStrategies
{
    internal class SolidColorStructuresVisualizationStrategy : IStructuresVisualizationStrategy
    {
        public Color Color { get; }

        private CityPlanner cityPlanner;

        public SolidColorStructuresVisualizationStrategy(Color color, CityPlanner cityPlanner)
        {
            this.Color = color;
            this.cityPlanner = cityPlanner;
        }

        public void DrawStructures(Graphics graphics)
        {
            Brush b = new SolidBrush(Color);
            List<IStructure> structures = cityPlanner.GetAllStructures();

            foreach (IStructure s in structures)
            {
                graphics.FillPolygon(b, s.GlobalCorners);
            }

            b.Dispose();
        }
    }
}
