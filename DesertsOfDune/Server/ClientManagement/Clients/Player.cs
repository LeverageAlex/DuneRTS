using System;
using System.Collections.Generic;
using System.Text;
using Server.ClientManagement.Clients;

namespace Server.Clients
{
    abstract public class Player : Client
    {
        protected Player(string clientName, string sessionID) : base(clientName, true, sessionID)
        {
                        
        }

        /// <summary>
        /// Generate a random string for identification of the client
        /// </summary>
        /// <returns>Random generated string for clientSecret</returns>
        [Obsolete("Not used anymore, because the Client class has the oppurtunity to create a UUID, when a new client is created")]
        public string GenerateClientSecret()
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
    }
}
