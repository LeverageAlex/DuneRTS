using System;

using GameData.network.controller;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MessageController messageController = new MessageController();
            MessageController messageController2 = new MessageController();
            Console.WriteLine("Created messageController");

            ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler("127.0.0.1", 7890);
            Console.WriteLine("Created serverconnectionhandler");
            ClientConnectionHandler clientConnectionHandler = new ClientConnectionHandler("127.0.0.1", 7890);
            Console.WriteLine("Created clientconnectionhandler");

            ServerNetworkController serverNetworkController = new ServerNetworkController(serverConnectionHandler, messageController);
            ClientNetworkController clientNetworkController = new ClientNetworkController(clientConnectionHandler, messageController2);


            // server
            messageController.DoDebug("Hallo hier ist Günther");

            // client

            messageController2.DoDebug("Hallo emil, hier ist Alex");

            // server
            messageController.DoDebug("Alex ist sehr fleißig");
        }
    }
}
