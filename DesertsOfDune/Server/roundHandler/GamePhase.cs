using System;
using System.Collections.Generic;
using System.Text;

namespace Server.roundHandler
{
    /// <summary>
    /// This interface demands the contract for the GamePhases
    /// </summary>
    public interface GamePhase
    {
        /// <summary>
        /// This method is used to execute a RoundPhase
        /// </summary>
        public void Execut();
    }
}
