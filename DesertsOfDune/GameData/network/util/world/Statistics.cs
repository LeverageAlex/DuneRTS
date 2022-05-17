using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is responsible for holding the data of the game statistics
    /// </summary>
    public class Statistics
    {
        private int houseSpiceStorage;
        private int totalSpiceCollected;
        private int enemiesDefeated;
        private int charactersSwallowed;
        List<String> charactersAlive;

        /// <summary>
        /// This method returns the statistics in a specific format.
        /// </summary>
        /// <returns>the statistics</returns>
        public List<String> GetFormatedStatistics()
        {
            return null;
        }

        /// <summary>
        /// This method is used to reset the statistics
        /// </summary>
        public void resetStatistics()
        {
            this.houseSpiceStorage = 0;
            this.totalSpiceCollected = 0;
            this.enemiesDefeated = 0;
            this.charactersSwallowed = 0;
            charactersAlive.Clear();
        }

        /// <summary>
        /// This method is used to increase the house spice by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void addToHouseSpiceStorage(int amount)
        {
            this.houseSpiceStorage = houseSpiceStorage + amount;
        }


        /// <summary>
        /// This method is used to increase the total collected Spice by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void addToTotalSpiceCollected(int amount)
        {
            this.totalSpiceCollected = totalSpiceCollected + amount;
        }

        /// <summary>
        /// This method is used to increase the total defeated enemies by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void AddToEnemiesDefeated(int amount)
        {
            this.enemiesDefeated = enemiesDefeated + amount;
        }

        /// <summary>
        /// This method is used to increase the swallowed characters by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void addToCharactersSwallowed(int amount)
        {
            this.charactersSwallowed = charactersSwallowed + amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveFromCharactersAlive(string name)
        {
            return false;
        }
    }
}
