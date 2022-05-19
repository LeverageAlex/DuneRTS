using System;
using Server.Clients;

namespace Server
{
    public class HumanPlayer : Player
    {
        private string _clientName;

        public HumanPlayer(string clientName)
        {
            _clientName = clientName;
        }
    }
}
