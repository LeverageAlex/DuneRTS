using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to comunicate the despawn of a sandworm.
    /// </summary>
    public class SandwormDespawnDemandMessage : Message
    {

        /// <summary>
        /// Constructor of the class SandwormDespawnMessage
        /// </summary>
        public SandwormDespawnDemandMessage() : base("1.1", MessageType.SANDWORM_DESPAWN_DEMAND)
        {

        }
    }
}
