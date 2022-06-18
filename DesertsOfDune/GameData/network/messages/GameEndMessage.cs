using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to comunicate the statistics at the end of the game
    /// </summary>
    public class GameEndMessage : Message
    {
        [JsonProperty]
        public int winnerID { get; }
        [JsonProperty]
        public int loserID { get; }
        [JsonProperty]
        public Statistics[] statistics { get; }

        /// <summary>
        /// Constructor of the class GameEndMessage
        /// </summary>
        /// <param name="winner">the winner of the game</param>
        /// <param name="loser">the loser of the game</param>
        /// <param name="statistics">the statistics of the game</param>
        public GameEndMessage(int winnerID, int loserID, Statistics[] statistics) : base("1.0", MessageType.GAME_END)
        {
            this.winnerID = winnerID;
            this.loserID = loserID;
            this.statistics = statistics;
        }

        /// <summary>
        /// This method is used to create a String out of 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            string s = "House storage: " + statistics[0].HouseSpiceStorage + "Spice collected: " + statistics[0].TotalSpiceCollected + "Enemies defeated: " + statistics[0].EnemiesDefeated + "Count swallowed Characters: " + statistics[0].CharactersSwallowed;
            StringBuilder sb = new StringBuilder(s);
            foreach(String character in statistics[0].CharactersAlive)
            {
                sb.Append(character);
            }
            sb.Append("Has last Character standing: " + statistics[0].LastCharacterStanding);
            sb.AppendLine("House storage: " + statistics[1].HouseSpiceStorage + "Spice collected: " + statistics[1].TotalSpiceCollected + "Enemies defeated: " + statistics[1].EnemiesDefeated + "Count swallowed Characters: " + statistics[1].CharactersSwallowed);
            foreach (String character in statistics[1].CharactersAlive)
            {
                sb.Append(character);
            }
            return sb.ToString();
        }
    }
}
