using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class defines the SandwormPhase for extending classes.
    /// </summary>
    public abstract class SandwormPhase : GamePhase
    {
        private bool overLenght;
        private GameData.gameObjects.RoundHandler parent;

        public void MoveSandWorm()
        {

        }

        public void Execut()
        {
            throw new NotImplementedException();
        }
    }
}
