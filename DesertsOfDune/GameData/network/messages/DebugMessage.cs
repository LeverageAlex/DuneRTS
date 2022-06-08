using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This Class is used for Debug Messages.
    /// </summary>
    public class DebugMessage : Message
    {
        [JsonProperty]
        public int code { get; }
        [JsonProperty]
        public string explanation { get; }

        /// <summary>
        /// Constructor of the Class DebugMessage
        /// </summary>
        /// <param name="code">the code of the DebugMessage</param>
        /// <param name="explanation">the explanation for the DebugMessage</param>
        public DebugMessage(int code, string explanation) : base("0.1", MessageType.DEBUG)
        {
            this.code = code;
            this.explanation = explanation;
        }
    }
}
