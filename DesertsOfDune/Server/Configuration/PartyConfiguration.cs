using System;
using System.Runtime.CompilerServices;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace Server.Configuration
{
    /// <summary>
    /// This class stores the party configuration and is used for loading the match configuration file.
    /// </summary>
    public class PartyConfiguration
    {
        [JsonProperty]
        public CharacterProperties noble { get; }
        [JsonProperty]
        public CharacterProperties mentat { get; }
        [JsonProperty]
        public CharacterProperties beneGesserit { get; }
        [JsonProperty]
        public CharacterProperties fighter { get; }

        [JsonProperty]
        public int numbOfRounds { get; }
        [JsonProperty]
        public int actionTimeUserClient { get; }
        [JsonProperty]
        public int actionTimeAiClient { get; }
        [JsonProperty]
        public double highGroundBonusRatio { get; }
        [JsonProperty]
        public double lowerGroundMalusRatio { get; }
        [JsonProperty]
        public double kanlySuccessProbability { get; }
        [JsonProperty]
        public int spiceMinimum { get; set; }
        [JsonProperty]
        public string cellularAutomaton { get; }
        [JsonProperty]
        public int sandWormSpeed { get; }
        [JsonProperty]
        public int sandWormSpawnDistance { get; }
        [JsonProperty]
        public double cloneProbability { get; }
        [JsonProperty]
        public int minPauseTime { get; }

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
