using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate Sandworm movement.
    /// </summary>
    public class SandwormMoveMessage : Message
    {
        private Position[] fields;

        /// <summary>
        /// Constructor of the class SandwormMoveMessage
        /// </summary>
        /// <param name="fields">takes a array of Position to represent the sandworm movement.</param>
        public SandwormMoveMessage(Position[] fields) : base("v1", Enums.MessageType.SANDWORM_MOVE)
        {
            this.fields = fields;
        }
     }
}
