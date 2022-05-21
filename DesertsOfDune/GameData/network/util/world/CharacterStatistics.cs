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
        private int HP;
        [JsonProperty]
        private int AP;
        [JsonProperty]
        private int MP;
        [JsonProperty]
        private int spice;
        [JsonProperty]
        private bool isLoud;
        [JsonProperty]
        private bool isSwallowed;

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
    }
}
