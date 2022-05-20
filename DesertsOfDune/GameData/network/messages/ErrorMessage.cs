using System;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    public class ErrorMessage : Message
    {
        [JsonProperty]
        private int ErrorCode;
        [JsonProperty]
        private string ErrorDescription;

        public ErrorMessage(int errorCode, string errorDescription) : base("v1", MessageType.ERROR)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
        }
    }
}
