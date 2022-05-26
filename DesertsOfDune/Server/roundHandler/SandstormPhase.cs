using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Server.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the SandstormPhase.
    /// </summary>
    public class SandstormPhase : IGamePhase
    {
        private MapField eyeOfStorm;
        private MapField[][] strom;
        private MapField field;
        private GameData.gameObjects.RoundHandler parent;

        /// <summary>
        /// This method handles the walk for the Sandstorm
        /// </summary>
        /// <returns>true, if movement was possible</returns>
        public bool RandomWalk()
        {
            // TODO implement logic
            return false;
        }

        /// <summary>
        /// This method creates a new Storm if 
        /// </summary>
        /// <returns></returns>
        public bool CalculateNewStrom()
        {
            // TODO implement logic
            return false;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
