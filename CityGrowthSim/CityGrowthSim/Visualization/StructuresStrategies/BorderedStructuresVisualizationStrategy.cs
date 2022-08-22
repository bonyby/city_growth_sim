using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Visualization.StructuresStrategies
{
    internal class BorderedStructuresVisualizationStrategy : IStructuresVisualizationStrategy
    {
        private CityPlanner cityPlanner;
        public Color MainColor { get; }
        public Color BorderColor { get; }
        public float BorderWidth { get; }

        public BorderedStructuresVisualizationStrategy(Color mainColor, Color borderColor, float borderWidth, CityPlanner cityPlanner)
        {
            MainColor = mainColor;
            BorderColor = borderColor;
            BorderWidth = borderWidth;
            this.cityPlanner = cityPlanner;
        }

        public void DrawStructures(Graphics graphics)
        {
            Brush mainBrush = new SolidBrush(MainColor);
            //Brush mBBoxBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 255));
            Pen borderPen = new Pen(BorderColor, BorderWidth);

            List<IStructure> structures = cityPlanner.GetAllStructures();

            foreach (IStructure s in structures)
            {
                graphics.FillPolygon(mainBrush, s.GlobalCorners);
                //graphics.FillPolygon(mBBoxBrush, s.MinimumBoundingBox);

                List<Point> points = s.GlobalCorners.ToList();
                points.Add(s.GlobalCorners[0]); // Append the first point to draw the last remaining line segment
                graphics.DrawLines(borderPen, points.ToArray());
            }

            mainBrush.Dispose();
        }
    }
}
