using System;
using GameData.network.util.enums;
using WebSocketSharp;

namespace GameData.network.util.world.greatHouse
{
    /// <summary>
    /// Factory for creating new great houses
    /// </summary>
    public static class GreatHouseFactory
    {
        /// <summary>
        /// creates a new great house based on a given type
        /// </summary>
        /// <param name="type">the type of the new great house</param>
        /// <returns>the created instance</returns>
        public static GreatHouse CreateNewGreatHouse(GreatHouseType type)
        {
            switch (type)
            {
                case GreatHouseType.CORRINO:
                    return new Corrino();
                case GreatHouseType.ATREIDES:
                    return new Atreides();
                case GreatHouseType.HARKONNEN:
                    return new Harkonnen();
                case GreatHouseType.ORDOS:
                    return new Ordos();
                case GreatHouseType.RICHESE:
                    return new Richese();
                case GreatHouseType.VERNIUS:
                    return new Vernius();
                default:
                    // TODO: do not return null, but throw an exception
                    return null;
            }
        }
    }
}
