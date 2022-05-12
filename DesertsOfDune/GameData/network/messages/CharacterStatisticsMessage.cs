using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    public class CharacterStatisticsMessage : TurnMessage
    {
        private CharacterStatistics statistics;

        public CharacterStatisticsMessage(int clientID, int characterID, CharacterStatistics statistics) : base(characterID,clientID,Enums.MessageType.CHARACTER_STAT_CHANGE)
        {
            this.statistics = statistics;
        }
    }
}
