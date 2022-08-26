using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Class is used for messages from the Server to demand a characterStatistic change.
    /// </summary>
    public class ChangeCharacterStatisticsDemandMessage : TurnMessage
    {
        [JsonProperty]
        public CharacterStatistics stats { get; }

        /// <summary>
        /// Constructor of the Class CharacterStatisticsMessageDemand
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="stats">the statistics of the client</param>
        public ChangeCharacterStatisticsDemandMessage(int clientID, int characterID, CharacterStatistics stats) : base(characterID,clientID,MessageType.CHARACTER_STAT_CHANGE_DEMAND)
        {
            this.stats = stats;
        }
    }
}
