using CityGrowthSim.City.Structures;
using CityGrowthSim.City.Structures.Shapes;
using CityGrowthSim.Factories;
using CityGrowthSim.Managers.Settings;
using CityGrowthSim.Utility;
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
        SettingsManager settingsMan;

        public Main()
        {
            InitializeComponent();

            persistentFact = new PersistentObjectsFactory(this);
            settingsMan = persistentFact.CreateSettingsManager();

            int width = settingsMan.GetSettingsValueAsInt("DefaultWindowWidth");
            int height = settingsMan.GetSettingsValueAsInt("DefaultWindowHeight");

            Size = new Size(width, height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
