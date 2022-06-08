using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used for Json serialization
    /// </summary>
    public class HouseCharacter
    {
        [JsonProperty]
        public string characterName { get; }
        [JsonProperty]
        public string characterClass { get; }

        /// <summary>
        /// Constructor of the class HouseCharacter
        /// </summary>
        /// <param name="characterName">the name of the character</param>
        /// <param name="characterClass">the House of the character</param>
        public HouseCharacter(string characterName, string characterClass)
        {
            this.characterName = characterName;
            this.characterClass = characterClass;
        }
    }
}
