using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Vernius
    /// </summary>
    public class Vernius : GreatHouse
    {
        /// <summary>
        /// Constructor of the class Vernius
        /// </summary>
        /// <param name="name">the name of the house</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public Vernius(string name, string color, Character[] characters) : base(name, color, characters)
        {

        }
    }
}
