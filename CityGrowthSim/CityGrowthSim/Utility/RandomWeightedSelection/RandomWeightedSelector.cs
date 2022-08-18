using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Utility.RandomWeightedSelection
{
    static class RandomWeightedSelector
    {

        /// <summary>
        /// Selects an option randomly based on the weights for each option.
        /// </summary>
        /// <typeparam name="T">Type of the options' objects</typeparam>
        /// <param name="options">Array containing all options</param>
        /// <param name="random">Random object to use for selecting option</param>
        /// <returns>Chosen options' object. Default value if no options</returns>
        public static T SelectRandomOption<T>(WeightedOption<T>[] options, Random random)
        {
            if (options.Length == 0) return default(T);

            // Idea is to sum all weights and generate a random number n in [0;sum-1].
            // Each option subtracts it's weight from n, and is selected if n <= 0.
            // This results in an option with a higher weight having a larger % chance of being selected.
            int weightSum = 0;
            foreach (WeightedOption<T> option in options)
            {
                weightSum += option.Weight;
            }

            int n = random.Next(weightSum);

            T chosen = options[0].Obj;
            foreach (WeightedOption<T> option in options)
            {
                n -= option.Weight;

                if (n < 0) { chosen = option.Obj; break; }
            }

            return chosen;
        }

    }

    internal class WeightedOption<T>
    {
        public WeightedOption(int weight, T obj)
        {
            Weight = weight;
            Obj = obj;
        }

        public int Weight { get; }
        public T Obj { get; }
    }
}
