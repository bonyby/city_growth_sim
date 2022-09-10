using CityGrowthSim.City.Neighbourhoods;
using CityGrowthSim.City.Structures;
using CityGrowthSim.City.Structures.Walls;
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
        List<BaseWall> walls;
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
            walls = new List<BaseWall>();

            // Create a wall - just a test for now
            Point[] corners = new Point[] { new Point(50, 50), new Point(75, 50), new Point(100, 60), new Point(125, 150), new Point(110, 225), new Point(65, 260) };
            BaseWall wall = new Palisade(corners[0], corners, 12);
            walls.Add(wall);
        }

        /// <summary>
        /// Retrieves all structures in the city
        /// </summary>
        /// <returns>List of all structures in the city</returns>
        public List<IStructure> GetAllStructures()
        {
            List<IStructure> s = new List<IStructure>();

            // Add structures from neighbourhoods
            foreach (Neighbourhood neighbourhood in neighbourhoods)
            {
                s.AddRange(neighbourhood.Structures);
            }

            // Add walls
            s.AddRange(walls);

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
                    var plotData = GetPotentialPlots(mBBox, index);

                    // Check for an intersection with each structure in the neighbourhood except for the candidate itself
                    List<IStructure> candList = new List<IStructure>() { cand };
                    List<IStructure> structsToCheck = n.Structures.Except(candList).ToList();
                    bool structureAdded = false;

                    for (int j = 0; j < plotData.plots.Length; j++)
                    {
                        structureAdded = AddStructureIfPlotValid((plotData.plots[j], plotData.alignmentDirs[j]), structsToCheck);
                        if (structureAdded) break;
                    }

                    if (structureAdded) { availablePlotFound = true; break; } // break out of loop if a structure was succesfully added
                }

                // Remove candidate from future checks if no available space around it
                if (!availablePlotFound) n.RemoveNeighbourCandidate(num);

            } while (candidates.Count > 0 && !availablePlotFound);

            return;

            (Point[][] plots, PointF[] alignmentDirs) GetPotentialPlots(Point[] mBBox, int i) // Generates two potential plots for a given corner of the candidates MBBox (one to each of the available two sides)
            {
                Point p_i = mBBox[i];
                Point p_j = mBBox[(i + 1) % mBBox.Length]; // next corner
                Point p_k = mBBox[(mBBox.Length + i - 1) % mBBox.Length]; // previous corner (needs to add the length of the array in case i=0 which results in -1 otherwise)
                PointF dir_ji = PointUtility.DirectionTo(p_j, p_i);
                PointF dir_ki = PointUtility.DirectionTo(p_k, p_i);
                float offset = (float)(random.NextDouble() * 2 + 1); // these three are placeholder for now. TODO: maybe set the offset as a settings variable. Variable width/height?
                float width = 50;
                float height = 50;

                // Generate potential plots to each side of the corner
                Point[] plot1 = GeneratePotentialPlot(p_i, dir_ji, dir_ki, offset, width, height);
                Point[] plot2 = GeneratePotentialPlot(p_i, dir_ki, dir_ji, offset, width, height);

                Point[][] plots = new Point[][] { plot1, plot2 };
                PointF[] alignDirs = new PointF[] { PointUtility.DirectionTo(plot1[0], p_i), PointUtility.DirectionTo(plot2[0], p_i) };

                return (plots, alignDirs);
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

            bool AddStructureIfPlotValid((Point[] plot, PointF alignmentDir) plotData, List<IStructure> structures)
            {
                bool valid = ValidPlot(plotData.plot, structures);

                // Add the new structure (house for now) if an available plot found
                if (valid)
                {
                    IStructure house = structureFact.CreateHouse(plotData.plot, plotData.alignmentDir);
                    if (house != null)
                    {
                        n.AddStructure(house);
                        return true;
                    }
                }

                return false;
            }

            bool ValidPlot(Point[] plot, List<IStructure> structures) // Checks if the plot is valid (no intersection with other structures)
            {
                bool intersection = false;

                foreach (IStructure structure in structures)
                {
                    bool inter = PointUtility.CheckPolygonsIntersecting(plot, structure.MinimumBoundingBox);
                    if (inter) { intersection = true; break; }
                }

                return !intersection;
            }
        }

        private void CreateNewNeighbourhood()
        {
            if (neighbourhoods == null) neighbourhoods = new List<Neighbourhood>();

            neighbourhoods.Add(new Neighbourhood());
        }
    }
}
