using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the Great house "Corrino"
    /// </summary>
    public class Corrino : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Corrino" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Corrino() : base("CORRINO", "GOLD", GreatHouseConfiguration.HouseCharactersCorrino)
        {
        }

        /// <summary>
        /// This constructor is implemented for testing perpeces only.
        /// </summary>
        /// <param name="houseCharacters">the characters of a house specificly specified</param>
        public Corrino(HouseCharacter[] houseCharacters) : base("CORRINO", "GOLD", houseCharacters)
        {

        }
    }
}
