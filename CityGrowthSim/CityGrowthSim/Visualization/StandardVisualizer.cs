using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using CityGrowthSim.Managers;
using CityGrowthSim.Visualization.TerrainStrategies;
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
        private ITerrainVisualizationStrategy terrainStrat;

        public StandardVisualizer(Main main, TimeManager timeManager, CityPlanner cityPlanner, ITerrainVisualizationStrategy terrainStrat)
        {
            this.main = main;
            this.timeManager = timeManager;
            this.cityPlanner = cityPlanner;

            this.terrainStrat = terrainStrat;

            timeManager.UpdateReached += TimeManager_UpdateReached;
        }

        private void TimeManager_UpdateReached(object sender, EventArgs e)
        {
            DrawWorld();
        }

        public void DrawWorld()
        {
            Graphics graphics = main.Graphics;
            Size formSize = main.Size;
            Bitmap bitmap = new Bitmap(formSize.Width, formSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);

            terrainStrat.DrawTerrain(g);

            Brush b = new SolidBrush(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            List<IStructure> structures = cityPlanner.GetAllStructures();

            foreach (IStructure s in structures)
            {
                g.FillPolygon(b, s.GlobalCorners);
            }

            graphics.DrawImage(bitmap, 0, 0);

            g.Dispose();
            graphics.Dispose();
            b.Dispose();
        }
    }
}
