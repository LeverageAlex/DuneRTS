using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.server.roundHandler;
using Serilog;
using GameData;
using GameData.Clients;
using GameData.Configuration;
using GameData.roundHandler;
using GameData.roundHandler.duneMovementHandler;
using GameData.roundHandler.endOfGame;
using GameData.network.util.world.character;

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
        public int _roundCounter { get; private set; }

        /// <summary>
        /// the maximum number of rounds, that should be played.
        /// If this number is reached, trigger the overlength mechanism
        /// </summary>
        private readonly int _maximumNumberOfRounds;

        /// <summary>
        /// the implementation of the overlength mechanism
        /// </summary>
        private OverLengthMechanism _overLengthMechanism;

        public bool IsOverlengthMechanismActive = false;

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
        /// states, whether the party (so the current round) is currently paused or not
        /// </summary>
        /// <remarks>
        /// This is necessary to know, because a paused game cannot be paused again and a game can only
        /// be resumed, if it is currently paused
        /// </remarks>
        public bool IsPartyPaused { get; private set; }

        /// <summary>
        /// contains the current pause request or is null, if there is no request / active pause
        /// </summary>
        public PauseRequest PauseRequest { get; private set; }

        private System.Timers.Timer maximalPauseOverTimer;

        /// <summary>
        /// false at the beginning; needed to check if the great house convention got broken in the last round
        /// </summary>
        private bool GreatHouseConventionBroken = false;

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

            SetMaximalPauseOverTimer();
        }

        /// <summary>
        /// triggers the next round, so the execution of all phases and the check, whether this is the last round
        /// </summary>
        public void NextRound()
        {
            if (GreatHouseConventionBroken != Noble.greatHouseConventionBroken)
            {
                GreatHouseConventionBroken = true;
                AddCharactersFromAtomic();
            }
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
                        Statistics[] statistics = { Party.GetInstance().GetPlayerByClientID(winnerID).statistics, Party.GetInstance().GetPlayerByClientID(loserID).statistics };
                        Party.GetInstance().messageController.DoGameEndMessage(winnerID, loserID, statistics);

                        RestartServer();
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
                        RestartServer();
                        //Party.GetInstance().messageController.OnEndGameMessage(new EndGameMessage());
                    }
                }
                _clonePhase.Execute();
                Log.Debug("Executed the clone phase.");

                Log.Debug("Execute the character trait phase...");
                _roundCounter++;
                characterTraitPhase.Execute();
                
                // increase round counter, because the round was finished
            }
        }

        private void RestartServer()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "Server.exe";
            startInfo.Arguments = string.Join(' ', Programm.startArguments);
            Process.Start(startInfo);
            Process.GetCurrentProcess().Kill();
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
                    if (player.UsedGreatHouse.GetCharactersNotSwallowedBySandworm().Count <= 0)
                    {
                        int loserID = player.ClientID;
                        int winnerID = Party.GetInstance().GetActivePlayers().Find(c => c.ClientID != player.ClientID).ClientID;
                        Statistics[] statistics = { Party.GetInstance().GetPlayerByClientID(winnerID).statistics, Party.GetInstance().GetPlayerByClientID(loserID).statistics };
                        Party.GetInstance().messageController.DoGameEndMessage(winnerID, loserID, statistics);
                        partyFinished = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetMaximalPauseOverTimer()
        {
            // Create a timer with a 100 miliseconds interval.
            maximalPauseOverTimer = new System.Timers.Timer(PartyConfiguration.GetInstance().minPauseTime);
            // Hook up the Elapsed event for the timer. 
            maximalPauseOverTimer.Elapsed += OnMaxPauseOverTimedEvent;
            maximalPauseOverTimer.AutoReset = false;
            maximalPauseOverTimer.Enabled = true;
        }

        private void OnMaxPauseOverTimedEvent(Object source, ElapsedEventArgs e)
        {
            // check, whether there is pause and if so, whether the other client can also request a resumption
            if (PauseRequest != null && PauseRequest.RequestedPause && PauseRequest.CanPauseRepealedByOtherClient())
            {
                // send a unpause offer
                Party.GetInstance().messageController.DoSendUnpauseGameOffer();

            }
        }

        /// <summary>
        /// pauses the game, so create a new pause request
        /// </summary>
        /// <param name="clientID">the ID of the client, who requested pausing the game</param>
        /// <returns>true, if the game was paused</returns>
        public bool PauseGame(int clientID)
        {
            if (IsPartyPaused)
            {
                // party already paused, so it is not possible to pause the game again
                return false;
            }
            IsPartyPaused = true;
            PauseRequest = new PauseRequest(true, clientID);

            Party.GetInstance().messageController.NetworkController.GamePaused = true;
            
            maximalPauseOverTimer.Start();
            if (characterTraitPhase != null)
            {
                characterTraitPhase.freezeTraitPhase(true);
            }

            // TODO: do not used Thread.Sleep() and <thread>.Interrupt()!!

            return true;
        }

        /// <summary>
        /// resumes the game, so create a new "pause" request with the parameter, that this request is used for resumption
        /// </summary>
        /// <returns>true, if the game was resumed</returns>
        public bool ContinueGame(int clientID)
        {
            if (!IsPartyPaused)
            {
                // party is not paused, so it is not possible to resume the game
                return false;
            }
            if (!PauseRequest.CanPauseRepealedByOtherClient())
            {
                // only the client, who requested the pause can resume it
                if (PauseRequest.ClientID != clientID)
                {
                    return false;
                }
            }
            if (characterTraitPhase != null)
            {
                characterTraitPhase.freezeTraitPhase(false);
            }
            maximalPauseOverTimer.Stop();
            IsPartyPaused = false;
            PauseRequest = new PauseRequest(false, clientID);


            Party.GetInstance().messageController.NetworkController.GamePaused = false;

            Party.GetInstance().roundHandlerThread.Interrupt();
            return true;
        }

        /// <summary>
        /// getter for the characterTraitPhase
        /// </summary>
        /// <returns>current characterTraitPhase</returns>
        public CharacterTraitPhase GetCharacterTraitPhase()
        {
            return this.characterTraitPhase;
        }
        
        /// <summary>
        /// This methods adds new characters after the greatHouseConvention gets broken for the first time.
        /// </summary>
        private void AddCharactersFromAtomic()
        {
            Player harmedPlayer = null;
            foreach (var character in Party.GetInstance().GetAllCharacters())
            {
                if (character.Shunned)
                {
                    var player = Party.GetInstance().GetPlayerByCharacterID(character.CharacterId);
                    if (player.Equals(Party.GetInstance().GetActivePlayers()[0]))
                    {
                        harmedPlayer = Party.GetInstance().GetActivePlayers()[1];
                    }
                    else
                    {
                        harmedPlayer = Party.GetInstance().GetActivePlayers()[0];
                    }
                    break;
                }
            }

            if (harmedPlayer == null)
            {
                throw new Exception("Exception happend at spwaning new characters because of atomic action in the next round.");
            }

            foreach (var character in Character.CharactersToAddAfterAtomics)
            {
                MapField fieldForCharacter = null;
                bool emptyFieldFound = false;
                while (!emptyFieldFound)
                {
                    fieldForCharacter = Party.GetInstance().map.GetRandomApproachableField();
                    if (!fieldForCharacter.IsCharacterStayingOnThisField)
                    {
                        emptyFieldFound = true;
                    }
                }
                fieldForCharacter.PlaceCharacter(character);
                character.CurrentMapfield = fieldForCharacter;
                harmedPlayer.UsedGreatHouse.Characters.Add(character);
                Party.GetInstance().messageController.DoSpawnCharacterDemand(character);
            }
            Character.CharactersToAddAfterAtomics.Clear();
        }
    }  
}
