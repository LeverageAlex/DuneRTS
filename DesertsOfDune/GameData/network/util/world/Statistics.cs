using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// Holds the data of the game statistics for a player
    /// </summary>
    public class Statistics
    {
        public int HouseSpiceStorage { get; set; }
        public int TotalSpiceCollected { get; set; }
        public int EnemiesDefeated { get; set; }
        public int CharactersSwallowed { get; set; }
        public List<String> CharactersAlive { get; set; }
        public bool LastCharacterStanding { get; set; }

        /// <summary>
        /// This method returns the statistics in a specific format.
        /// </summary>
        /// <returns>the statistics</returns>
        public List<String> GetFormatedStatistics()
        {
            //TODO: not return null
            throw new NotImplementedException("GetFormatedStatistics is not implemented!");
            //return null;
        }

        /// <summary>
        /// resets the statistics, so set every value to 0 and empty the list for the characters
        /// </summary>
        public void ResetStatistics()
        {
            this.HouseSpiceStorage = 0;
            this.TotalSpiceCollected = 0;
            this.EnemiesDefeated = 0;
            this.CharactersSwallowed = 0;
            this.CharactersAlive.Clear();
            this.LastCharacterStanding = false;
        }

        /// <summary>
        /// increases the house spice by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void AddToHouseSpiceStorage(int amount)
        {
            this.HouseSpiceStorage += amount;
        }


        /// <summary>
        /// increases the total collected spice by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void AddToTotalSpiceCollected(int amount)
        {
            this.TotalSpiceCollected += amount;
        }

        /// <summary>
        /// increases the total defeated enemies by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void AddToEnemiesDefeated(int amount)
        {
            this.EnemiesDefeated += amount;
        }

        /// <summary>
        /// increases the amount of swallowed characters (by a ordinary sandworm) by a certain amount
        /// </summary>
        /// <param name="amount">the amount used to increase by</param>
        public void AddToCharactersSwallowed(int amount)
        {
            this.CharactersSwallowed += amount;
        }

        /// <summary>
        /// removes a 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveFromCharactersAlive(string name)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
