using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CityGrowthSim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.DarkSlateBlue, 3);
            Brush b = new SolidBrush(Color.Aqua);

            Point[] points = new Point[] { new Point(5, 5), new Point(56, 25), new Point(50, 150), new Point(25, 100), new Point(15, 115), new Point(10, 75) };

            g.FillPolygon(b, points);
            g.DrawPolygon(p, points);

            //g.DrawEllipse(p, new Rectangle(50, 50, 300, 200));
            //g.FillEllipse(b, new Rectangle(50, 50, 300, 200));

            p.Dispose();
            g.Dispose();
        }
    }
}
