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


            // TODO: Look through neighbour candidates untill an available space has been found.
            // just placing besides some candidate now without collision detection to prototype.

            Random random = new Random(); // DISCLAIMER: temporary Random obj. Will be removed soon or replaced with perm. Random obj.
            if (candidates.Count == 0)
            {
                IStructure h = structureFact.CreateHouse(new Point(random.Next(750), random.Next(500)));
                h.RotateCornersAroundCentroid(random.Next(360));
                n.AddStructure(h);
                return;
            }

            // I'm so sorry for anyone having to look at this prototype spaghet.. *cough* code
            //int numb = random.Next(neighbourCandidates.Count);
            //IStructure cand = neighbourCandidates[numb];
            //Point[] candMBBox = cand.MinimumBoundingBox;
            //int i = random.Next(candMBBox.Length);
            //Point p_i = candMBBox[i];
            ////Console.WriteLine("p_i: " + p_i);
            //int j = (candMBBox.Length + i + (random.Next(2) * 2 - 1)) % candMBBox.Length;
            //Point p_j = candMBBox[j];
            //PointF vec_ij = new PointF((p_j.X - p_i.X), (p_j.Y - p_i.Y));
            //double vec_ijLen = Math.Sqrt(Math.Pow(vec_ij.X, 2) + Math.Pow(vec_ij.Y, 2));
            //PointF vec_ij_norm = new PointF((float)(vec_ij.X / vec_ijLen), (float)(vec_ij.Y / vec_ijLen));
            //Point pos = new Point(p_i.X - (int)Math.Round(vec_ij.X), p_i.Y - (int)Math.Round(vec_ij.Y));
            //IStructure house = structureFact.CreateHouse(pos);
            //house.RotateCornersAroundCentroid(cand.Rotation + random.Next(11) - 5);
            //n.AddStructure(house);

            // Continue looking for an available plot besides an existing structure
            // -- (still prototyping for now, but a bit cleaner than the code above, atleast)
            bool availablePlotFound = false;
            do
            {
                // Get a new neighbour candidate
                int num = random.Next(candidates.Count);
                IStructure cand = candidates[num];
                candidates.RemoveAt(num);

                // Check for available space next to each of the corners of the MBBox of the candidate
                Point[] mBBox = cand.MinimumBoundingBox;
                for (int i = 0; i < mBBox.Length; i++)
                {
                    Point p_i = mBBox[i];
                    Point p_j = mBBox[(i + 1) % mBBox.Length]; // next corner
                    Point p_k = mBBox[(mBBox.Length + i - 1) % mBBox.Length]; // previous corner (needs to add the length of the array in case i=0 which results in -1 otherwise)
                    PointF dir_ji = PointUtility.DirectionTo(p_j, p_i);
                    PointF dir_ki = PointUtility.DirectionTo(p_k, p_i);
                    PointF offsetDir_ji = PointUtility.Multiply(dir_ji, 50); // Magic constants for now. Will change later
                    PointF offsetDir_ki = PointUtility.Multiply(dir_ki, 50);
                    float offset = 1;
                    Console.WriteLine("#######");
                    Console.WriteLine("p_i: " + p_i);
                    Console.WriteLine("p_0.X: " + (p_i.X + dir_ji.X * offset));
                    Console.WriteLine("#######");
                    PointF p_0 = new PointF(p_i.X + dir_ji.X * offset, p_i.Y); // Anchor/first point of the potential plot
                    Point[] potentialPlot = PointUtility.ConvertPointFsToPoints(new PointF[] { p_0,
                                                                                               new PointF(p_0.X + offsetDir_ji.X, p_0.Y),
                                                                                               new PointF(p_0.X + offsetDir_ji.X, p_0.Y - offsetDir_ki.Y),
                                                                                               new PointF(p_0.X, p_0.Y - offsetDir_ki.Y) });

                    // Check for an intersection with each structure in the neighbourhood except for the candidate itself
                    bool intersection = false;
                    List<IStructure> candList = new List<IStructure>();
                    candList.Add(cand);
                    foreach (IStructure structure in n.Structures.Except(candList))
                    {
                        bool inter = PointUtility.CheckPolygonsIntersecting(potentialPlot, structure.MinimumBoundingBox);
                        if (inter) { intersection = true; break; }
                    }

                    // Add the new house if an available plot found
                    if (!intersection)
                    {
                        Point intp_0 = PointUtility.ConvertPointFToPoint(p_0);
                        Point anchorPos = new Point(intp_0.X, intp_0.Y);
                        //IStructure house = structureFact.CreateHouse(anchorPos);
                        Console.Write("Potential plot:");
                        foreach (Point point in potentialPlot)
                        {
                            Console.Write(" " + point);
                        }
                        Console.WriteLine();
                        IStructure house = structureFact.CreateHouse(potentialPlot);
                        n.AddStructure(house);
                        availablePlotFound = true;
                        break;
                    }
                }

                // Remove candidate from future checks if no available space around it
                if (!availablePlotFound) n.RemoveNeighbourCandidate(num);

            } while (candidates.Count > 0 && !availablePlotFound);

            return;
        }

        private void CreateNewNeighbourhood()
        {
            if (neighbourhoods == null) neighbourhoods = new List<Neighbourhood>();

            neighbourhoods.Add(new Neighbourhood());
        }
    }
}
