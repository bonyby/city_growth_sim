using CityGrowthSim.City;
using CityGrowthSim.Managers;
using CityGrowthSim.Managers.Settings;
using CityGrowthSim.Visualization;
using CityGrowthSim.Visualization.TerrainStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Factories
{
    internal class PersistentObjectsFactory
    {
        Main main;
        SettingsManager settingsManager;
        TimeManager timeManager;
        CityPlanner cityPlanner;
        IVisualizer visualizer;

        Random random;
        ShapeFactory shapeFact;
        StructureFactory structFact;

        public PersistentObjectsFactory(Main main)
        {
            this.main = main;
            CreateSettingsManager();
            CreateTimeManager();
            CreateCityPlanner();
            CreateVisualizer();
        }

        public Random CreateRandom()
        {
            if (random == null)
            {
                string seed = settingsManager.GetSettingsValue("RandomObjectSeed");

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

            if (visualizer == null)
            {
                // TODO: Strategies, colors etc. should be selected through preferences/settings at some point
                ITerrainVisualizationStrategy terrainStrat = new SolidColorTerrainVisualizationStrategy(Color.LawnGreen);

                visualizer = new StandardVisualizer(main, CreateTimeManager(), CreateCityPlanner(), terrainStrat);
            }
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
            if (settingsManager == null) settingsManager = new SettingsManager();
            return settingsManager;
        }

        public TimeManager CreateTimeManager()
        {
            if (timeManager == null)
            {
                string hzString = settingsManager.GetSettingsValue("SimulationHz");

                if (hzString == null) { return null; }

                int hz;
                bool isInt = int.TryParse(hzString, out hz);

                if (!isInt || hz <= 0) { Console.Error.WriteLine(string.Format("Not a valid simulationHz (should be an integer n > 0): {0}", hz)); return null; }

                timeManager = new TimeManager(hz);
            }

            return timeManager;
        }

        public CityPlanner CreateCityPlanner()
        {
            if (cityPlanner == null) cityPlanner = new CityPlanner(CreateTimeManager(), CreateStructureFactory());
            return cityPlanner;
        }
    }
}
