using System;
using GameData.Clients;

namespace GameData.roundHandler.endOfGame
{
    /// <summary>
    /// Checks or determines, which player won the game, after it was ended.
    /// </summary>
    public class VictoryChecker
    {
        /// <summary>
        /// creates anew victory checker
        /// </summary>
        public VictoryChecker()
        {
        }


        /// <summary>
        /// determines the winner, if the passed the overlength mechanism with the victory metrics.
        /// </summary>
        /// <returns>returns the winning player of the game</returns>
        public Player GetWinnerByCheckWinnerVictoryMetric()
        {
            // fetch both player, who played the game
            var player1 = Party.GetInstance().GetActivePlayers()[0];
            var player2 = Party.GetInstance().GetActivePlayers()[1];

            return CheckFirstVictoryMetric(player1, player2);
        }

        /// <summary>
        /// checks, which house has the bigger stock of spice
        /// </summary>
        /// <param name="player1">the first active player of the party</param>
        /// <param name="player2">the second active player of the party</param>
        /// <returns>return the player with the bigger stock of spice. If the stock of both player is equal, it will return the result of the second victory metric</returns>
        private Player CheckFirstVictoryMetric(Player player1, Player player2)
        {
            if (player1.statistics.HouseSpiceStorage > player2.statistics.HouseSpiceStorage)
            {
                return player1;
            }
            else if (player1.statistics.HouseSpiceStorage < player2.statistics.HouseSpiceStorage)
            {
                return player2;
            }
            else
            {
                return CheckSecondVictoryMetric(player1, player2);
            }
        }

        /// <summary>
        /// checks, which player's house recorded more spice during the party
        /// </summary>
        /// <param name="player1">the first active player of the party</param>
        /// <param name="player2">the second active player of the party</param>
        /// <returns>return the player with more recorded spice. If the recorded spice of both player is equal, it will the result of the third victory metric</returns>
        private Player CheckSecondVictoryMetric(Player player1, Player player2)
        {
            if (player1.statistics.TotalSpiceCollected > player2.statistics.TotalSpiceCollected)
            {
                return player1;
            }
            else if (player1.statistics.TotalSpiceCollected < player2.statistics.TotalSpiceCollected)
            {
                return player2;
            }
            else
            {
                return CheckThirdVictoryMetric(player1, player2);
            }
        }

        /// <summary>
        /// checks, which house has defeated more enemy characters.
        /// </summary>
        /// <param name="player1">the first active player of the party</param>
        /// <param name="player2">the second active player of the party</param>
        /// <returns>return the player, who defeated more enemy characters of the other house. If the amount of defeated enemy characters of both player is equal, it will the result of the second fourth metric</returns>
        private Player CheckThirdVictoryMetric(Player player1, Player player2)
        {
            if (player1.statistics.EnemiesDefeated > player2.statistics.EnemiesDefeated)
            {
                return player1;
            }
            else if (player1.statistics.EnemiesDefeated < player2.statistics.EnemiesDefeated)
            {
                return player2;
            }
            else
            {
                return Check4thVictoryMetric(player1, player2);
            }
        }

        /// <summary>
        /// checks, which house has less characters, which were swallowed by the usual sandworm
        /// </summary>
        /// <param name="player1">the first active player of the party</param>
        /// <param name="player2">the second active player of the party</param>
        /// <returns>return the player where less characters were swallowed by the usual sandworm. If the amount of both player is equal, it will the result of the second fifth metric</returns>
        private Player Check4thVictoryMetric(Player player1, Player player2)
        {
            if (player1.statistics.CharactersSwallowed < player2.statistics.CharactersSwallowed)
            {
                return player1;
            }
            else if (player1.statistics.CharactersSwallowed > player2.statistics.CharactersSwallowed)
            {
                return player2;
            }
            else
            {
                return Check5thVictoryMetric(player1, player2);
            }
        }

        /// <summary>
        /// checks, which house had the last character standing, so which house had the character, which was swallowed last by the Shai-Hulud
        /// </summary>
        /// <param name="player1">the first active player of the party</param>
        /// <param name="player2">the second active player of the party</param>
        /// <returns>returns the player, with the last character standing or null, if there is no player with last standing character</returns>
        /// TODO: do not return null
        private Player Check5thVictoryMetric(Player player1, Player player2)
        {
            if (player1.statistics.LastCharacterStanding)
            {
                return player1;
            }
            else if (player2.statistics.LastCharacterStanding)
            {
                return player2;
            }
            else
            {
                return null;
            }
        }
    }
}
