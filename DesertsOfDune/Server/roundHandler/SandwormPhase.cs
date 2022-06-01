using GameData.network.util.world;
using Server.Configuration;
using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the phase, which handles the sandworm
    /// </summary>
    public class SandwormPhase : IGamePhase
    {
        private Sandworm _sandworm;

        private readonly Map _map;

        /// <summary>
        /// creates a new sandworm phase
        /// </summary>
        /// <param name="map">the map, the sandworm (phase) is working on</param>
        public SandwormPhase(Map map)
        {
            this._map = map;
        }

        public void Execute()
        {
            // check, whether there is a sandworm
            if (_sandworm != null)
            {
                Queue<MapField> path = _sandworm.CalculatePathToTarget();
                _sandworm.MoveSandWorm(path);
            } else
            {
                List<Character> characters = this._map.GetCharactersOnMap();

                // check, if there are any loud characters on the map, so the sandworm spawns
                if (CheckLoudness(characters))
                {
                    // there is no sandworm, but loud characters, so spawn one
                    _sandworm = Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, this._map, characters);
                }
            }
        }

        /// <summary>
        /// checks, if there is a loud character on the map
        /// </summary>
        /// <returns>true, if a loud character exists</returns>
        public bool CheckLoudness(List<Character> characters)
        {
            return characters.Any(character => character.IsLoud());
        }
    }
}
