using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Server.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the game phase, which handles the sandstorm
    /// </summary>
    public class SandstormPhase : IGamePhase
    {
        /// <summary>
        /// the central field of the storm
        /// </summary>
        private MapField eyeOfStorm;

        private Map map;

        /// <summary>
        /// create a new sandworm phase object and set the eye of the storm to a random start field
        /// </summary>
        /// <param name="map"></param>
        public SandstormPhase(Map map)
        {
            this.map = map;
            this.eyeOfStorm = GetRandomStartField();
        }



        /// <summary>
        /// moves the eye of the storm to a random neighbor field
        /// </summary>
        private void MoveStormToRandomNeighborField()
        {
            List<MapField> neighbors = this.map.GetNeighborFields(eyeOfStorm);

            // chose a random neighbor
            Random random = new Random();
            int neighborIndex = random.Next(neighbors.Count);

            eyeOfStorm = neighbors[neighborIndex];
        }

        private MapField GetRandomStartField()
        {
            Random random = new Random();
            int randomX = random.Next(this.map.MAP_WIDTH);
            int randomY = random.Next(this.map.MAP_HEIGHT);

            return this.map.GetMapFieldAtPosition(randomX, randomY);
        }

        

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
