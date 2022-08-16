﻿using CityGrowthSim.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Factories
{
    internal class StructureFactory
    {
        private Random random;
        private ShapeFactory shapeFact;

        public StructureFactory(Random random, ShapeFactory shapeFact)
        {
            this.random = random;
            this.shapeFact = shapeFact;
        }

        public IStructure CreateHouse()
        {
            House h = new House(new Point(random.Next(750), random.Next(500)), shapeFact.CreateShape("random"));
            h.RotateCorners(45);
            return h;
        }
    }
}
