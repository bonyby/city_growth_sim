using CityGrowthSim.City.Structures.Shapes;
using CityGrowthSim.Utility.RandomWeightedSelection;
using System;

namespace CityGrowthSim.Factories
{
    internal class ShapeFactory
    {
        private Random random;
        private int lWeight = 34, rectWeight = 66;

        public ShapeFactory(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Creates a shape based on the supplied type. Returns null if no matching type.
        /// Available types: rect, lshape, random
        /// </summary>
        /// <param name="type">Type of shape</param>
        /// <returns>Shape object</returns>
        public IShape CreateShape(string type)
        {
            switch (type.ToLower())
            {
                case "rect":
                    return new RectangleShape(random);
                case "lshape":
                    return new LShape(random);
                case "random":
                    return GetRandomShape();
                default:
                    Console.Error.WriteLine(string.Format("Shape type not valied: {0}", type.ToLower()));
                    return null;
            }
        }

        private IShape GetRandomShape()
        {
            WeightedOption<IShape>[] options = new WeightedOption<IShape>[]
            {
                new WeightedOption<IShape>(lWeight, new LShape(random)),
                new WeightedOption<IShape>(rectWeight, new RectangleShape(random))
            };

            return RandomWeightedSelector.SelectRandomOption(options, random);
        }
    }
}
