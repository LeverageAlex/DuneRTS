using GameData.network.controller;
using GameData.network.util.world;
using Server;
using Server.Configuration;
using Server.roundHandler;
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
        private static Timer _timer;

        public void Execute()
        {
            SetTimer(); //initialize timer
            GenerateTraitSequenze();
            _currentCharacterIndex = 0;
            SendRequestForNextCharacter();
        }

        

        /// <summary>
        /// This method gets all characters and randomizes this list for the traitsequenze
        /// </summary>
        /// <returns>Returns the new sorted list of characters.</returns>
        public bool GenerateTraitSequenze()
        {
            _allCharacters = new List<Character>();
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    _allCharacters.Add(character);
                }
            }
            var random = new Random();
            _allCharacters.OrderBy(item => random.Next());
            return true;
        }

        /// <summary>
        /// preparing the turn for the next character in the random sequence
        /// </summary>
        public void SendRequestForNextCharacter()
        {
            if (_currentCharacterIndex < _allCharacters.Count -1 )
            {
                _currentCharacterIndex++;
                _currentCharacter = _allCharacters[_currentCharacterIndex];
                _currentCharacter.SetSilent();
                if (!_currentCharacter.IsDead() && !_currentCharacter.IsInSandStorm(Party.GetInstance().map)) // check if character is dead or staying in storm
                {
                    _currentCharacter.resetMPandAp();
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
                        //SetIsTraitActive(true);
                         _timer.Start(); // starts the timer when characterTrait starts
                    }
                }
            }
        }


        /// <summary>
        /// Starts a new timer with the time from the parameter.
        /// </summary>
        /// <param name="timeInSeconds">Time in seconds how long the timer runs.</param>
        private static void SetTimer()
        {
            int timeInMilliseconds = PartyConfiguration.GetInstance().actionTimeUserClient;
            foreach(var player in Party.GetInstance().GetActivePlayers())
            {
                foreach(var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    if(character == _currentCharacter)
                    {
                        if (player is HumanPlayer){
                            timeInMilliseconds = PartyConfiguration.GetInstance().actionTimeUserClient;
                        }
                        else if(player is AIPlayer){
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
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            string sessionID = "";
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
            ((ServerConnectionHandler)Party.GetInstance().messageController.NetworkController.connectionHandler).sessionManager.CloseSession(sessionID, WebSocketSharp.CloseStatusCode.Normal, "Timeout happend in characterTraitPhase!");
        }
    }
}