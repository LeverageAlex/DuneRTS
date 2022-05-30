using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.server.roundHandler;
using Server;
using Server.Clients;
using Server.roundHandler.duneMovementHandler;
using Server.roundHandler.endOfGame;

namespace GameData.gameObjects
{
    /// <summary>
    /// This Class handles the Rounds of the game DeserOfDune.
    /// </summary>
    public class RoundHandler
    {
        /// <summary>
        /// the counter, which state the current round number
        /// </summary>
        private int _roundCounter;

        /// <summary>
        /// the maximum number of rounds, that should be played.
        /// If this number is reached, trigger the overlength mechanism
        /// </summary>
        private readonly int _maximumNumberOfRounds;

        /// <summary>
        /// the implementation of the overlength mechanism
        /// </summary>
        private readonly OverLengthMechanism _overLengthMechanism;

        private bool IsOverlengthMechanismActive = false;

        /// <summary>
        /// checker, which determines the winner of the party after the overlength mechanism
        /// </summary>
        private readonly VictoryChecker _victoryChecker;

        /// <summary>
        /// the map reference of the game, the round handler execute the phases on
        /// </summary>
        private readonly Map _map;

        /// <summary>
        /// game phases, handled only by the server (dune movement, sandstorm, sandworm, cloning, overlength mechanism)
        /// </summary>
        private readonly DuneMovementPhase _duneMovementPhase;
        private readonly SandstormPhase _sandstormPhase;
        private readonly SandwormPhase _sandwormPhase;
        private readonly ClonePhase _clonePhase;

        /// <summary>
        /// game phase for the trait phase
        /// </summary>
        private CharacterTraitPhase characterTraitPhase;

        /// <summary>
        /// game phase for the (optional) spice blow
        /// </summary>
        private readonly SpiceBlow _spiceBlow;

        /// <summary>
        /// the minimum of spice, needed on the map.
        /// </summary>
        /// <remarks>
        /// if this current spice is falls below this limit, a spice blow happens
        /// </remarks>
        private readonly int _spiceMinimum;


        private bool partyFinished = false;

        /// <summary>
        /// Constructor of the class RoundHandler
        /// </summary>
        /// <param name="numbOfRounds">the maximum number of rounds specified in the pary config</param>
        public RoundHandler(int numbOfRounds, int spiceMinimum, Map map)
        {
            this._maximumNumberOfRounds = numbOfRounds;
            this._spiceMinimum = spiceMinimum;
            this._map = map;

            // initialize variables
            this._roundCounter = 0;

            // initialize game phases
            this._duneMovementPhase = new DuneMovementPhase(map);
            this._sandstormPhase = new SandstormPhase(map);
        }

        /// <summary>
        /// triggers the next round, so the execution of all phases and the check, whether this is the last round
        /// </summary>
        public void NextRound()
        {
            // check, whether a spice blow is necessary
            if (_spiceBlow.IsSpiceBlowNecessary(this._spiceMinimum, this._map.GetAmountOfSpiceOnMap()))
            {
                _spiceBlow.Execute();
            }

            // check, whether the round limit is exceeded
            if (IsLastRoundOver())
            {
                _overLengthMechanism.Execute();
            }
            else
            {
                // execute the server side rounds
                _duneMovementPhase.Execute();
                _sandstormPhase.Execute();
                _sandwormPhase.Execute();
                //call CheckVictory to check if after sandworm phase the last character of one house is gone and the the other player has won
                _clonePhase.Execute();
                characterTraitPhase.Execute();

                // increase round counter, because the round was finished
                _roundCounter++;
            }
        }

        /// <summary>
        /// This method Handles the Rounds of the game Dessert of Dune.
        /// </summary>
        public void HandleRounds()
        {
            for (int i = 0; i < _maximumNumberOfRounds; i++)
            {
                _duneMovementPhase.Execute();
                _sandstormPhase.Execute();
                ((SandWorm)_sandwormPhase).Execute();
                //call CheckVictory to check if after sandworm phase the last character of one house is gone and the the other player has won
                _clonePhase.Execute();
                characterTraitPhase.Execute();
            }
            _overLengthMechanism.Execute();
        }

        /// <summary>
        /// checks, whether the game reached the last round
        /// </summary>
        /// <returns>true, if the game has reached the last round and need to enter the overlength mechanism</returns>
        public bool IsLastRoundOver()
        {
            if (this._roundCounter >= this._maximumNumberOfRounds)
            {
                Party.GetInstance().messageController.DoEndGame();
                IsOverlengthMechanismActive = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method checks whether one Client has won the game and if one has, send GameEndMessage to the client.
        /// </summary>
        /// <returns>true, if one client won the game</returns>
        public bool CheckVictory()
        {
            if (Party.GetInstance().AreTwoPlayersRegistred())
            {
                foreach (var player in Party.GetInstance().GetActivePlayers())
                {
                    if (player.UsedGreatHouse.Characters.Count == 0) //TODO: check, if in Characters.count are also defeated characters which are not cloned again yet
                    {
                        int loserID = player.ClientID;
                        int winnerID = Party.GetInstance().GetActivePlayers().Find(c => c.ClientID != player.ClientID).ClientID;
                        Party.GetInstance().messageController.DoGameEndMessage(winnerID, loserID, new Statistics()); //TODO: get stats for both players
                        partyFinished = true;
                        return true;
                    }
                }
            }
            return false;
        } 

        /// <summary>
        /// This method is responsible for pausing the game.
        /// </summary>
        /// <returns>true, if the game was paused</returns>
        public bool PauseGame()
        {
            // TODO implement logic
            return false;
        }

        /// <summary>
        /// This method is responsible for continuing the game.
        /// </summary>
        /// <returns>true, if the game was continued</returns>
        public bool ContinueGame()
        {
            // TODO implement logic
            return false;
        }
    }
}
