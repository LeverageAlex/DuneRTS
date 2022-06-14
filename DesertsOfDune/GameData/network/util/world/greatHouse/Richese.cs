using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the Great house "Richese
    /// </summary>
    public class Richese : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Richese" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Richese() : base("RICHESE", "SILBER", GreatHouseConfiguration.HouseCharactersRichese, true)
        {
        }
    }
}

