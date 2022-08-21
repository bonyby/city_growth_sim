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
        public Neighbourhood()
        {
            Structures = new List<IStructure>();
            NewNeighbourCandidates = new List<IStructure>();
        }

        public List<IStructure> Structures { get; }

        public List<IStructure> NewNeighbourCandidates { get; }

        public void AddStructure(IStructure structure)
        {
            Structures.Add(structure);
            NewNeighbourCandidates.Add(structure);
        }
    }
}
