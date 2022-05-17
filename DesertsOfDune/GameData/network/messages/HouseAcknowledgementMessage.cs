﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the House acknowledgement
    /// </summary>
    public class HouseAcknowledgementMessage : ClientServerMessage
    {
        private string houseName;

        /// <summary>
        /// Constructor of the class HouseAcknowledgementMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="houseName">the name of the acknowlaged house for the client</param>
        public HouseAcknowledgementMessage(int clientID, string houseName) : base(clientID,Enums.MessageType.HOUSE_ACKNOWLEGDEMENT)
        {
            this.houseName = houseName;
        }
    }
}