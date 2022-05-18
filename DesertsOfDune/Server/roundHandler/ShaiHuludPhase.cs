using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// In this Phase the overlength Shaihulud actions are handled.
    /// </summary>
    public class ShaiHuludPhase : OverLengthMechanism
    {
        // TODO implment character class
        //private Character targetCharacter;

        /// <summary>
        /// This method uses 
        /// </summary>
        /// <returns></returns>
        public bool ChooseTargetCharacter()
        {
            // todo: implement logic
            return false;
        }

        /// <summary>
        /// This method handles how a character should be eaten
        /// </summary>
        /// <returns>true, if a character was eaten.</returns>
        public bool EatTargetCharacter()
        {
            // todo: implement logic
            return false;
        }
    }
}
