using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Atreides
    /// </summary>
    public class Atreides : GreatHouse
    {
        /// <summary>
        /// Constructor of the class Atreides
        /// </summary>
        /// <param name="name">the name of the house</param>
        /// <param name="color">the color of the house</param>
        /// <param name="characters">the characters of the house</param>
        public Atreides (string name, float color, Character[] characters) : base(name,color,characters)
        {

        }
    }
}
