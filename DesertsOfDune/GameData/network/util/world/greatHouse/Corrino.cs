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
    }
}
