using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to comunicate the despawn of a sandworm.
    /// </summary>
    public class SandwormDespawnMessage : Message
    {

        /// <summary>
        /// Constructor of the class SandwormDespawnMessage
        /// </summary>
        public SandwormDespawnMessage() : base("v1",ActionType.MessageType.SANDWORM_DESPAWN)
        {

        }
    }
}
