using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used to store the character statistics
    /// </summary>
    public class CharacterStatistics
    {
        [JsonProperty]
        public int HP { get; }
        [JsonProperty]
        public int AP { get; }
        [JsonProperty]
        public int MP { get; }
        [JsonProperty]
        public int spice { get; }
        [JsonProperty]
        public bool isLoud { get; }
        [JsonProperty]
        public bool isSwallowed { get; }

        /// <summary>
        /// Constructor of the class CharacterStatistics
        /// </summary>
        /// <param name="hp">the healthpoints of the character</param>
        /// <param name="ap">the actionPoints of the character</param>
        /// <param name="mp">the movementPoints of the character</param>
        /// <param name="spice">the amount of spice the character has</param>
        /// <param name="isLoud">true, if the character is loud</param>
        /// <param name="isSwallowed">true, if the character got swallowed</param>
        public CharacterStatistics(int hp, int ap, int mp, int spice, bool isLoud, bool isSwallowed)
        {
            this.HP = hp;
            this.AP = ap;
            this.MP = mp;
            this.spice = spice;
            this.isLoud = isLoud;
            this.isSwallowed = isSwallowed;
        }

        /// <summary>
        /// creates the character statistics depending on a character
        /// </summary>
        /// <param name="character">the character, whose statistics should be created</param>
        /// TODO: check, whether spice, isLoud and isSwallowed is correct
        public CharacterStatistics(Character character)
        {
            this.HP = character.healthCurrent;
            this.AP = character.APcurrent;
            this.MP = character.MPcurrent;
            this.spice = character.inventoryUsed;
            this.isLoud = character.IsLoud();
            this.isSwallowed = character.IsDead();
        }
    }
}
