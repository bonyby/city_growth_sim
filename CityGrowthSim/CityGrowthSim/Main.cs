using CityGrowthSim.Structures;
using CityGrowthSim.Visualization;
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
    public partial class Main : Form
    {
        public Graphics Graphics => this.CreateGraphics();
        public List<IStructure> Structures { get; } = new List<IStructure>();

        public Main()
        {
            InitializeComponent();
            Point[] corners = new Point[] { new Point(0, 2), new Point(121, 5), new Point(143, 67), new Point(0, 85) };
            House h = new House(new Point(10, 42), corners);
            Structures.Add(h);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IVisualizer vis = new StandardVisualizer(this);
            vis.DrawWorld();
        }
    }
}
