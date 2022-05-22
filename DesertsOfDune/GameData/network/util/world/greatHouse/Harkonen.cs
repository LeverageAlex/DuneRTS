using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Harkonen
    /// </summary>
    public class Harkonen : GreatHouse
    {
        /// <summary>
        /// Constructor of the class Harkonen
        /// </summary>
        /// <param name="name">the name of the house</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public Harkonen(string name, string color, Character[] characters) : base(name, color, characters)
        {

        }
    }
}
