using GameData.gameObjects;
using GameData.network.util.world;
using Server;
using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the character trait phase.
    /// </summary>
    public class CharacterTraitPhase : IGamePhase
    {
        // todo implement class character
        // private List<Character> charactersAlive;
        //private Character[] traitSequence;

        private RoundHandler parent;
        private bool characterInAction = false;
        private List<Character> allCharacters = new List<Character>();
        private Character aktiveCharacter; //TODO:if turn from previous character is over, set next character from list to aktive character

        public void Execute()
        {
            //TODO: finish implementation
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    allCharacters.Add(character);
                }
            }
            RandomizeTraitSequenze();


            throw new NotImplementedException();
        }

        /// <summary>
        /// This method randomizes the traitsequenze
        /// </summary>
        /// <returns>Returns the new sorted list of characters.</returns>
        public bool RandomizeTraitSequenze()
        {
            // todo implement logic
            var random = new Random();
            allCharacters = (List<Character>)allCharacters.OrderBy(item => random.Next());
            return true;
        }

        /// <summary>
        /// Sends a message to the client who has the next turn with the ID of the character, whos turn it is.
        /// </summary>
        public void RequestClientForNextCharacterTrait()
        {
            //TODO: finish implementation
            var characterID = allCharacters[0].CharacterId; //TODO: change this

            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character.CharacterId == characterID)
                    {
                        Party.GetInstance().messageController.DoSendTurnDemand(player.ClientID, characterID);
                    }
                }
            }
        }
    }
}
