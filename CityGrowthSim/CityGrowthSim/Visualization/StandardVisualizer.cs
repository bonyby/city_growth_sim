using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using CityGrowthSim.Managers;
using CityGrowthSim.Visualization.StructuresStrategies;
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

        private ITerrainVisualizationStrategy terrainStrat;
        private IStructuresVisualizationStrategy structuresStrat;

        public StandardVisualizer(Main main, TimeManager timeManager, ITerrainVisualizationStrategy terrainStrat, IStructuresVisualizationStrategy structuresStrat)
        {
            this.main = main;
            this.timeManager = timeManager;

            this.terrainStrat = terrainStrat;
            this.structuresStrat = structuresStrat;

            timeManager.UpdateReached += TimeManager_UpdateReached;
        }

        private void TimeManager_UpdateReached(object sender, EventArgs e)
        {
            DrawWorld();
        }

        public void DrawWorld()
        {
            // Setup
            Graphics graphics = main.Graphics;

            Size formSize = main.Size;
            Bitmap bitmap = new Bitmap(formSize.Width, formSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            
            // Reset and draw
            g.Clear(Color.White);
            terrainStrat.DrawTerrain(g);
            structuresStrat.DrawStructures(g);
            graphics.DrawImage(bitmap, 0, 0);

            // Disposal
            g.Dispose();
            graphics.Dispose();
        }
    }
}
