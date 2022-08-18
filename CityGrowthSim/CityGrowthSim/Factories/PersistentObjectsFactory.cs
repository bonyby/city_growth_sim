using CityGrowthSim.Managers;
using CityGrowthSim.Managers.Settings;
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
        SettingsManager settings;
        TimeManager time;

        Random random;
        ShapeFactory shapeFact;
        StructureFactory structFact;
        IVisualizer visualizer;

        public PersistentObjectsFactory(Main main)
        {
            this.main = main;
            CreateSettingsManager();
            CreateTimeManager();
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

        public TimeManager CreateTimeManager()
        {
            string hzString = settings.GetSettingsValue("SimulationHz");

            if (hzString == null) { return null; }

            int hz;
            bool isInt = int.TryParse(hzString, out hz);

            if (!isInt || hz <= 0) { Console.Error.WriteLine(string.Format("Not a valid simulationHz (should be an integer n > 0): {0}", hz)); return null; }

            if (time == null) time = new TimeManager(hz);
            return time;
        }
    }
}
