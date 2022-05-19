using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class Holds the Data of a GreatHouse
    /// </summary>
    public class GreatHouse
    {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private float color;
        [JsonProperty]
        private bool illegalAtomicUsage;
        [JsonProperty]
        private Character[] characters;
        private City city;

        /// <summary>
        /// Constructor of the Class GreatHouse
        /// </summary>
        /// <param name="name">the name of the Greathouse</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public GreatHouse(string name, float color, Character[] characters)
        {
            this.name = name;
            this.color = color;
            this.illegalAtomicUsage = false;
            this.characters = characters;
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
