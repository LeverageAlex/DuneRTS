using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using GameData.gameObjects;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the ClonePhase.
    /// </summary>
    public class ClonePhase
    {
        List<Character> clonableCharacters;
        private RoundHandler parent;
        private double cloneProbability = 0.5;

        /// <summary>
        /// This method triggers the cloning of a character by chance.
        /// </summary>
        /// <returns>true, if the cloning is triggered</returns>
        public bool CalculateCloning(Character character)
        {
            if (character.IsDead() && !(character.KilledBySandworm))
            {
                Random random = new Random();
                if (cloneProbability + random.NextDouble() >= 1.0)
                {
                    CloneCharacter(character);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method clones a specific character
        /// </summary>
        /// <returns>true, if the character was cloned</returns>
        public bool CloneCharacter(Character character)
        {
            // todo implement logic
            return false;
        }

    }
}
