using CityGrowthSim.City.Neighbourhoods;
using CityGrowthSim.City.Structures;
using CityGrowthSim.Factories;
using CityGrowthSim.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.City
{
    internal class CityPlanner
    {
        private TimeManager timeManager;
        List<Neighbourhood> neighbourhoods;
        private StructureFactory structureFact;

        public CityPlanner(TimeManager timeManager, StructureFactory structureFact)
        {
            this.timeManager = timeManager;
            timeManager.UpdateReached += TimeManager_UpdateReached;
            this.structureFact = structureFact;

            // Create initial neighbourhood
            CreateNewNeighbourhood();
        }

        /// <summary>
        /// Retrieves all structures in the city
        /// </summary>
        /// <returns>List of all structures in the city</returns>
        public List<IStructure> GetAllStructures()
        {
            List<IStructure> s = new List<IStructure>();

            foreach (Neighbourhood neighbourhood in neighbourhoods)
            {
                s.AddRange(neighbourhood.Structures);
            }

            return s;
        }

        private void TimeManager_UpdateReached(object sender, EventArgs e)
        {
            // Simple logic for now. Simply add a structure at each update to the single neighbourhood available.
            //AddNewStructure();
        }

        private void AddNewStructure()
        {
            Neighbourhood n = neighbourhoods[0];
            List<IStructure> neighbourCandidates = n.NewNeighbourCandidates;

            // TODO: Look through neighbour candidates untill an available space has been found.
            // just placing besides some candidate now without collision detection to prototype.

            Random random = new Random(); // DISCLAIMER: temporary Random obj. Will be removed soon or replaced with perm. Random obj.
            if (neighbourCandidates.Count == 0)
            {
                IStructure h = structureFact.CreateHouse(new Point(random.Next(750), random.Next(500)));
                h.RotateCornersAroundCentroid(random.Next(360));
                n.AddStructure(h);
                return;
            }

            // I'm so sorry for anyone having to look at this prototype spaghet.. *cough* code
            int numb = random.Next(neighbourCandidates.Count);
            IStructure cand = neighbourCandidates[numb];
            Point[] candMBBox = cand.MinimumBoundingBox;
            int i = random.Next(candMBBox.Length);
            Point p_i = candMBBox[i];
            //Console.WriteLine("p_i: " + p_i);
            int j = (candMBBox.Length + i + (random.Next(2) * 2 - 1)) % candMBBox.Length;
            Point p_j = candMBBox[j];
            PointF vec_ij = new PointF((p_j.X - p_i.X), (p_j.Y - p_i.Y));
            double vec_ijLen = Math.Sqrt(Math.Pow(vec_ij.X, 2) + Math.Pow(vec_ij.Y, 2));
            Console.WriteLine("p_j len: " + vec_ijLen);
            PointF vec_ij_norm = new PointF((float)(vec_ij.X / vec_ijLen), (float)(vec_ij.Y / vec_ijLen));
            Console.WriteLine("p_j: " + p_j + " p_j.norm.x: " + (vec_ij.X / vec_ijLen) + " p_j.nrom.y: " + (vec_ij.Y / vec_ijLen));
            Point pos = new Point(p_i.X - (int)Math.Round(vec_ij.X), p_i.Y - (int)Math.Round(vec_ij.Y));
            IStructure house = structureFact.CreateHouse(pos);
            house.RotateCornersAroundCentroid(cand.Rotation + random.Next(11) - 5);
            n.AddStructure(house);
            return;
        }

        private void CreateNewNeighbourhood()
        {
            if (neighbourhoods == null) neighbourhoods = new List<Neighbourhood>();

            neighbourhoods.Add(new Neighbourhood());
        }
    }
}
