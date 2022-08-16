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
        ShapeFactory shapeFact;
        StructureFactory structFact;
        IVisualizer visualizer;

        public PersistentObjectsFactory(Main main)
        {
            this.main = main;
        }

        public Random CreateRandom()
        {
            if (random == null) random = new Random(); // Should seed based on settings
            return random;
        }

        public IVisualizer CreateVisualizer()
        {
            if (visualizer == null) visualizer = new StandardVisualizer(main); // Should select based on settings
            return visualizer;
        }

        public StructureFactory CreateStructureFactory()
        {
            if (structFact == null) structFact = new StructureFactory(CreateRandom(), CreateShapeFactory());
            return structFact;
        }

        public ShapeFactory CreateShapeFactory()
        {
            if (shapeFact == null) shapeFact = new ShapeFactory(CreateRandom());
            return shapeFact;
        }
    }
}
