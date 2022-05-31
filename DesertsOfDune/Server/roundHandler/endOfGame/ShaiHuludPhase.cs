using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Server;
using Server.Clients;
using Server.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the game phases, which handles the Shai-Hulud
    /// </summary>
    public class ShaiHuludPhase
    {
        private readonly Map _map;
        private MapField _currentField;

        private bool _lastCharacterEaten; 

        /// <summary>
        /// create a new handler for the shai hulud phase
        /// </summary>
        /// <param name="map">the map, the overlength mechanism is working on</param>
        public ShaiHuludPhase(Map map)
        {
            this._map = map;
            this._currentField = map.GetRandomDesertField();
            this._lastCharacterEaten = false;
        }

        /// <summary>
        /// choose a random character on the map 
        /// </summary>
        /// <returns>the chosen random character on the map</returns>
        private Character ChooseTargetCharacter()
        {
            Random random = new Random();
            List<Character> charactersOnMap = this._map.GetCharactersOnMap();

            return charactersOnMap[random.Next(charactersOnMap.Count)];
        }

        /// <summary>
        /// move the shai hulud to the target character and eat it
        /// </summary>
        /// <param name="targetCharacter">the targeted character, who will be eaten</param>
        private void EatTargetCharacter(Character targetCharacter)
        {
            _currentField = new FlatSand(_currentField.HasSpice, _currentField.isInSandstorm, _currentField.stormEye);
            _currentField.IsApproachable = true;
            _currentField = targetCharacter.CurrentMapfield;
            _currentField.IsApproachable = false;

            _currentField.Character.KilledBySandworm = true;
            _currentField.Character = null;

        }

        /// <summary>
        /// check, whether there is only one player left on tge map and if, whose character it is
        /// </summary>
        /// <returns>true, if there is only character left</returns>
        private bool CheckLastPlayerStanding()
        {
            List<Character> charactersOnMap = this._map.GetCharactersOnMap();

            if (charactersOnMap.Count == 1)
            {
                // only one character on the map left
                List<Player> players = Party.GetInstance().GetActivePlayers();

                if (players[0].UsedGreatHouse.Characters.Count == 0)
                {
                    // player 1 has no characters left, so player 2 has the last standing player
                    players[0].statistics.LastCharacterStanding = false;
                    players[1].statistics.LastCharacterStanding = true;
                } else
                {
                    // player 2 has no characters left, so player 1 has the last standing player
                    players[1].statistics.LastCharacterStanding = false;
                    players[0].statistics.LastCharacterStanding = true;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// executes a shai hulud phase
        /// </summary>
        /// <returns>true, if the last character was eaten and the game can be finished</returns>
        public bool Execute()
        {
            if (_lastCharacterEaten)
            {
                return true;
            } else
            {
                if (CheckLastPlayerStanding())
                {
                    Character target = ChooseTargetCharacter();
                    EatTargetCharacter(target);

                    // after the last character was eaten, there is no character left
                    _lastCharacterEaten = true;
                }

                return false;
            }

            
        }
    }
}
