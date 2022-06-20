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
        if(Input.GetKeyDown(KeyCode.C))
        {
            SessionHandler.CloseNetworkModule();
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
        if (!SessionHandler.clientconhandler.ConnectionIsAlive())
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
