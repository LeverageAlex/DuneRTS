using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class Holds the Data of a GreatHouse
    /// </summary>
    public abstract class GreatHouse
    {
        private string name;
        private float color;
        private bool illegalAtomicUsage;
        private Character[] characters;

        /// <summary>
        /// Getter for field illegalAtomicUsage
        /// </summary>
        /// <returns>true, if house used illegalAtomicUsage</returns>
        public bool GetIllegalAtomicUsage()
        {
            return illegalAtomicUsage;
        }
    }
}
