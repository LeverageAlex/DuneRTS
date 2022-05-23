using GameData.gameObjects;
using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the character trait phase.
    /// </summary>
    public class CharacterTraitPhase : GamePhase
    {
        // todo implement class character
        // private List<Character> charactersAlive;
        //private Character[] traitSequence;
        private RoundHandler parent;


        public void Execut()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method randomizes the traitsequenze
        /// </summary>
        /// <returns>true, if the traitsequenze was changed.</returns>
        public bool RandomizeTraitSequenze()
        {
            // todo implement logic
            return false;
        }
    }
}
