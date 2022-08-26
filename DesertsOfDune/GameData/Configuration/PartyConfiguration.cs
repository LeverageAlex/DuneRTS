using System;
using System.Runtime.CompilerServices;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.Configuration
{
    /// <summary>
    /// This class stores the party configuration and is used for loading the match configuration file.
    /// </summary>
    public class PartyConfiguration
    {
        [JsonProperty]
        public CharacterProperties noble { get; set; }
        [JsonProperty]
        public CharacterProperties mentat { get; set; }
        [JsonProperty]
        public CharacterProperties beneGesserit { get; set; }
        [JsonProperty]
        public CharacterProperties fighter { get; set; }

        [JsonProperty]
        public int numbOfRounds { get; set; }
        [JsonProperty]
        public int actionTimeUserClient { get; set; }
        [JsonProperty]
        public int actionTimeAiClient { get; set; }
        [JsonProperty]
        public double highGroundBonusRatio { get; set; }
        [JsonProperty]
        public double lowerGroundMalusRatio { get; set; }
        [JsonProperty]
        public double kanlySuccessProbability { get; set; }
        [JsonProperty]
        public int spiceMinimum { get; set; }
        [JsonProperty]
        public string cellularAutomaton { get; set; }
        [JsonProperty]
        public int sandWormSpeed { get; set; }
        [JsonProperty]
        public int sandWormSpawnDistance { get; set; }
        [JsonProperty]
        public double cloneProbability { get; set; }
        [JsonProperty]
        public int minPauseTime { get; set; }
        [JsonProperty]
        public double crashProbability { get; set; }

        private static PartyConfiguration singleton;

        /// <summary>
        /// hide default constructor for implementing the singleton pattern
        /// </summary>
        private PartyConfiguration()
        {

        }

        /// <summary>
        /// get a reference to this instance
        /// </summary>
        /// <returns></returns>
        public static PartyConfiguration GetInstance()
        {
            if (singleton == null)
            {
                singleton = new PartyConfiguration();
            }

            return singleton;
        }

        /// <summary>
        /// sets a reference of a PartyConfiguration-object to this singleton reference
        /// </summary>
        /// <param name="configuration">the reference to a party configuration object</param>
        public static void SetInstance(PartyConfiguration configuration)
        {
            singleton = configuration;
        }
    }
}
