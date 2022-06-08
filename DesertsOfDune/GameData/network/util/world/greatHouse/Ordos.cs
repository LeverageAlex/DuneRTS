using System;
using System.Collections.Generic;
using System.Text;
using GameData.Configuration;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents the Great house "Ordos"
    /// </summary>
    public class Ordos : GreatHouse
    {
        /// <summary>
        /// create a new Great house of type "Ordos" with the characters from the "Standisierungsdokument"
        /// </summary>
        public Ordos() : base("ORDOS", "BLAU", GreatHouseConfiguration.HouseCharactersOrdos)
        {
        }
    }
}
