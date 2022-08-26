using GameData.network.util.world;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class StartConnectionWatchdog : MonoBehaviour
{

    Timer connectionMonitiorTimer;
    public static StartConnectionWatchdog instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        CreateConnectionMonitor();
    }

    private void Update()
    {
        /* if(Input.GetKeyDown(KeyCode.C))
         {
             SessionHandler.CloseNetworkModule();
         }*/
       
        if (Input.GetKeyDown(KeyCode.C))
        {
            SessionHandler.messageController.OnHeliDemandMessage(new GameData.network.messages.HeliDemandMessage(SessionHandler.clientId, CharacterMgr.instance.randomChar().characterId, new Position(0, 0),true));
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SessionHandler.messageController.OnHeliDemandMessage(new GameData.network.messages.HeliDemandMessage(SessionHandler.clientId, CharacterMgr.instance.randomChar().characterId, new Position(10, 10), false));
        }
    }


    public void CreateConnectionMonitor()
    {
        if (connectionMonitiorTimer == null)
        {
            connectionMonitiorTimer = new System.Timers.Timer(1000);
            connectionMonitiorTimer.Elapsed += this.ConnectionMonitorEvent;
            connectionMonitiorTimer.AutoReset = true;
            connectionMonitiorTimer.Enabled = true;
        }
    }

    public void ConnectionMonitorEvent(object source, ElapsedEventArgs e)
    {
        if (!SessionHandler.clientconhandler.ConnectionIsAlive() && !SessionHandler.endGame)
        {
            SessionHandler.CloseNetworkModule();
            connectionMonitiorTimer.Stop();
            //Log.Debug("ConnectionMonitor: Connection just died. Opening rejoin menu");
            InGameMenuManager.getInstance().DemandRejoinOption();

        }
    }

    public void RestartConnectionMonitor()
    {
        connectionMonitiorTimer.Start();
    }



}
