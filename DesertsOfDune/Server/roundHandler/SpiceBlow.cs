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
        private RoundHandler parent;

        public SpiceBlow(RoundHandler parent)
        {
            this.parent = parent;
        }
        /// <summary>
        /// This method does the SpiceBlow.
        /// </summary>
        /// <returns>true, if the spiceblow was possible</returns>
        public bool RandomSpiceBlow()
        {
            if (SpiceBlowIsApplicable())
            {
                int indexX;
                int indexZ;
                while (true)
                {
                    indexX = ChoosRandomMapFieldIndxX();
                    indexZ = ChoosRandomMapFieldIndxZ();
                    if (parent.Map[indexX, indexZ].TileType ==  "DUNE")
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
        private int ChoosRandomMapFieldIndxX()
        {
            int mapLengthX = parent.Map.GetLength(0);
            Random random = new Random();
            return random.Next(0, mapLengthX-1);

        }

        /// <summary>
        /// This method chooses one random z-coordinate on the map
        /// </summary>
        /// <returns>the coordinate on the map</returns>
        private int ChoosRandomMapFieldIndxZ()
        {
            int mapLengthZ = parent.Map.GetLength(1);
            Random random = new Random();
            return random.Next(0, mapLengthZ-1);
        }

        /// <summary>
        /// This method changes the selected Field and its neightbors to Flatsand/Dune by chance
        /// </summary>
        /// <param name="indexX">the x-coordinate of the selcted field</param>
        /// <param name="indexZ">the z-coordinate of the selected field</param>
        private void ChangeFieldAndNeighborsRandomly(int indexX, int indexZ)
        {
            for(int i = indexX-1; i < indexX+1; i++)
            {
                for(int j = indexZ-1; j < indexZ+1; j++)
                {
                    if (parent.Map[i,j] != null)
                    {
                        Random random = new Random();
                        if (random.NextDouble() > 0.5)
                        {
                            parent.Map[i, j].TileType = Enum.GetName(typeof(TileType), network.util.enums.TileType.DUNE);
                        } else
                        {
                            parent.Map[i, j].TileType = Enum.GetName(typeof(TileType), network.util.enums.TileType.FLAT);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// this method places spice by chance on the specified fields and ist
        /// </summary>
        /// <param name="indexX"></param>
        /// <param name="indexZ"></param>
        private void placeSpiceOnFields(int indexX, int indexZ)
        {
            Random random = new Random();
            int amountOfSpice = random.Next(3, 6);
            for (int i = indexX - 1; i < indexX + 1; i++)
            {
                for (int j = indexZ - 1; j < indexZ + 1; j++)
                {
                    if (random.NextDouble() > 0.5)
                    {
                        if(! (parent.Map[i,j].HasSpice) && amountOfSpice > 0)
                        {
                            parent.Map[i, j].HasSpice = true;
                            amountOfSpice--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method determines weather the SpiceBlow is applicable
        /// </summary>
        /// <returns>true, if the SpiceBlow should be done</returns>
        public bool SpiceBlowIsApplicable()
        {
            if (parent.SpiceMinimum > parent.CurrentSpice)
            {
                return true;
            }
            return false;
        }
    }
}
