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
        public List<IStructure> Structures { get; } = new List<IStructure>();

        PersistentObjectsFactory persistentFact;
        StructureFactory structFact;
        IVisualizer vis;

        public Main()
        {
            InitializeComponent();

            persistentFact = new PersistentObjectsFactory(this);
            structFact = persistentFact.CreateStructureFactory();
            vis = persistentFact.CreateVisualizer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Structures.Add(structFact.CreateHouse());
            }
            vis.DrawWorld();
        }
    }
}
