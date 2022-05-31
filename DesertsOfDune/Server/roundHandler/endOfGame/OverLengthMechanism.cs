using System;
using System.Collections.Generic;
using System.Text;
using GameData.server.roundHandler;
using Server.roundHandler;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents and executes the overlength mechanism
    /// </summary>
    public class OverLengthMechanism : IGamePhase
    {
        private readonly Map _map;
        private readonly ShaiHuludPhase _shaiHuludPhase;

        public OverLengthMechanism(Map map)
        {
            this._map = map;

            // execute the earthquake
            EarthQuakeExecutor earthQuakeExecutor = new EarthQuakeExecutor(map);
            earthQuakeExecutor.TransformRockPlanes();
        }



        /// <summary>
        /// execute the overlength mechanism
        /// </summary>
        public void Execute()
        {
            _shaiHuludPhase.Execute();
        }
    }
}
