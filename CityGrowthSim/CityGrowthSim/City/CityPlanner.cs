using CityGrowthSim.City.Neighbourhoods;
using CityGrowthSim.City.Structures;
using CityGrowthSim.Factories;
using CityGrowthSim.Managers;
using CityGrowthSim.Utility;
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
        private Random random;

        public CityPlanner(TimeManager timeManager, StructureFactory structureFact, Random random)
        {
            this.timeManager = timeManager;
            timeManager.UpdateReached += TimeManager_UpdateReached;
            this.structureFact = structureFact;
            this.random = random;

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
            AddNewStructure();
        }

        private void AddNewStructure()
        {
            Neighbourhood n = neighbourhoods[0];
            List<IStructure> candidates = new List<IStructure>(n.NewNeighbourCandidates.ToList());

            if (candidates.Count == 0)
            {
                IStructure h = structureFact.CreateHouse(new Point(random.Next(750), random.Next(500)));
                h.RotateCornersAroundCentroid(random.Next(360));
                n.AddStructure(h);
                return;
            }

            // Look for an available plot besides an existing structure
            bool availablePlotFound = false;
            do
            {
                // Get a new neighbour candidate
                int num = random.Next(candidates.Count);
                IStructure cand = candidates[num];
                candidates.RemoveAt(num);

                // Check for available space next to each of the corners of the MBBox of the candidate
                Point[] mBBox = cand.MinimumBoundingBox;
                int iOffset = random.Next(mBBox.Length); // Random number to offset i. Used to ensure randomness when looking through all corners of MBBox
                for (int i = 0; i < mBBox.Length; i++)
                {
                    int index = (i + iOffset) % mBBox.Length;
                    Point p_i = mBBox[index];
                    Point p_j = mBBox[(index + 1) % mBBox.Length]; // next corner
                    Point p_k = mBBox[(mBBox.Length + index - 1) % mBBox.Length]; // previous corner (needs to add the length of the array in case i=0 which results in -1 otherwise)
                    PointF dir_ji = PointUtility.DirectionTo(p_j, p_i);
                    PointF dir_ki = PointUtility.DirectionTo(p_k, p_i);
                    float offset = 1f; // these three are placeholder for now
                    float width = 50; 
                    float height = 50;

                    // Generate potential plots to each side of the corner
                    Point[] plot1 = GeneratePotentialPlot(p_i, dir_ji, dir_ki, offset, width, height);
                    Point[] plot2 = GeneratePotentialPlot(p_i, dir_ki, dir_ji, offset, width, height);

                    // Check for an intersection with each structure in the neighbourhood except for the candidate itself
                    bool intersection = false;
                    List<IStructure> candList = new List<IStructure>();
                    candList.Add(cand);
                    foreach (IStructure structure in n.Structures.Except(candList))
                    {
                        bool inter = PointUtility.CheckPolygonsIntersecting(plot1, structure.MinimumBoundingBox);
                        if (inter) { intersection = true; break; }
                    }

                    // Add the new house if an available plot found
                    if (!intersection)
                    {
                        IStructure house = structureFact.CreateHouse(plot1, PointUtility.DirectionTo(plot1[0], p_i));
                        if (house != null)
                        {
                            n.AddStructure(house);
                            availablePlotFound = true;
                            break;
                        }
                    }

                    // !!!! JUST COPIED FOR NOW - SIMPLY TESTING !!!!!
                    intersection = false;
                    foreach (IStructure structure in n.Structures.Except(candList))
                    {
                        bool inter = PointUtility.CheckPolygonsIntersecting(plot2, structure.MinimumBoundingBox);
                        if (inter) { intersection = true; break; }
                    }

                    // Add the new house if an available plot found
                    if (!intersection)
                    {
                        Console.WriteLine("p_i: " + p_i + " plot[0]: " + plot2[0]);
                        IStructure house = structureFact.CreateHouse(plot2, PointUtility.DirectionTo(plot2[0], p_i));
                        if (house != null)
                        {
                            n.AddStructure(house);
                            availablePlotFound = true;
                            break;
                        }
                    }
                }

                // Remove candidate from future checks if no available space around it
                if (!availablePlotFound) n.RemoveNeighbourCandidate(num);

            } while (candidates.Count > 0 && !availablePlotFound);

            return;

            (Point[] plot1, PointF alignDir1, Point[] plot2, PointF alignDir2) GetPotentialPlots(Point[] mBBox, int i) // Generates two potential plots for a given corner of the candidates MBBox
            {
                throw new NotImplementedException();
            }

            Point[] GeneratePotentialPlot(PointF pos, PointF primaryDir, PointF secondaryDir, float offset, float width, float height)
            {
                PointF primOffset = PointUtility.Multiply(primaryDir, offset);
                PointF primWidth = PointUtility.Multiply(primaryDir, width);
                PointF secondHeight = PointUtility.Multiply(secondaryDir, height);

                PointF p_0 = PointUtility.Add(pos, primOffset); // Anchor/first point of the potential plot
                PointF p_1 = PointUtility.Add(p_0, primWidth);
                PointF p_2 = PointUtility.Subtract(p_1, secondHeight);
                PointF p_3 = PointUtility.Subtract(p_2, primWidth);

                return PointUtility.ConvertPointFsToPoints(new PointF[] { p_0, p_1, p_2, p_3 });
            }
        }

        private void CreateNewNeighbourhood()
        {
            if (neighbourhoods == null) neighbourhoods = new List<Neighbourhood>();

            neighbourhoods.Add(new Neighbourhood());
        }
    }
}
