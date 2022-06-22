using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class ErrorMessage : Message
    {
        [JsonProperty]
        public int ErrorCode { get; }
        [JsonProperty]
        public string ErrorDescription { get; }

        /// <summary>
        /// If an error happens while login or relogin, the server sends this error message to the client.
        /// </summary>
        /// <param name="errorCode">Type of error</param>
        /// <param name="errorDescription">Description of the error.</param>
        public ErrorMessage(int errorCode, string errorDescription) : base("1.1", MessageType.ERROR)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
        }
    }
}
