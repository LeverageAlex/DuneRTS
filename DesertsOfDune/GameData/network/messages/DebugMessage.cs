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
        private int code;
        [JsonProperty]
        private string explanation;

        /// <summary>
        /// Constructor of the Class DebugMessage
        /// </summary>
        /// <param name="code">the code of the DebugMessage</param>
        /// <param name="explanation">the explanation for the DebugMessage</param>
        public DebugMessage(int code, string explanation) : base("v1", Enums.MessageType.DEBUG)
        {
            this.code = code;
            this.explanation = explanation;
        }

        /// <summary>
        /// Constructof of the class DebugMessage that only take the explanation
        /// </summary>
        /// <param name="explanation">The explanation for the DebugMessage</param>
        public DebugMessage(string explanation) : base("v1", Enums.MessageType.DEBUG)
        {
            this.explanation=explanation;
        }
    }
}
