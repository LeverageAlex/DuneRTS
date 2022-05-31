using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the Great house "Harkonnen"
    /// </summary>
    public class Harkonnen : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Harkonnen" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Harkonnen() : base("HARKONNEN", "ROT", GreatHouseConfiguration.HouseCharactersHarkonnen)
        {
        }
    }
}
