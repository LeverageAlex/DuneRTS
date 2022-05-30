using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using GameData.gameObjects;
using GameData.network.util.world.mapField;
using Server.roundHandler;
using Server.Configuration;
using Server;
using Server.Clients;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the game phase, which handles the cloning of dead characters
    /// </summary>
    public class ClonePhase : IGamePhase
    {
        private readonly double _cloneProbability;
        private readonly Map _map;

        /// <summary>
        /// creates a new game phase for the cloning
        /// </summary>
        /// <param name="map">the map, this phase is working on</param>
        /// <param name="cloneProbability">the probability, at which a dead character is respawn</param>
        public ClonePhase(Map map, double cloneProbability)
        {
            this._map = map;
            this._cloneProbability = cloneProbability;
        }

        /// <summary>
        /// triggers the cloning of a character by chance.
        /// </summary>
        /// <returns>true, if the cloning is triggered</returns>
        private void CloneCharacters(GreatHouse greatHouse)
        {
            Random random = new Random();

            List<Character> clonableCharacters = DetermineClonableCharacters(greatHouse.Characters);
            foreach (Character character in clonableCharacters)
            {
                if (_cloneProbability > random.NextDouble())
                {
                    // the character will be cloned
                    CloneCharacter(character, greatHouse.City);
                }
            }
        }
        
        /// <summary>
        /// determines the clonable characters out of all characters of a house
        /// </summary>
        /// <param name="characters">all characters of a House</param>
        /// <returns>all clonable characters of a House</returns>
        private List<Character> DetermineClonableCharacters(List<Character> characters)
        {
            List<Character> clonableCharacters = new List<Character>();

            foreach (Character character in characters)
            {
                if (character.IsDead() && !character.KilledBySandworm)
                {
                    // a character is clonable, if it dead, but not killed by a sandworm
                    clonableCharacters.Add(character);
                }
            }
            return clonableCharacters;
        }

        /// <summary>
        /// clones a specific character
        /// </summary>
        private void CloneCharacter(Character character, City city)
        {
            MapField spawnField = this._map.GetRandomApproachableNeighborField(city);
            spawnField.Character = character;
            character.ResetData();

            // send the client a message, that a character was respawned
            Party.GetInstance().messageController.DoSpawnCharacterDemand(character);
        }

        public void Execute()
        {
            // get both active player
            if (Party.GetInstance().AreTwoPlayersRegistred())
            {
                GreatHouse greatHousePlayer1 = Party.GetInstance().GetActivePlayers()[0].UsedGreatHouse;
                GreatHouse greatHousePlayer2 = Party.GetInstance().GetActivePlayers()[1].UsedGreatHouse;

                // clone characters for both great houses
                CloneCharacters(greatHousePlayer1);
                CloneCharacters(greatHousePlayer2);
            }
        }
    }
}
