using System;
using System.Threading;
using GameData.network.controller;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);

            Console.WriteLine("Hello World 2!");

            MessageController messageController2 = new MessageController();

            ClientConnectionHandler clientConnectionHandler = new ClientConnectionHandler("127.0.0.1", 10101);

            ClientNetworkController clientNetworkController = new ClientNetworkController(clientConnectionHandler, messageController2);

            // client

            messageController2.DoDebug(123, "Hallo emil, hier ist Alex");
        }
    }
}
