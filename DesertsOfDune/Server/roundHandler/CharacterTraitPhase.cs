using GameData.network.controller;
using GameData.network.util.world;
using GameData;
using GameData.Configuration;
using GameData.roundHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Serilog;

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

        /// <summary>
        /// This method executes the Character trait phase
        /// </summary>
        public void Execute()
        {
            SetTimer(); //initialize timer
            _allCharacters = GenerateTraitSequenze(); // set the radomized list of characters to _allCharacters
            _currentCharacterIndex = 0;
            SendRequestForNextCharacter(); // requesting the first character of the list for his trait
        }


        /// <summary>
        /// This method gets all characters and randomizes this list for the next traitsequenze
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
            if (_timer != null)
            {
                _timer.Stop();

                if (_currentCharacterIndex < _allCharacters.Count)
                {
                    _currentCharacter = _allCharacters[_currentCharacterIndex++]; // set the new current character whos turn it is
                    _currentCharacter.SetSilent(); // set the current character silent before his trait starts
                    if (!_currentCharacter.IsDead() && !_currentCharacter.KilledBySandworm && !_currentCharacter.IsInSandStorm(Party.GetInstance().map)) // check if character is dead or staying in storm
                    {
                        _currentCharacter.resetMPandAp();
                        _currentCharacter.SteppedOnSandfield = false;
                        var player = Party.GetInstance().GetPlayerByCharacterID(_currentCharacter.CharacterId);
                        foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                        {
                            if (character.CharacterId == _currentCharacter.CharacterId)
                            {
                                Party.GetInstance().messageController.DoSendChangeCharacterStatsDemand(player.ClientID, character.CharacterId, new CharacterStatistics(character));
                            }
                        }
                        RequestClientForNextCharacterTrait(_currentCharacter.CharacterId);
                    }
                    else
                    {
                        SendRequestForNextCharacter(); // if character is dead or staying in storm request the next character
                    }

                }
                else
                {
                    Party.GetInstance().RoundHandler.NextRound(); // if all characters had its turn the next round in the round handler should be started
                }
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
        ///<param name="characterID">the character which is on turn</param>
        public void RequestClientForNextCharacterTrait(int characterID)
        {
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character.CharacterId == characterID)
                    {
                        Party.GetInstance().messageController.DoSendTurnDemand(player.ClientID, characterID); //request client to execute a characterTrait
                        if (_timer != null)
                        {
                            _timer.Start(); // starts the timer when characterTrait starts
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Starts a new timer with the time from the parameter.
        /// </summary>
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
            _timer.AutoReset = false;
        }

        /// <summary>
        /// This Event is called when the timer runs out and then disconnect the client.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Log.Warning($"Timeout for the client {_currentCharacter.CharacterId}");

            Party.GetInstance().messageController.OnEndTurnRequestMessage(new network.messages.EndTurnRequestMessage(Party.GetInstance().GetPlayerByCharacterID(_currentCharacter.CharacterId).ClientID, _currentCharacter.CharacterId));
        }

        /// <summary>
        /// This method freezes the timer if the player pauses the game
        /// </summary>
        /// <param name="pause">bool if the player pauses or unpauses the game</param>
        public void FreezeTraitPhase(bool pause)
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

        /// <summary>
        /// Get the current character which is on turn
        /// </summary>
        /// <returns>return the current character</returns>
        public Character GetCurrentTurnCharacter()
        {
            return _currentCharacter;
        }

        /// <summary>
        /// Get the timer which is currently setted
        /// </summary>
        /// <returns>return the current timer</returns>
        public Timer GetTimer()
        {
            return this._timer;
        }
    }
}