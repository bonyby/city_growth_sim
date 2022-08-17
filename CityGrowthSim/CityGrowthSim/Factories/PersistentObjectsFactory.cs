using CityGrowthSim.Settings;
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
        SettingsManager settings;
        IVisualizer visualizer;

        public PersistentObjectsFactory(Main main)
        {
            this.main = main;
            CreateSettingsManager();
        }

        public Random CreateRandom()
        {
            if (random == null)
            {
                string seed = settings.GetSettingsValue("RandomObjectSeed");

                int numb;
                bool is32BitNumber = int.TryParse(seed, out numb);

                if (seed == null || seed.ToLower() == "random" || !is32BitNumber) random = new Random();
                else
                {
                    random = new Random(numb);
                }
            }

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

        public SettingsManager CreateSettingsManager()
        {
            if (settings == null) settings = new SettingsManager();
            return settings;
        }
    }
}
