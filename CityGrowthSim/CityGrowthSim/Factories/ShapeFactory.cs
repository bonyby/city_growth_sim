using CityGrowthSim.Structures.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Factories
{
    internal class ShapeFactory
    {
        private Random random;

        public ShapeFactory(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Creates a shape based on the supplied type.
        /// Available types: rect, lshape, random
        /// </summary>
        /// <param name="type">Type of shape</param>
        /// <returns>Shape object</returns>
        public IShape CreateShape(String type)
        {
            return new RectangleShape(random); // TODO: should be based on type-string
        }
    }
}
