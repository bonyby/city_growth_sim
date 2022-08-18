using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGrowthSim.Managers.Settings
{
    internal class SettingsManager
    {
        const string RELATIVE_FILE_PATH = "..\\..\\Managers\\Settings\\Settings.txt";
        Dictionary<string, string> settings;

        public SettingsManager()
        {
            ReadSettings();
        }

        private void ReadSettings()
        {
            settings = new Dictionary<string, string>();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RELATIVE_FILE_PATH);
            string[] lines = File.ReadAllLines(path);

            // Add each valid line to settings dictionary
            foreach (string line in lines)
            {
                string l = line.Replace(" ", "");
                if (l.StartsWith("//") || l.Length == 0) continue; // Comment or empty line

                if (l.Contains("="))
                {
                    string[] ls = l.Split('=');

                    if (ls.Length == 2) { settings.Add(ls[0], ls[1]); continue; } // Invalid if length != 2
                }

                Console.Error.WriteLine(string.Format("Invalid Settings Entry: '{0}'", line)); // Invalid
            }
        }

        /// <summary>
        /// Finds the string value for the given string key. Returns null if not found.
        /// </summary>
        /// <param name="key">Key for the key-value pair</param>
        /// <returns>String value. Null if not found</returns>
        public string GetSettingsValue(string key)
        {
            if (key == null) return null;

            string value;
            settings.TryGetValue(key, out value);
            
            if (value == null) Console.Error.WriteLine(string.Format("'{0}' not found in settings. Check spelling and capital letters.", key));

            return value;
        }

        /// <summary>
        /// Finds the int value for the given string key
        /// </summary>
        /// <param name="key">Key for the key-value pair</param>
        /// <returns>int value.</returns>
        public int GetSettingsValueAsInt(string key)
        {
            // TODO: Should throw an error if value is not an int
            string stringVal = GetSettingsValue(key);
            int value;
            int.TryParse(stringVal, out value);
            return value;
        }
    }
}
