using System;
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

        private static PartyConfiguration singleton;

        /// <summary>
        /// hide default constructor for implementing the singleton pattern
        /// </summary>
        /// TODO: set default values, if a new instance is created
        private PartyConfiguration()
        {
        }

        /// <summary>
        /// get the reference to the party configuration class (implementation of the singleton pattern)
        /// </summary>
        /// <returns>reference to this class</returns>
        public static PartyConfiguration GetInstance()
        {
            if (singleton == null)
            {
                singleton = new PartyConfiguration();
            }
            return singleton;
        }

        /// <summary>
        /// creates a new (singleton) instance based on an existing PartyConfiguration-Object
        /// </summary>
        /// <param name="partyConfigObject">the object to "copy"</param>
        public static void CreateInstance(PartyConfiguration partyConfigObject)
        {
            singleton = (PartyConfiguration)partyConfigObject.MemberwiseClone();
        }
    }
}
