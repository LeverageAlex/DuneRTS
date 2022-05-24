using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Determines the winner if the game goes to the overlengthMechanism with the victory metrics.
        /// </summary>
        /// <returns>Returns the winner of the game</returns>
        public Player CheckWinnerVictoryMetric()
        {
            Player winner;
            Player p1 = party.GetActivePlayers()[0];
            Player p2 = party.GetActivePlayers()[1];

            winner = CheckFirstVictoryMetric(p1, p2);
            if (winner != null)
            {
                return winner;
            }
            winner = CheckSecondVictoryMetric(p1, p2);
            if (winner != null)
            {
                return winner;
            }
            winner = CheckThirdVictoryMetric(p1, p2);
            if (winner != null)
            {
                return winner;
            }
            winner = Check4thVictoryMetric(p1, p2);
            if (winner != null)
            {
                return winner;
            }
            winner = Check5thVictoryMetric(p1, p2);
            if(winner != null)
            {
                return winner;
            }
            return null; //should not come to this part
            //nobody has won, TODO: InfoMessage
        }

        /// <summary>
        /// Checks, which house has the bigger stock of spice.
        /// </summary>
        /// <param name="player1">The first active player of the party.</param>
        /// <param name="player2">The second active player of the party.</param>
        /// <returns>Return the player with the bigger stock of spice. If the stock is of both player is equal it will return null.</returns>
        private Player CheckFirstVictoryMetric(Player player1, Player player2)
        {
            if (player1.UsedGreatHouse.City.GetSpiceCount() > player2.UsedGreatHouse.City.GetSpiceCount())
            {
                return player1;
            }
            else if (player1.UsedGreatHouse.City.GetSpiceCount() < player2.UsedGreatHouse.City.GetSpiceCount())
            {
                return player2;
            }
            return null;
        }

        /// <summary>
        /// Checks, which player's house recorded more spice during the party.
        /// </summary>
        /// <param name="player1">The first active player of the party.</param>
        /// <param name="player2">The second active player of the party.</param>
        /// <returns>Return the player more recorded spice. If the recorded spice of both player is equal it will return null.</returns>
        private Player CheckSecondVictoryMetric(Player player1, Player player2)
        {
            throw new NotImplementedException("not impelmented");
        }

        /// <summary>
        /// Checks, which house has defeated more enemy characters.
        /// </summary>
        /// <param name="player1">The first active player of the party.</param>
        /// <param name="player2">The second active player of the party.</param>
        /// <returns>Return the player who defeated more enemy characters of the other house. If the amount of defeated enemy characters of both player is equal it will return null.</returns>
        private Player CheckThirdVictoryMetric(Player player1, Player player2)
        {
            throw new NotImplementedException("not impelmented");
        }

        /// <summary>
        /// Checks, at which house less characters were swallowed by the usual sandworm.
        /// </summary>
        /// <param name="player1">The first active player of the party.</param>
        /// <param name="player2">The second active player of the party.</param>
        /// <returns>Return the player where less characters were swallowed by the usual sandworm. If the amount of both player is equal it will return null.</returns>
        private Player Check4thVictoryMetric(Player player1, Player player2)
        {
            throw new NotImplementedException("not impelmented");
        }

        /// <summary>
        /// Checks, at which house the last character standing is..
        /// </summary>
        /// <param name="player1">The first active player of the party.</param>
        /// <param name="player2">The second active player of the party.</param>
        /// <returns>Returns the player with the LastCharacterStanding.</returns>
        private Player Check5thVictoryMetric(Player player1, Player player2)
        {
            throw new NotImplementedException("not impelmented");
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
