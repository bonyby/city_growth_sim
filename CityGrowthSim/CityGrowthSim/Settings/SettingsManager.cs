using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Settings
{
    internal class SettingsManager
    {
        const string RELATIVE_FILE_PATH = "..\\..\\Settings\\Settings.txt";
        Dictionary<string, string> settings;

        public SettingsManager()
        {
            ReadSettings();
            foreach (KeyValuePair<string, string> entry in settings)
            {
                Console.WriteLine(string.Format("Entry: <{0}, {1}>", entry.Key, entry.Value));
            }
        }

        private void ReadSettings()
        {
            settings = new Dictionary<string, string>();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RELATIVE_FILE_PATH);
            string[] lines = File.ReadAllLines(path);

            // Add each valid line to settings dictionary
            foreach (string line in lines)
            {
                if (line.StartsWith("//") || line.Length == 0) continue; // Comment or empty line

                if (line.Contains("="))
                {
                    string[] ls = line.Split('=');

                    if (ls.Length == 2) { settings.Add(ls[0], ls[1]); continue; } // Invalid if length != 2
                }

                Console.Error.WriteLine(string.Format("Invalid Settings Entry: '{0}'", line)); // Invalid
            }
        }
    }
}
