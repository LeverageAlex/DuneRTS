using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used to store the character statistics
    /// </summary>
    public class CharacterStatistics
    {
        private int healthPoints;
        private int actionPoints;
        private int movementPoints;
        private int spice;
        private bool isLoud;
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
            this.healthPoints = hp;
            this.actionPoints = ap;
            this.movementPoints = mp;
            this.spice = spice;
            this.isLoud = isLoud;
            this.isSwallowed = isSwallowed;
        }
    }
}
