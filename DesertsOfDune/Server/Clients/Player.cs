using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Clients
{
    public class Player
    {
        private static List<string> _clientSecrets;
        private static List<int> _clientIDs;

        public int ClientID { get; }
        public string ClientName { get; }
        public string ClientSecret { get; }
        public string LobbyCode { get; }

        public Player()
        {
            
        }

        public Player(string clientName, string lobbyCode)
        {
            ClientName = clientName;
            ClientID = GenerateClientID();
            ClientSecret = GenerateClientSecret();
            LobbyCode = lobbyCode;
        }

        /// <summary>
        /// Generate a random string for identification of the client
        /// </summary>
        /// <returns>Random generated string for clientSecret</returns>
        private string GenerateClientSecret()
        {
            //todo: check if clientSecret already available
            int length = 12;
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                stringBuilder.Append(letter);
            }
            return stringBuilder.ToString();
        }

        private int GenerateClientID()
        {
            //TODO: generate clientID
            //TODO: check if clientID already available
            return 1234;
        }
    }
}
