using CityGrowthSim.Structures;
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

        public StandardVisualizer(Main main)
        {
            this.main = main;
        }

        public void DrawWorld()
        {
            Brush b = new SolidBrush(Color.Black);
            Graphics g = main.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            List<IStructure> structures = main.Structures;

            foreach (IStructure s in structures)
            {
                g.FillPolygon(b, s.GlobalCorners);
            }

            g.Dispose();
            b.Dispose();
        }
    }
}
