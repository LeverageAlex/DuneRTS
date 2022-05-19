using System;
using System.Collections.Generic;
using Server.Clients;

namespace Server
{
    public class Party
    {
        private string _lobbyCode;
        private int _cpuCount;

        private List<Player> _connectedPlayers;

        public Party(string lobbyCode, int cpuCount)
        {
            _lobbyCode = lobbyCode;
            _cpuCount = cpuCount;
        }

        public void AddPlayer(Player player)
        {
            _connectedPlayers.Add(player);
        }
    }
}
