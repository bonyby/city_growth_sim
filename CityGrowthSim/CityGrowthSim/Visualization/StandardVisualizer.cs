using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using CityGrowthSim.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Visualization
{
    internal class StandardVisualizer : IVisualizer
    {
        Main main;
        private TimeManager timeManager;
        private CityPlanner cityPlanner;

        public StandardVisualizer(Main main, TimeManager timeManager, CityPlanner cityPlanner)
        {
            this.main = main;
            this.timeManager = timeManager;
            this.cityPlanner = cityPlanner;

            timeManager.UpdateReached += TimeManager_UpdateReached;
        }

        private void TimeManager_UpdateReached(object sender, EventArgs e)
        {
            DrawWorld();
        }

        public void DrawWorld()
        {
            Brush b = new SolidBrush(Color.Black);
            Graphics g = main.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            List<IStructure> structures = cityPlanner.GetAllStructures();

            foreach (IStructure s in structures)
            {
                g.FillPolygon(b, s.GlobalCorners);
            }

            g.Dispose();
            b.Dispose();
        }
    }
}
