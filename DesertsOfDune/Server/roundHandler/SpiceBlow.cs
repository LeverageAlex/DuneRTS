using System;
using System.Collections.Generic;
using System.Text;
using GameData.gameObjects;
using GameData.network.util.enums;
using GameData.network.util.world;

namespace GameData.server.roundHandler
{

    /// <summary>
    /// This Class is responsible for handling the spice blow.
    /// </summary>
    public class SpiceBlow
    {
        private MapField[,] mapFields;
       
        /// <summary>
        /// Constructor of the Class SpiceBlow
        /// </summary>
        /// <param name="mapFields">the map of the game</param>
        public SpiceBlow(MapField[,] mapFields)
        {
            this.mapFields = mapFields;
        }
        /// <summary>
        /// This method does the SpiceBlow.
        /// </summary>
        /// <returns>true, if the spiceblow was possible</returns>
        public bool RandomSpiceBlow(int spiceMinimum, int currentSpice)
        {
            if (SpiceBlowIsApplicable(spiceMinimum,currentSpice))
            {
                int indexX;
                int indexZ;
                while (true)
                {
                    indexX = ChoosRandomMapFieldIndexX();
                    indexZ = ChoosRandomMapFieldIndexZ();
                    if (mapFields[indexX, indexZ].TileType ==  "DUNE")
                    {
                        break;
                    }
                }
                ChangeFieldAndNeighborsRandomly(indexX,indexZ);
                return true;
            }
            return false;

        }

        /// <summary>
        /// This method chooses one random x-coordinate on the map
        /// </summary>
        /// <returns>the coordinate on the map</returns>
        public int ChoosRandomMapFieldIndexX()
        {
            int mapLengthX = mapFields.GetLength(0);
            Random random = new Random();
            return random.Next(0, mapLengthX);
        }

        /// <summary>
        /// This method chooses one random z-coordinate on the map
        /// </summary>
        /// <returns>the coordinate on the map</returns>
        public int ChoosRandomMapFieldIndexZ()
        {
            int mapLengthZ = mapFields.GetLength(1);
            Random random = new Random();
            return random.Next(0, mapLengthZ);
        }

        /// <summary>
        /// This method changes the selected Field and its neightbors to Flatsand/Dune by chance
        /// </summary>
        /// <param name="indexX">the x-coordinate of the selcted field</param>
        /// <param name="indexZ">the z-coordinate of the selected field</param>
        public void ChangeFieldAndNeighborsRandomly(int indexX, int indexZ)
        {
            for(int i = indexX-1; i < indexX+2; i++)
            {
                for(int j = indexZ-1; j < indexZ+2; j++)
                {
                    if (i < mapFields.GetLength(0) && i >= 0 && j < mapFields.GetLength(1) && j >= 0)
                    {
                        Random random = new Random();
                        if (random.NextDouble() > 0.5)
                        {
                            mapFields[i, j].TileType = Enum.GetName(typeof(TileType), network.util.enums.TileType.DUNE);
                        } else
                        {
                            mapFields[i, j].TileType = Enum.GetName(typeof(TileType), network.util.enums.TileType.FLAT);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// this method places spice by chance on the specified fields and its neighbours
        /// </summary>
        /// <param name="indexX">the x coordinate</param>
        /// <param name="indexZ">the z coordinate</param>
        public void placeSpiceOnFields(int indexX, int indexZ)
        {
            //TODO determine count neigthbors to break loop if necessary
            Random random = new Random();
            int amountOfSpice = random.Next(3, 6);
            Console.WriteLine("random number: " + amountOfSpice);
            while (amountOfSpice > 0)
            {
                for (int i = indexX - 1; i < indexX + 2; i++)
                {
                    for (int j = indexZ - 1; j < indexZ + 2; j++)
                    {
                        if (i < mapFields.GetLength(0) && i >= 0 && j < mapFields.GetLength(1) && j >= 0)
                        {

                            if (random.NextDouble() > 0.5)
                            {
                                if (/*!(parent.Map[i, j].HasSpice) &&*/ amountOfSpice > 0)
                                {
                                    mapFields[i, j].HasSpice = true;
                                    amountOfSpice--;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method determines weather the SpiceBlow is applicable
        /// </summary>
        /// <returns>true, if the SpiceBlow should be done</returns>
        public bool SpiceBlowIsApplicable(int spiceMinimum, int currentSpice)
        {
            if (spiceMinimum > currentSpice)
            {
                return true;
            }
            return false;
        }
    }
}
