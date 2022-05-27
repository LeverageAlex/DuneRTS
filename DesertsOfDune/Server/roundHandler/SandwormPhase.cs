using GameData.network.util.world;
using Server.roundHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This class defines the SandwormPhase for extending classes.
    /// </summary>
    public abstract class SandwormPhase : IGamePhase
    {
        private bool overLenght;
        private GameData.gameObjects.RoundHandler parent;

        public List<MapField> MoveSandWorm()
        {
            return null;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
