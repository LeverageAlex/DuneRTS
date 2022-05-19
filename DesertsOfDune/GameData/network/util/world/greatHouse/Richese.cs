using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Richese
    /// </summary>
    public class Richese : GreatHouse
    {
        /// <summary>
        /// Constructor of the class Richese
        /// </summary>
        /// <param name="name">the name of the house</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public Richese(string name, float color, Character[] characters) : base(name, color, characters)
        {

        }
    }
}
