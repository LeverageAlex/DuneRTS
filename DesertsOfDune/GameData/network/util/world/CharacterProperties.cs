using System;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// Stores all character properties needed for defining the character.
    /// </summary>
    public class CharacterProperties
    {
        [JsonProperty]
        public int maxHP { get; set; }
        [JsonProperty]
        public int maxMP { get; set; }
        [JsonProperty]
        public int maxAP { get; set; }
        [JsonProperty]
        public int damage { get; set; }
        [JsonProperty]
        public int inventorySize { get; set; }
        [JsonProperty]
        public int healingHP { get; set; }

        public CharacterProperties()
        {
        }
    }
}
