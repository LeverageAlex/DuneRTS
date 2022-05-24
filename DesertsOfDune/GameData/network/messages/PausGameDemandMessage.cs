﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class represents the PausGameDemandMessage used to demand start or stop of a paus
    /// </summary>
    public class PausGameDemandMessage : Message
    {
        [JsonProperty]
        private int requestedByClientID;
        [JsonProperty]
        private bool pause;
        /// <summary>
        /// Constructor fo the Class PausGAmeDemandMessage
        /// </summary>
        /// <param name="requestedByClientID">the id of the requesting client</param>
        /// <param name="pause">weather the game should be paused or run again</param>
        public PausGameDemandMessage(int requestedByClientID, bool pause) : base("0.1",MessageType.GAME_PAUSE_DEMAND)
        {
            this.requestedByClientID = requestedByClientID;
            this.pause = pause;
        }
    }
}