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

        IVisualizer vis;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            House h = new House(new Point(random.Next(100), random.Next(100)));
            Structures.Add(h);

            if (vis == null) { vis = new StandardVisualizer(this); }

            vis.DrawWorld();
        }
    }
}
