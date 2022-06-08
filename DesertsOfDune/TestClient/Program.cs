using System;
using System.Threading;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util;
using Server;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);

            Console.WriteLine("Hello World 2!");

            MessageController messageController2 = new ServerMessageController();

            ClientConnectionHandler clientConnectionHandler = new ClientConnectionHandler("127.0.0.1", 10101);

            ClientNetworkController clientNetworkController = new ClientNetworkController(clientConnectionHandler, messageController2);

            // client

            Logger logger = new Logger();
            logger.CreateDebugLogger();

            messageController2.DoDebug(123, "Hallo emil, hier ist Alex");

            clientNetworkController.HandleSendingMessage(new JoinMessage("client", true, false));
        }
    }
}
