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
            

            ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler("127.0.0.1", 7890);

            ServerNetworkController serverNetworkController = new ServerNetworkController(serverConnectionHandler, messageController);

            // server
            messageController.DoDebug(113, "Hallo hier ist Günther");


            // server
            // messageController.DoDebug("Alex ist sehr fleißig");
        }
    }
}
