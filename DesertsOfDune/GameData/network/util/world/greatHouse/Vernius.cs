using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Rrepresents the Great house "Vernius"
    /// </summary>
    public class Vernius : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Vernius" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Vernius() : base("VERNIUS", "VIOLETT", GreatHouseConfiguration.HouseCharactersVernius)
        {
        }
    }
}
