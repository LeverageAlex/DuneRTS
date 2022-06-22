using GameData.network.controller;
using GameData.network.util.world;
using GameData;
using GameData.Configuration;
using GameData.roundHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the character trait phase.
    /// </summary>
    public class CharacterTraitPhase : IGamePhase
    {
        private List<Character> _allCharacters;
        private static Character _currentCharacter = null;
        private int _currentCharacterIndex;
        private Timer _timer;

        public void Execute()
        {
            SetTimer(); //initialize timer
            _allCharacters = GenerateTraitSequenze();
            _currentCharacterIndex = 0;
            SendRequestForNextCharacter();
        }

        /// <summary>
        /// This method gets all characters and randomizes this list for the traitsequenze
        /// </summary>
        /// <returns>Returns the new sorted list of characters.</returns>
        public List<Character> GenerateTraitSequenze()
        {
            var characters = new List<Character>();
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    characters.Add(character);
                }
            }
            var random = new Random();
            return characters.OrderBy(item => random.Next()).ToList<Character>();
        }

        /// <summary>
        /// preparing the turn for the next character in the random sequence
        /// </summary>
        public void SendRequestForNextCharacter()
        {
           // _timer.Stop();
            if (_currentCharacterIndex < _allCharacters.Count)
            {
                _currentCharacter = _allCharacters[_currentCharacterIndex++];
                _currentCharacter.SetSilent();
                if (!_currentCharacter.IsDead() && !_currentCharacter.KilledBySandworm && !_currentCharacter.IsInSandStorm(Party.GetInstance().map)) // check if character is dead or staying in storm
                {
                    _currentCharacter.resetMPandAp();
                    foreach (var player in Party.GetInstance().GetActivePlayers())
                    {
                        foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                        {
                            if (character.CharacterId == _currentCharacter.CharacterId)
                            {
                                Party.GetInstance().messageController.DoSendChangeCharacterStatsDemand(player.ClientID, character.CharacterId, new CharacterStatistics(character));
                            }
                        }
                    }
                    RequestClientForNextCharacterTrait(_currentCharacter.CharacterId);
                }
                else
                {
                    SendRequestForNextCharacter();
                }

            }
            else
            {
                Party.GetInstance().RoundHandler.NextRound();
            }

        }

        /// <summary>
        /// stop timer
        /// </summary>
        public void StopTimer()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Sends a message to the client who has the next turn with the ID of the character, whos turn it is.
        /// </summary>
        public void RequestClientForNextCharacterTrait(int characterID)
        {
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character.CharacterId == characterID)
                    {
                        Party.GetInstance().messageController.DoSendTurnDemand(player.ClientID, characterID); //request client to execute a characterTrait
                        _timer.Start(); // starts the timer when characterTrait starts
                    }
                }
            }
        }


        /// <summary>
        /// Starts a new timer with the time from the parameter.
        /// </summary>
        /// <param name="timeInSeconds">Time in seconds how long the timer runs.</param>
        public void SetTimer()
        {
            int timeInMilliseconds = PartyConfiguration.GetInstance().actionTimeUserClient;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character == _currentCharacter)
                    {
                        if (player is HumanPlayer)
                        {
                            timeInMilliseconds = PartyConfiguration.GetInstance().actionTimeUserClient;
                        }
                        else if (player is AIPlayer)
                        {
                            timeInMilliseconds = PartyConfiguration.GetInstance().actionTimeAiClient;
                        }
                    }
                }
            }
            _timer = new Timer(timeInMilliseconds);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
        }

        /// <summary>
        /// This Event is called when the timer runs out and then disconnect the client.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            /**string sessionID = "";
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.Characters)
                {
                    if (character.CharacterId == _currentCharacter.CharacterId)
                    {
                        sessionID = player.SessionID;
                    }
                }
            }
            // ((ServerConnectionHandler)Party.GetInstance().messageController.NetworkController.connectionHandler).sessionManager.CloseSession(sessionID, WebSocketSharp.CloseStatusCode.Normal, "Timeout happend in characterTraitPhase!");
           // SendRequestForNextCharacter(); */

            Party.GetInstance().messageController.OnEndTurnRequestMessage(new network.messages.EndTurnRequestMessage(Party.GetInstance().GetPlayerByCharacterID(_currentCharacter.CharacterId).ClientID, _currentCharacter.CharacterId));
        }


        public void freezeTraitPhase(bool pause)
        {
            if (pause)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        public Character GetCurrentTurnCharacter()
        {
            return _currentCharacter;
        }

        public Timer GetTimer()
        {
            return this._timer;
        }
    }
}