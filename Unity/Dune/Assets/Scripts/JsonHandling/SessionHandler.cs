using System;
using GameData.network.controller;
using GameData.network.util;
using Serilog;

public static class SessionHandler
{

    public static PlayerMessageController messageController;
    public static int clientId;
    public static int enemyClientId;
    public static string clientSecret;
    public static bool isPlayer = false;
    public static int viewerId;

        public static void CreateNetworkModule(String ip, int port)
        {

        Logger logger = new Logger();
        Log.Logger = new LoggerConfiguration().WriteTo.File("debugLogs.log").MinimumLevel.Debug().CreateLogger();
        messageController = new PlayerMessageController();

            ClientConnectionHandler clientconhandler = new ClientConnectionHandler(ip, port);
            _ = new ClientNetworkController(clientconhandler, messageController);


    }
      
    



}
