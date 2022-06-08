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
        /// loads the scenario configuration file at a given filepath and stores the information in the <see cref="ScenarioConfiguration"/> class
        /// </summary>
        /// <param name="filepath">path, from which the file can be loaded</param>
        /// <returns>true, if the scenario config could be read and processed successfully</returns>
        /// TODO: check for exceptions and catch them
        public ScenarioConfiguration LoadScenarioConfiguration(string filepath)
        {
            // reads the file
            string scenarionConfigurationJSONString = File.ReadAllText(filepath);

            // TODO: check against JSON-Schema

            // parse the JSON-String contained in the file
            return JsonConvert.DeserializeObject<ScenarioConfiguration>(scenarionConfigurationJSONString);
        }

        /// <summary>
        /// loads the party configuration file at a given filepath and stores the information in the <see cref="PartyConfiguration"/> class
        /// </summary>
        /// <param name="filepath">path, from which the file can be loaded</param>
        /// <returns>true, if the party config could be read and processed successfully</returns>
        /// TODO: check for exceptions and catch them
        public PartyConfiguration LoadPartyConfiguration(string filepath)
        {
            // reads the file
            string partyConfigurationJSONString = File.ReadAllText(filepath);

            // TODO: check against JSON-Schema

            // parse the JSON-String contained in the file
            return JsonConvert.DeserializeObject<PartyConfiguration>(partyConfigurationJSONString);
        }
    }
}
