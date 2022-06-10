using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Server.Configuration;
using Server.roundHandler;

namespace Server.roundHandler.duneMovementHandler
{
    /// <summary>
    /// Represents the game phase, which execute the movement of the dunes
    /// </summary>
    public class DuneMovementPhase : IGamePhase
    {
        private readonly Map map;
        private readonly CellularAutomaton automaton;

        /// <summary>
        /// creates a new handler for the dune movement phase
        /// </summary>
        /// <param name="map">the map, the phase is working on</param>
        public DuneMovementPhase(Map map)
        {
            this.map = map;
            this.automaton = new CellularAutomaton(map);
        }

        /// <summary>
        /// moves the dunes following the principle of a cellular automaton
        /// </summary>
        private void MoveDunes()
        {
            this.automaton.NextIteration();
        }

        /// <summary>
        /// execute the dune movement phase
        /// </summary>
        public void Execute()
        {
            MoveDunes();
        }
    }
}
