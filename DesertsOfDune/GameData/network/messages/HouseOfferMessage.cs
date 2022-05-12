﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the Houses that are offered by the server.
    /// </summary>
    public class HouseOfferMessage : ClientServerMessage
    {
        private GreatHouse[] offeredHouses;

        /// <summary>
        /// Constructor of the class ClientServerMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="offeredHouses">the houses that the server offers the client.</param>
        public HouseOfferMessage(int clientID, GreatHouse[] offeredHouses) : base(clientID,Enums.MessageType.HOUSE_OFFER)
        {
            this.offeredHouses = offeredHouses;
        }
    }
}
