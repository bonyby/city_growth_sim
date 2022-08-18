using CityGrowthSim.City.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City.Neighbourhoods
{
    // Simply a container for structures for now.
    // Will need updating later for more functionality.
    // Needed for proper code structure.
    internal class Neighbourhood
    {
        List<IStructure> structures;

        public Neighbourhood()
        {
            Structures = new List<IStructure>();
        }

        public List<IStructure> Structures { get => structures; set => structures = value; }

        public void AddStructure(IStructure structure)
        {
            structures.Add(structure);
        }
    }
}
