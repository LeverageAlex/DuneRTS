using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used to also send empty Statistics withing Messages.
    /// </summary>
    public class EmptyStatistics : Statistics
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HouseSpiceStorage { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TotalSpiceCollected { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int EnemiesDefeated { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CharactersSwallowed { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<String> CharactersAlive { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool LastCharacterStanding { get; set; }
    }
}
