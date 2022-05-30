using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using GameData.gameObjects;
using GameData.network.util.world.mapField;
using Server.roundHandler;
using Server.Configuration;
using Server;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class handles the ClonePhase.
    /// </summary>
    public class ClonePhase : IGamePhase
    {
        private double cloneProbability;
        private MapField[,] map;

        /// <summary>
        /// Constructor of the class ClonPhase
        /// </summary>
        public ClonePhase()
        {
            this.cloneProbability = PartyConfiguration.GetInstance().cloneProbability;
        }

        /// <summary>
        /// This method triggers the cloning of a character by chance.
        /// </summary>
        /// <returns>true, if the cloning is triggered</returns>
        public void CloneCharacters(GreatHouse greatHouse)
        {
            List<Character> clonableCharacters = DetermineClonableCharacters(greatHouse.Characters);
            foreach (Character character in clonableCharacters)
            {
                Random random = new Random();
                if (cloneProbability + random.NextDouble() >= 1.0)
                {
                    CloneCharacter(character, greatHouse.City);
                }
            }
        }
        
        /// <summary>
        /// This method determines the clonable characters out of all characters of a house
        /// </summary>
        /// <param name="characters">all characters of a House</param>
        /// <returns>all clonable characters of a House</returns>
        private List<Character> DetermineClonableCharacters(List<Character> characters)
        {
            List<Character> clonableCharacters = new List<Character>();
            foreach (Character character in characters)
            {
                if (character.IsDead() && !character.KilledBySandworm)
                {
                    clonableCharacters.Add(character);
                }
            }
            return clonableCharacters;
        }

        /// <summary>
        /// This method clones a specific character
        /// </summary>
        /// <returns>true, if the character was cloned</returns>
        public void CloneCharacter(Character character, City city)
        {
            MapField mapField = DetermineCloneSpawnPosition(city);
            mapField.Character = character;
            character.ResetData();
            Party.GetInstance().messageController.DoSpawnCharacterDemand(character);
        }

        /// <summary>
        /// This method determines the position of the cloned Character
        /// </summary>
        /// <param name="city">the city of the GreatHouse</param>
        /// <returns></returns>
        public MapField DetermineCloneSpawnPosition(City city)
        {
            if (FreeNeighborFieldExists(city))
            {
                while(true)
                {
                    int indexX = ChooseRandomNeighborIndexX(city);
                    int indexZ = ChooseRandomNeighborIndexZ(city);
                    if (map[indexX,indexZ].IsApproachable)
                    {
                        return map[indexX,indexZ];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Thies method determines weather a free neighbor field of the city field exists
        /// </summary>
        /// <param name="city">the city of the GreatHouse</param>
        /// <returns>true, if a free neighbor field exists</returns>
        private bool FreeNeighborFieldExists(City city)
        {
            int xStart = city.XCoordinate - 1;
            int zStart = city.ZCoordinate - 1;
            int xEnd = city.XCoordinate + 2;
            int zEnd = city.ZCoordinate + 2;
            if (city.XCoordinate == 0)
            {
                xStart = city.XCoordinate;
            } 
            if (city.ZCoordinate == 0)
            {
                zStart = city.ZCoordinate;
            }
            if (city.XCoordinate == map.GetLength(0))
            {
                xEnd = city.XCoordinate + 1;
            }
            if (city.ZCoordinate == map.GetLength(1))
            {
                xEnd = city.XCoordinate + 1;
            }
            for(int i = xStart; i < xEnd; i++)
            {
                for(int j = zStart; j < zEnd; j++)
                {
                    if (map[i,j].IsApproachable)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This method determines a random x coordinate that is next to the city
        /// </summary>
        /// <param name="city">the city of the GreatHouse</param>
        /// <returns>the randomly selected coordinate</returns>
        private int ChooseRandomNeighborIndexX(City city)
        {
            Random random = new Random();
            int start = city.XCoordinate - 2;
            int end = city.XCoordinate + 2;
            if (city.XCoordinate == 0)
            {
                start = city.XCoordinate - 1;
            }
            if (city.XCoordinate == map.GetLength(0)-1)
            {
                end = city.XCoordinate + 1;
            }
            return random.Next(start, end);
        }

        /// <summary>
        /// This method determines a random z coordinate that is next to the city
        /// </summary>
        /// <param name="city">the city of the GreatHouse</param>
        /// <returns>the randomly selected coordinate</returns>
        private int ChooseRandomNeighborIndexZ(City city)
        {
            Random random = new Random();
            int start = city.ZCoordinate - 2;
            int end = city.ZCoordinate + 2;
            if (city.ZCoordinate == 0)
            {
                start = city.ZCoordinate - 1;
            }
            if (city.ZCoordinate == map.GetLength(1) - 1)
            {
                end = city.ZCoordinate + 1;
            }
            return random.Next(start, end);
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
