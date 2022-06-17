using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
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

            // send map change message because of ENDGAME
            Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.ENDGAME);

            // despawn the usual sandworm
            if (Sandworm.GetSandworm() != null)
            {
                Sandworm.Despawn(Party.GetInstance().messageController);
            }
            _shaiHuludPhase = new ShaiHuludPhase(map);
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
