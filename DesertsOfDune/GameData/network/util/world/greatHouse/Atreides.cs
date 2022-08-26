using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the Great house "Atreides"
    /// </summary>
    public class Atreides : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Atreides" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Atreides() : base("ATREIDES", "GREEN", GreatHouseConfiguration.HouseCharactersAtreides, true)
        {
        }
    }
}
