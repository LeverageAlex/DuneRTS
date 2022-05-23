using System;
using System.Runtime.CompilerServices;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace Server.Configuration
{
    /// <summary>
    /// This class stores the party configuration and is used for loading the match configuration file.
    /// </summary>
    public static class PartyConfiguration
    {
        [JsonProperty]
        public static CharacterProperties noble { get; }
        [JsonProperty]
        public static CharacterProperties mentat { get; }
        [JsonProperty]
        public static CharacterProperties beneGesserit { get; }
        [JsonProperty]
        public static CharacterProperties fighter { get; }

        [JsonProperty]
        public static int numbOfRounds { get; }
        [JsonProperty]
        public static int actionTimeUserClient { get; }
        [JsonProperty]
        public static int actionTimeAiClient { get; }
        [JsonProperty]
        public static double highGroundBonusRatio { get;}
        [JsonProperty]
        public static double lowerGroundMalusRatio { get; }
        [JsonProperty]
        public static double kanlySuccessProbability { get; }
        [JsonProperty]
        public static int spiceMinimum { get; set; }
        [JsonProperty]
        public static string cellularAutomaton { get; }
        [JsonProperty]
        public static int sandWormSpeed { get; }
        [JsonProperty]
        public static int sandWormSpawnDistance { get; }
        [JsonProperty]
        public static double cloneProbability { get; }
        [JsonProperty]
        public static int minPauseTime { get; }
    }
}
