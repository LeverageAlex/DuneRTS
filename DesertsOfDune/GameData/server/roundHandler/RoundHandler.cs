using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.server.roundHandler;

namespace GameData.gameObjects
{
    /// <summary>
    /// This Class handles the Rounds of the game DeserOfDune.
    /// </summary>
    public class RoundHandler
    {
        private int roundCounter;
        private GreatHouseSelection greatHouseSelection;
        private SpiceBlow spiceBlow;
        private DuneMovementPhase duneMovementPhase;
        private SandstormPhase sandstormPhase;
        private SandwormPhase sandwormPhase;
        private List<ClonePhase> clonePhaseList;
        private CharacterTraitPhase characterTraitPhase;


        /// <summary>
        /// This method checks weather the game is in overlenth
        /// </summary>
        /// <returns>true, if the game has overlength</returns>
        public bool CheckOverLength()
        {
            // TODO implement logic
            return false;
        }

        /// <summary>
        /// This method checks weather one Client has won the game
        /// </summary>
        /// <returns>true, if one client won the game</returns>
        public bool CheckVictory()
        {
            // TODO implement logic
            return false;
        }

        /// <summary>
        /// This method checks weather the spice threshold is reached
        /// </summary>
        /// <returns>true, if the spice threshold is reached.</returns>
        public bool CheckSpiceThreshold()
        {
            // TODO implement logic
            return false;
        }

        /// <summary>
        /// This method is responsible for pausing the game.
        /// </summary>
        /// <returns>true, if the game was paused</returns>
        public bool PauseGame()
        {
            // TODO implement logic
            PauseGameMessage pauseGameMessage = new PauseGameMessage(1234, true);
            return false;
        }

        /// <summary>
        /// This method is responsible for continuing the game.
        /// </summary>
        /// <returns>true, if the game was continued</returns>
        public bool ContinueGame()
        {
            // TODO implement logic
            return false;
        }
    }
}
