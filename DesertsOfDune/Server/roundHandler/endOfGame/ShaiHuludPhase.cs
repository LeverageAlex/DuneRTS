using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Serilog;
using GameData;
using GameData.Clients;
using GameData.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the game phases, which handles the Shai-Hulud
    /// </summary>
    /// <remarks>
    /// So in the execution of one shai hulud round, the following things will be done:
    /// <list type="bullet">
    /// <item>Check, whether the shai hulud already eaten the last character and the game can be finished</item>
    /// <item>If the game need to be continued, choose a new target character</item>
    /// <item>and eat the target character, so move the shai hulud to the character and kill the character</item>
    /// <item>Check, whether this character was the last character standing on the map</item>
    /// </list>
    /// </remarks>
    public class ShaiHuludPhase
    {
        private readonly Map _map;

        /// <summary>
        /// the current map field of the shai hulud
        /// </summary>
        public MapField CurrentField { get; private set; }

        /// <summary>
        /// states, whether the last character was eaten (if so, the round handler will be informed and finish the game)
        /// </summary>
        private bool _lastCharacterEaten;

        /// <summary>
        /// the time, which need to pass, until the server informs the clients about the despawn of the shai hulud (in ms)
        /// </summary>
        private readonly int _timeUntilShaiHuludDespawn = 500;
        private readonly Timer _wormDespawnTimer;

        /// <summary>
        /// create a new handler for the shai hulud phase
        /// </summary>
        /// <param name="map">the map, the overlength mechanism is working on</param>
        public ShaiHuludPhase(Map map)
        {
            _map = map;
            CurrentField = map.GetRandomFieldWithoutCharacter();
            CurrentField.IsApproachable = false;
            _lastCharacterEaten = false;

            _wormDespawnTimer = new Timer(_timeUntilShaiHuludDespawn);
            _wormDespawnTimer.Elapsed += OnTimerDespawnWorm;
            _wormDespawnTimer.AutoReset = false;

        }

        /// <summary>
        /// trigger the server message controller to despawn the shai hulud
        /// </summary>
        private static void OnTimerDespawnWorm(Object source, ElapsedEventArgs e)
        {
            Party.GetInstance().messageController.DoDespawnSandwormDemand();
        }

        /// <summary>
        /// choose a random character on the map 
        /// </summary>
        /// <returns>the chosen random character on the map or null, if there is no character on the map</returxns>
        private Character ChooseTargetCharacter()
        {
            Random random = new Random();
            List<Character> charactersOnMap = _map.GetCharactersOnMap();

            if (charactersOnMap.Count == 0)
            {
                // TODO: do not return null
                return null;
            }
            return charactersOnMap[random.Next(charactersOnMap.Count)];
        }

        /// <summary>
        /// moves the shai hulud to a given target character and therefore set the current field new
        /// </summary>
        /// <param name="targetCharacter"></param>
        private void MoveToTargetCharacter(Character targetCharacter)
        {
            _map.SetMapFieldAtPosition(new FlatSand(CurrentField.hasSpice, CurrentField.isInSandstorm), CurrentField.XCoordinate, CurrentField.ZCoordinate);
            _map.GetMapFieldAtPosition(CurrentField.XCoordinate, CurrentField.ZCoordinate).IsApproachable = true;

            // send map change for updating the map in the user client
            Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
            CurrentField = targetCharacter.CurrentMapfield;
            CurrentField.IsApproachable = false;
        }

        /// <summary>
        /// eat a given character so kill it
        /// </summary>
        /// <param name="targetCharacter">the targeted character, who will be eaten</param>
        /// <return>true, if the character could be eaten, otherwise false</return>
        private bool EatTargetCharacter(Character targetCharacter)
        {
            // get the id the client, whose character the target character is
            Player player = Party.GetInstance().GetPlayerByCharacterID(targetCharacter.CharacterId);
            if (player != null)
            {
                // kill target character and send message, that stats of character changed
                targetCharacter.KilledBySandworm = true;
                player.UsedGreatHouse.Characters.Remove(targetCharacter);
                Party.GetInstance().messageController.DoSendChangeCharacterStatsDemand(player.ClientID, targetCharacter.CharacterId, new CharacterStatistics(targetCharacter));

                CurrentField.DisplaceCharacter(targetCharacter);
                return true;
            }
            else
            {
                Log.Error($"There is no player with a character with the character ID {targetCharacter.CharacterId}!");
                return false;
            }  
        }

        /// <summary>
        /// check, whether there is only one player left on the map and if, whose character it is
        /// </summary>
        /// <returns>true, if there is only character left</returns>
        private bool DetermineLastPlayerStanding()
        {
            List<Character> charactersOnMap = _map.GetCharactersOnMap();

            if (charactersOnMap.Count == 1)
            {
                // only one character on the map left
                List<Player> players = Party.GetInstance().GetActivePlayers();

                if (players[0].UsedGreatHouse.Characters.Count == 0)
                {
                    // player 1 has no characters left, so player 2 has the last standing player
                    players[0].statistics.LastCharacterStanding = false;
                    players[1].statistics.LastCharacterStanding = true;
                }
                else
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
            }
            else
            {
                DetermineLastPlayerStanding();
                Character target = ChooseTargetCharacter();

                // spawn the shai hulud
                Party.GetInstance().messageController.DoSpawnSandwormDemand(target.CharacterId, target.CurrentMapfield);

                MoveToTargetCharacter(target);
                EatTargetCharacter(target);

                // despawn the shai hulud
                _wormDespawnTimer.Start();

                // after the last character was eaten, there is no character left
                _lastCharacterEaten = true;
                foreach (Character character in Party.GetInstance().map.GetCharactersOnMap())
                {
                    if (!character.killedBySandworm)
                    {
                        _lastCharacterEaten = false;
                        break;
                    }
                }
                return false;
            }
        }
    }
}
