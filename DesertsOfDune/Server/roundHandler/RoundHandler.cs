using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.server.roundHandler;
using Server;
using Server.Clients;

namespace GameData.gameObjects
{
    /// <summary>
    /// This Class handles the Rounds of the game DeserOfDune.
    /// </summary>
    public class RoundHandler
    {
        private ServerMessageController serverMessageController;
        private Party party;
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
        private ClonePhase clonePhase;
        private CharacterTraitPhase characterTraitPhase;
        private OverLengthMechanism overLengthMechanism;
        private bool overLengthMechanismActive = false;
        private bool partyFinished = false;

        /// <summary>
        /// Constructor of the class RoundHandler
        /// </summary>
        /// <param name="numbOfRounds">the maximum number of rounds specified in the pary config</param>
        public RoundHandler(int numbOfRounds, int spiceMinimum)
        {
            this.numbOfRounds = numbOfRounds;
            this.spiceMinimum = spiceMinimum;
        }

        public void SetParty(Party party)
        {
            this.party = party;
        }

        public void SetServerMessageController(ServerMessageController serverMessageController)
        {
            this.serverMessageController = serverMessageController;
        }

        /// <summary>
        /// This method Handles the Rounds of the game Dessert of Dune.
        /// </summary>
        public void HandleRounds()
        {
            for (int i = 0; i < numbOfRounds; i++)
            {
                duneMovementPhase.Execut();
                sandstormPhase.Execut();
                ((SandWorm)sandwormPhase).Execut();
                //call CheckVictory to check if after sandworm phase the last character of one house is gone and the the other player has won
                clonePhase.Execut();
                characterTraitPhase.Execut();
            }
            overLengthMechanism.Execut();
        }

        /// <summary>
        /// This method checks weather the game is in overlenth
        /// </summary>
        /// <returns>true, if the game has overlength</returns>
        public bool CheckOverLength()
        {
            if (roundCounter >= numbOfRounds)
            {
                serverMessageController.DoEndGame();
                overLengthMechanismActive = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method checks whether one Client has won the game
        /// </summary>
        /// <returns>true, if one client won the game</returns>
        public bool CheckVictory()
        {
            if (party.AreTwoPlayersRegistred())
            {
                foreach (var player in party.GetActivePlayers())
                {
                    if (player.UsedGreatHouse.Characters.Count == 0)
                    {
                        int loserID = player.ClientID;
                        int winnerID = party.GetActivePlayers().Find(c => c.ClientID != player.ClientID).ClientID;
                        serverMessageController.DoGameEndMessage(winnerID, loserID, new Statistics()); //TODO: get stats
                        partyFinished = true;
                        return true;
                    }
                }
            }
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
