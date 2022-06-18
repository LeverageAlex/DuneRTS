using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Serilog;
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
        private static Timer wormDespawnTimer;

        /// <summary>
        /// create a new handler for the shai hulud phase
        /// </summary>
        /// <param name="map">the map, the overlength mechanism is working on</param>
        public ShaiHuludPhase(Map map)
        {
            this._map = map;
            this._currentField = map.GetRandomDesertField();
            this._lastCharacterEaten = false;
            wormDespawnTimer = new Timer(1000);
            wormDespawnTimer.Elapsed += OnTimerDespawnWorm;
            wormDespawnTimer.AutoReset = false;

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
            List<MapField> path = new List<MapField>();
            path.Add(_currentField);
            path.Add(targetCharacter.CurrentMapfield);

            _currentField = new FlatSand(_currentField.hasSpice, _currentField.isInSandstorm);

            // send map change for updating the map in the user client
            Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
            _currentField.IsApproachable = true;
            _currentField = targetCharacter.CurrentMapfield;
            _currentField.IsApproachable = false;

            // move the shai hulud
            //Party.GetInstance().messageController.DoMoveSandwormDemand(path);

            // kill target character and send message, that stats of character changed
            _currentField.Character.KilledBySandworm = true;    

            // get the id the client, whose character the target character is
            Player player = Party.GetInstance().GetPlayerByCharacterID(targetCharacter.CharacterId);
            if (player != null)
            {
                Party.GetInstance().messageController.DoSendChangeCharacterStatsDemand(player.ClientID, targetCharacter.CharacterId, new CharacterStatistics(targetCharacter));
            }
            else
            {
                Log.Error($"There is no player with a character with the character ID {targetCharacter.CharacterId}!");
            }

            _currentField.DisplaceCharacter(_currentField.Character);
        }

        /// <summary>
        /// check, whether there is only one player left on tge map and if, whose character it is
        /// </summary>
        /// <returns>true, if there is only character left</returns>
        private void DetermineLastPlayerStanding()
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

            }


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
                    DetermineLastPlayerStanding();
                    Character target = ChooseTargetCharacter();

                    // spawn the shai hulud
                    Party.GetInstance().messageController.DoSpawnSandwormDemand(target.CharacterId, target.CurrentMapfield/*_currentField*/);
                    
                    EatTargetCharacter(target);

                    // despawn the shai hulud
                    wormDespawnTimer.Start();

                    // after the last character was eaten, there is no character left
                    _lastCharacterEaten = true;
                    foreach(Character character in Map.instance.GetCharactersOnMap())
                    {
                        if ( ! character.killedBySandworm)
                        {
                            _lastCharacterEaten = false;
                            break;
                        }
                    }
                

                return false;
            }

            
        }


        private static void OnTimerDespawnWorm(Object source, ElapsedEventArgs e)
        {
            Party.GetInstance().messageController.DoDespawnSandwormDemand();
        }

    }
}
