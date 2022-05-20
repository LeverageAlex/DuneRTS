using System;
using System.Collections.Generic;
using Server.Clients;

namespace Server
{
    public class Party
    {
        public string _lobbyCode { get; }
        public int _cpuCount { get; } //amount of AIPlayer
        private readonly List<Player> _connectedPlayers;
        //List for spectator ?

        public Party(string lobbyCode, int cpuCount)
        {
            _lobbyCode = lobbyCode;
            _cpuCount = cpuCount;

            _connectedPlayers = new List<Player>();

            //Console.WriteLine($"Party created with lobbycode: {lobbyCode} and cpucount: {cpuCount}");
        }

        public void AddPlayer(Player player)
        {
            _connectedPlayers.Add(player);
        }
    }
}
