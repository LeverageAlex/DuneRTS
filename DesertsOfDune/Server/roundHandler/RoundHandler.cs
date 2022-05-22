using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.server.roundHandler;

namespace GameData.gameObjects
{
    /// <summary>
    /// This Class handles the Rounds of the game DeserOfDune.
    /// </summary>
    public class RoundHandler
    {
        private int roundCounter;
        private int currentSpice;
        public int CurrentSpice { get { return currentSpice; } set { currentSpice = value; } }
        private readonly int numbOfRounds;
        private readonly int spiceMinimum;
        private MapField[,] map;
        public MapField[,] Map { get { return map; } set { map = value; } }
        public int SpiceMinimum { get { return spiceMinimum; } }
        private GreatHouseSelection greatHouseSelection;
        private SpiceBlow spiceBlow;
        private DuneMovementPhase duneMovementPhase;
        private SandstormPhase sandstormPhase;
        private SandwormPhase sandwormPhase;
        private List<ClonePhase> clonePhaseList;
        private CharacterTraitPhase characterTraitPhase;

        /// <summary>
        /// Constructor of the class RoundHandler
        /// </summary>
        /// <param name="numbOfRounds">the maximum number of rounds specified in the pary config</param>
        public RoundHandler(int numbOfRounds, int spiceMinimum)
        {
            this.numbOfRounds = numbOfRounds;
            this.spiceMinimum = spiceMinimum;
        }


        /// <summary>
        /// This method checks weather the game is in overlenth
        /// </summary>
        /// <returns>true, if the game has overlength</returns>
        public bool CheckOverLength()
        {
            if (roundCounter >= numbOfRounds)
            {
                return true;
            }
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
