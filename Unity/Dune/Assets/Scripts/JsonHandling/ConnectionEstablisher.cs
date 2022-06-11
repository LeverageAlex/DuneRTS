using System;
using GameData.network.controller;
using GameData.network.util;
using Serilog;

public static class ConnectionEstablisher
{

        public static PlayerMessageController messageController;


        public static void CreateNetworkModule(String ip, int port)
        {
            messageController = new PlayerMessageController();

            ClientConnectionHandler clientconhandler = new ClientConnectionHandler(ip, port);
            _ = new ClientNetworkController(clientconhandler, messageController);
        }
      
    



}
