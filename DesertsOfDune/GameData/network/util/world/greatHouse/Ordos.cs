using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Ordos
    /// </summary>
    public class Ordos : GreatHouse
    {
        /// <summary>
        /// Constructor of the class Ordos
        /// </summary>
        /// <param name="name">the name of the house</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public Ordos(string name, string color, Character[] characters) : base(name, color, characters)
        {

        }
    }
}
