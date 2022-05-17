using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the spawn of a character
    /// </summary>
    public class SpawnCharacterMessage : TurnMessage
    {
        private Position position;
        private Character character;
        private CharacterType characterType;

        /// <summary>
        /// Constructor of the class SpawnCharacterMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="position">the position of the character</param>
        /// <param name="character">the character</param>
        /// <param name="characterType">the type of the character</param>
        public SpawnCharacterMessage(int clientID, int characterID, Position position, Character character, CharacterType characterType) : base(characterID,clientID,MessageType.SPAWN_CHARACTER)
        {
            this.position = position;
            this.character = character;
            this.characterType = characterType;
        }

        public CharacterType GetCharacterTypeFromCharacter()
        {
            return this.characterType;
        }

        public string[] GetAttributesFromCharacter()
        {
            return null;
        }
    }
}
