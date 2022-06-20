using System;
using System.Collections.Generic;
using GameData.network.util.world;

namespace AIClient
{
    /// <summary>
    /// Represents the world, so the map and the characters, the ai client is playing on.
    /// </summary>
    /// <remarks>
    /// This class is used for storing the map changes coming from server and determine all possible moves or simulate moves in the world
    /// </remarks>
    public class World
    {
        /// <summary>
        /// the map, the ai client is playing on
        /// </summary>
        public Map Map { get; set; }

        public List<Character> AliveCharacters { get; private set; }

        /// <summary>
        /// the assigned (and chosen) great house for this client
        /// </summary>
        public string AssignedGreatHouse { get; set; }

        public Position CurrentPositionOfSandworm { get; set; }

        public World()
        {
            this.AliveCharacters = new List<Character>();
        }
    }
}
