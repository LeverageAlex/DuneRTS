using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using GameData.gameObjects;
using GameData.network.util.world.mapField;

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
        private MapField[,] map;

        /// <summary>
        /// This method triggers the cloning of a character by chance.
        /// </summary>
        /// <returns>true, if the cloning is triggered</returns>
        public void CloneCharacters(List<Character> clonableCharacters, City city)
        {
            foreach (Character character in clonableCharacters)
            {

                if (character.IsDead() && !(character.KilledBySandworm))
                {
                    Random random = new Random();
                    if (cloneProbability + random.NextDouble() >= 1.0)
                    {
                        CloneCharacter(character,city);
                    }
                }
            }
        }

        /// <summary>
        /// This method clones a specific character
        /// </summary>
        /// <returns>true, if the character was cloned</returns>
        public void CloneCharacter(Character character, City city)
        {
            MapField mapField =  DetermineCloneSpawnPosition(city);
            mapField.Character = character;
            character.resetMPandAp();
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
                    int indexX = ChoosRandomNeighborIndexX(city);
                    int indexZ = ChoosRandomNeighborIndexZ(city);
                    if (map[indexX,indexZ].IsAproachable)
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
                    if (map[i,j].IsAproachable)
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
        private int ChoosRandomNeighborIndexX(City city)
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
        private int ChoosRandomNeighborIndexZ(City city)
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

    }
}
