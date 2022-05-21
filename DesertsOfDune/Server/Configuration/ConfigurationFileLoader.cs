using System;
using System.IO;
using Newtonsoft.Json;

namespace Server.Configuration
{
    /// <summary>
    /// Loader of the configuration files for the party configuration and the scenario configuration.
    /// </summary>
    public class ConfigurationFileLoader
    {
        public ConfigurationFileLoader()
        {
        }

        /// <summary>
        /// loads the scenario configuration file at a given filepath and stores the information in the <see cref="Configuration.ScenarioConfiguration"/> class
        /// </summary>
        /// <param name="filepath">path, from which the file can be loaded</param>
        /// <returns>true, if the scenario config could be read and processed successfully</returns>
        public ScenarioConfiguration LoadScenarioConfiguration(string filepath)
        {
            // reads the file
            string scenarionConfigurationJSONString = File.ReadAllText(filepath);

            // TODO: check against JSON-Schema

            // parse the JSON-String contained in the file
            return JsonConvert.DeserializeObject<ScenarioConfiguration>(scenarionConfigurationJSONString);
        }
    }
}
