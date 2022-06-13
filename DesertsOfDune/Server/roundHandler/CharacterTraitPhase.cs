using GameData.network.util.world;
using Server;
using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the character trait phase.
    /// </summary>
    public class CharacterTraitPhase : IGamePhase
    {
        private List<Character> _allCharacters;
        private static bool _isTraitActive = false;

        public void Execute()
        {
            GenerateTraitSequenze();

            foreach (var character in _allCharacters)
            {
                if (!character.IsDead())
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
                }
            }
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
                    }
                }
            }
        }

        public static void SetIsTraitActive(bool isActive)
        {
            _isTraitActive = isActive;
        }
    }
}
