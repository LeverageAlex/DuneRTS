using GameData.network.util.world;
using GameData;
using GameData.Configuration;
using GameData.roundHandler;
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
//        private Sandworm _sandworm;

        private readonly Map _map;

        /// <summary>
        /// creates a new sandworm phase
        /// </summary>
        /// <param name="map">the map, the sandworm (phase) is working on</param>
        public SandwormPhase(Map map)
        {
            this._map = map;
        }

        /// <summary>
        /// Executes the sandworm phase
        /// </summary>
        public void Execute()
        {
            // check, whether there is a sandworm
            if (Sandworm.GetSandworm() != null)
            {
                var players = Party.GetInstance().GetActivePlayers();
                int greatHouse1CharacterAmount = players[0].UsedGreatHouse.GetCharactersAlive().Count;
                int greatHouse2CharacterAmount = players[1].UsedGreatHouse.GetCharactersAlive().Count;
                Queue<MapField> path = Sandworm.GetSandworm().CalculatePathToTarget();
                List<MapField> path2 = new List<MapField>(path);
                path2.Reverse();
                Sandworm.GetSandworm().MoveSandWorm(path2);
                // check if character was swallowed by sandworm, if yes update player statistics
                if (players[0].UsedGreatHouse.GetCharactersAlive().Count < greatHouse1CharacterAmount)
                {
                    players[0].statistics.AddToCharactersSwallowed(1);
                }
                else if (players[1].UsedGreatHouse.GetCharactersAlive().Count < greatHouse2CharacterAmount)
                {
                    players[1].statistics.AddToCharactersSwallowed(1);
                }
            }
            else
            {
                List<Character> characters = this._map.GetCharactersOnMap();
                // check, if there are any loud characters on the map, so the sandworm spawns
                if (CheckLoudness(characters))
                {
                    // there is no sandworm, but loud characters, so spawn one
                    Sandworm.Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, this._map, characters, Party.GetInstance().messageController);
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
