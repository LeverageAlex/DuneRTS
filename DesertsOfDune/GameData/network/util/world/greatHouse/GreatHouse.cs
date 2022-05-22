using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GameData.network.util.world.mapField;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class Holds the Data of a GreatHouse
    /// </summary>
    public class GreatHouse
    {
        [JsonProperty]
        private string houseName;
        [JsonProperty]
        private string houseColor;
        [JsonProperty]
        private bool illegalAtomicUsage;
        [JsonProperty]
        private Character[] houseCharacters;
        private City city;
        [JsonIgnore]
        public City City
        {
            get { return city; }
        }


        /// <summary>
        /// Constructor of the Class GreatHouse
        /// </summary>
        /// <param name="houseName">the name of the Greathouse</param>
        /// <param name="houseColor">the color of the house</param>
        /// <param name="houseCharacters">the characters of the house</param>
        public GreatHouse(string houseName, string houseColor, Character[] houseCharacters)
        {
            this.houseName = houseName;
            this.houseColor = houseColor;
            this.illegalAtomicUsage = false;
            this.houseCharacters = houseCharacters;
        }

        /// <summary>
        /// Getter for field illegalAtomicUsage
        /// </summary>
        /// <returns>true, if house used illegalAtomicUsage</returns>
        public bool GetIllegalAtomicUsage()
        {
            return illegalAtomicUsage;
        }
    }
}
