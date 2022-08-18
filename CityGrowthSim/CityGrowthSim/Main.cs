using CityGrowthSim.City.Structures;
using CityGrowthSim.Factories;
using CityGrowthSim.Utility.RandomWeightedSelection;
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
        PersistentObjectsFactory persistentFact;

        public Main()
        {
            InitializeComponent();

            persistentFact = new PersistentObjectsFactory(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
