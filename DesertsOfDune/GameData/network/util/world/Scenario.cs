using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world
{
    /// <summary>
    /// This Class holds information about the playingfield
    /// </summary>
    public class Scenario
    {
        private int width;
        private int height;
        private List<MapField> map;
        private static Scenario instance;

        /// <summary>
        /// private constructor of class Scenario to support singleton patter
        /// </summary>
        /// <param name="width">the width of the playingfield</param>
        /// <param name="height">the height of the playingfield</param>
        private Scenario(int width, int height)
        {
            instance.width = width;
            instance.height = height;
        }

        /// <summary>
        /// this method returns the singleton instance of the class Scenario
        /// </summary>
        /// <returns>the instance</returns>
        public static Scenario GetInstance(int width, int height)
        {
            if (instance == null)
            {
                instance = new Scenario(width,height);
                return instance;
            }
            return instance;
        }
    }
}
