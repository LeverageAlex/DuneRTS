using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.server.roundHandler;
using Serilog;
using Server;
using Server.Clients;
using Server.Configuration;
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
        private OverLengthMechanism _overLengthMechanism;

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
        public SandstormPhase SandstormPhase { get; }
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
            this._victoryChecker = new VictoryChecker();

            // initialize game phases
            this._duneMovementPhase = new DuneMovementPhase(map);
            this.SandstormPhase = new SandstormPhase(map);
            this._sandwormPhase = new SandwormPhase(map);
            this._clonePhase = new ClonePhase(map, PartyConfiguration.GetInstance().cloneProbability);
            this._spiceBlow = new SpiceBlow(map);
            this.characterTraitPhase = new CharacterTraitPhase();

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
                Log.Debug("Executed the spice blow.");
            }

            // check, whether the round limit is exceeded
            if (!IsOverlengthMechanismActive && IsLastRoundOver ())
            {
                _overLengthMechanism = new OverLengthMechanism(this._map);
                Log.Debug("The last round is over, so start the overlength mechanism.");
                
                NextRound();
            }
            else
            {
                // execute the server side rounds
                _duneMovementPhase.Execute();
                Log.Debug("Executed the dune movement phase.");

                SandstormPhase.Execute();
                Log.Debug("Executed the sandstorm phase.");

                if (IsOverlengthMechanismActive)
                {
                    bool finishedGame = _overLengthMechanism.Execute();
                    Log.Debug("Executed one round in the overlength mechanism.");

                    if (finishedGame)
                    {
                        Player winner = this._victoryChecker.GetWinnerByCheckWinnerVictoryMetric();
                        int winnerID = winner.ClientID;
                        int loserID = Party.GetInstance().GetActivePlayers().Find(c => c.ClientID != winner.ClientID).ClientID;
                        Party.GetInstance().messageController.DoGameEndMessage(winnerID, loserID, new Statistics());

                        Log.Information("The overlength mechanism was finished, so the game is over! \n The player " + winner.ClientName + " won the game!");
                    }
                }
                else
                {
                    _sandwormPhase.Execute();
                    Log.Debug("Executed the sandworm phase.");
                    // call CheckVictory to check if after sandworm phase the last character of one house is gone and the the other player has won
                    if (CheckVictory())
                    {
                        Log.Information("The game is over. One player has no characters left");
                    }
                }
                _clonePhase.Execute();
                Log.Debug("Executed the clone phase.");

                Log.Debug("Execute the character trait phase...");
                 characterTraitPhase.Execute();
                
                // increase round counter, because the round was finished
                _roundCounter++;
                Thread.Sleep(2000);
                NextRound();
            }
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
