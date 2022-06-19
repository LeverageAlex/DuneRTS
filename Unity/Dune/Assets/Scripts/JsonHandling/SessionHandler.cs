using System;
using System.Threading;
using GameData.network.controller;
using GameData.network.util;
using Serilog;

public static class SessionHandler
{

    public static PlayerMessageController messageController;
    public static int clientId;
    public static int enemyClientId;
    public static string clientSecret;
    public static bool isPlayer = true;
    public static int viewerId;
    public static int atomicsLeft;

    private static ClientConnectionHandler clientconhandler;

        public static void CreateNetworkModule(String ip, int port)
        {
        if (messageController == null)
        {
            //   if (logger == null)
            // {
            Logger logger = new Logger();
            Log.Logger = new LoggerConfiguration().WriteTo.File("debugLogs.log").MinimumLevel.Debug().CreateLogger();
            //   }

            messageController = new PlayerMessageController();

            clientconhandler = new ClientConnectionHandler(ip, port);
            _ = new ClientNetworkController(clientconhandler, messageController);
            GameData.network.util.world.GreatHouse.determineCharacters = false;
            Log.Debug("Logged Information.");
        }
        else
        {
            clientconhandler.ReconnectSocket(ip, port);
            Thread.Sleep(250);
        }
    
    }

    public static void CloseNetworkModule()
    {
        clientconhandler.CloseConnectionToWebsocketServer();
        Thread.Sleep(100);

    }
      
    



}
