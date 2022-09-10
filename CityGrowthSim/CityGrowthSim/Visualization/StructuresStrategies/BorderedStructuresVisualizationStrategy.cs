using CityGrowthSim.City;
using CityGrowthSim.City.Structures;
using CityGrowthSim.City.Structures.Walls;
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
            Pen borderPen = new Pen(BorderColor, BorderWidth);

            List<IStructure> structures = cityPlanner.GetAllStructures();

            foreach (IStructure s in structures)
            {
                if (s is BaseWall) DrawWall(graphics, (BaseWall)s, borderPen);  // Draw wall
                else DrawStructure(graphics, s, mainBrush, borderPen);          // Default draw structure
            }

            mainBrush.Dispose();
        }

        public void DrawStructure(Graphics graphics, IStructure structure, Brush brush, Pen pen)
        {
            graphics.FillPolygon(brush, structure.GlobalCorners);

            List<Point> points = structure.GlobalCorners.ToList();
            points.Add(structure.GlobalCorners[0]); // Append the first point to draw the last remaining line segment
            graphics.DrawLines(pen, points.ToArray());
        }

        public void DrawWall(Graphics graphics, BaseWall wall, Pen borderPen)
        {
            Color color = GetWallColor(); // Should create a Color class which loads colors from the settings..

            Pen mainPen = new Pen(color, wall.Width);

            graphics.DrawLines(mainPen, wall.GlobalCorners);

            Color GetWallColor()
            {
                return Color.SaddleBrown;
            }
        }
    }
}
