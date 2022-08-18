using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Managers
{
    internal class TimeManager
    {
        public event EventHandler UpdateReached;
        private System.Windows.Forms.Timer updateTimer;

        public TimeManager(int hz)
        {
            this.Hz = hz;

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Tick += new EventHandler(OnUpdateReached);
            updateTimer.Interval = 1000 / Hz;
            updateTimer.Start();
        }

        protected virtual void OnUpdateReached(Object myObject, EventArgs e)
        {
            EventHandler handler = UpdateReached;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Update frequency for the simulation
        /// </summary>
        public int Hz { get; }
    }
}
