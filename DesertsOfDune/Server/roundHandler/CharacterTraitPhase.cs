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
        private static bool _isTraitActive = false;
        private static Character _currentCharacter = null;
        private static Timer _timer;

        public void Execute()
        {
            SetTimer(); //initialize timer
            GenerateTraitSequenze();

            foreach (var character in _allCharacters)
            {
                _currentCharacter = character;
                if (!character.IsDead()) // check if character stays in storm
                {
                    character.resetMPandAp();
                    RequestClientForNextCharacterTrait(character.CharacterId);
                    while (_isTraitActive)
                    {
                        if ((character.APcurrent <= 0 && character.MPcurrent <= 0) || character.IsDead()) //if character has no point for action or movement left or is dead, end his turn
                        {
                            SetIsTraitActive(false);
                        }
                    }
                    _timer.Stop(); //stop timer when characterTrait is finished
                }
            }
            Party.GetInstance().RoundHandler.NextRound();
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
            _allCharacters = (List<Character>)_allCharacters.OrderBy(item => random.Next());
            return true;
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
                        SetIsTraitActive(true);
                        _timer.Start(); // starts the timer when characterTrait starts
                    }
                }
            }
        }

        public static void SetIsTraitActive(bool isActive)
        {
            _isTraitActive = isActive;
        }

        /// <summary>
        /// Starts a new timer with the time from the parameter.
        /// </summary>
        /// <param name="timeInSeconds">Time in seconds how long the timer runs.</param>
        private static void SetTimer()
        {
            int timeInMilliseconds = 0;
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