using CityGrowthSim.Factories;
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

        PersistentObjectsFactory persistentFact;
        IVisualizer vis;

        public Main()
        {
            InitializeComponent();

            persistentFact = new PersistentObjectsFactory(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = persistentFact.CreateRandom();
            House h = new House(new Point(random.Next(750), random.Next(500)));
            //h.RotateCorners(0);
            Structures.Add(h);

            if (vis == null) { vis = new StandardVisualizer(this); }

            vis.DrawWorld();
        }
    }
}
