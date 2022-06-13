using System;
using System.Collections.Generic;
using System.Text;
using GameData.server.roundHandler;
using Server;
using Server.roundHandler;

namespace GameData.network.util.world
{
    /// <summary>
    /// Represents and executes the overlength mechanism
    /// </summary>
    public class OverLengthMechanism
    {
        private readonly Map _map;
        private readonly ShaiHuludPhase _shaiHuludPhase;

        /// <summary>
        /// create a new handler for the overlength mechanism
        /// </summary>
        /// <param name="map">the map, the overlength mechanism is working on</param>
        public OverLengthMechanism(Map map)
        {
            this._map = map;

            // execute the earthquake
            EarthQuakeExecutor earthQuakeExecutor = new EarthQuakeExecutor(map);
            earthQuakeExecutor.TransformRockPlanes();

            // despawn the usual sandworm
            Sandworm.Despawn(Party.GetInstance().messageController);
        }



        /// <summary>
        /// execute the overlength mechanism
        /// </summary>
        /// <returns>true, if the overlength mechanism is over</returns>
        public bool Execute()
        {
            return _shaiHuludPhase.Execute();
        }
    }
}
