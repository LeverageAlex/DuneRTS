using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This Class is responsible for handling Sandworm in normal mode.
    /// </summary>
    public class Sandworm : SandwormPhase
    {
        private bool sandwormExists;
        private Character targetCharacter;
        private MapField[][] fields;
        
        /// <summary>
        /// This method handles the Sandworm movement.
        /// </summary>
        /// <returns>true, if sandworm was moved</returns>
        public bool MoveSandworm()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// This method checks the path of the sandworm.
        /// </summary>
        /// <returns>true, if</returns>
        public bool CheckSandwormPhath()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// This method checks for loud characters.
        /// </summary>
        /// <returns>true, if a loud character exists.</returns>
        public bool CheckLoudness()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// This method chooses the target character.
        /// </summary>
        /// <returns>true, if a target character was choosen.</returns>
        public bool ChooseTargetCharacter()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// This method spawns a Sandworm.
        /// </summary>
        /// <returns>true, if a sandworm was spawned.</returns>
        public bool Spawn()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// This method checks the target character
        /// </summary>
        /// <returns>true, if a target character exists</returns>
        public bool CheckTargetCharacter()
        {
            // TODO: implement logic
            return false;
        }
    }
}
