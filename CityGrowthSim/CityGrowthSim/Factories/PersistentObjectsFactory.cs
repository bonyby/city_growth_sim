using CityGrowthSim.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Factories
{
    internal class PersistentObjectsFactory
    {
        Main main;
        Random random;
        IVisualizer visualizer;

        public PersistentObjectsFactory(Main main)
        {
            this.main = main;
        }

        public Random CreateRandom()
        {
            if (random == null) random = new Random();
            return random;
        }

        public IVisualizer CreateVisualizer()
        {
            if (visualizer == null) visualizer = new StandardVisualizer(main);
            return visualizer;
        }
    }
}
