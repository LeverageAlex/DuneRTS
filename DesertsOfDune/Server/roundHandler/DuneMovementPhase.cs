using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Server.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class is responsible for the DuneMovement.
    /// </summary>
    public class DuneMovementPhase : IGamePhase
    {
        private MapField[][] fields;
        private GameData.gameObjects.RoundHandler parent;

        /// <summary>
        /// This method moves the Dunes on the map.
        /// </summary>
        /// <param szenario="the szenario of the game."></param>
        /// <returns></returns>
        public bool MoveDunes(/*TODO: IMplement Class Szenario Szenario field*/)
        {
            // TODO: Implement logic.
            return false;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
