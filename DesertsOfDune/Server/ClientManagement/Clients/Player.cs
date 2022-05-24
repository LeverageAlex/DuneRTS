using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.enums;
using GameData.network.util.world;
using Server.ClientManagement.Clients;

namespace Server.Clients
{
    abstract public class Player : Client
    {

        /// <summary>
        /// the great house, so especially the list of characters, the player can use
        /// </summary>
        public GreatHouse UsedGreatHouse { get; set; }

        /// <summary>
        /// the array of the two great houses, which were offered to the player
        /// </summary>
        public GreatHouseType[] OfferedGreatHouses { get; set; }

        public int AmountOfStrikes { get; set; }

        public Statistics statistics { get; set; }

        protected Player(string clientName, string sessionID) : base(clientName, true, sessionID)
        {
            this.AmountOfStrikes = 0;          
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

        /// <summary>
        /// adds a new strike for this player
        /// </summary>
        public void AddStrike()
        {
            this.AmountOfStrikes++;
        }
    }
}
