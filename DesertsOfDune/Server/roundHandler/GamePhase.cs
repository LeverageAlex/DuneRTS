using System;
using System.Collections.Generic;
using System.Text;

namespace Server.roundHandler
{
    /// <summary>
    /// This interface demands the contract for the GamePhases
    /// </summary>
    public interface IGamePhase
    {
        /// <summary>
        /// execute a round phase
        /// </summary>
        public void Execute();
    }
}
