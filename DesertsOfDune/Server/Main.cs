using System;

using GameData.network.controller;
using GameData.network.util;
using Serilog;

namespace Server
{
    static class Programm {

        /// <summary>
        /// main method, which is executed when the server was started / executed
        /// It will initialize all classes and trigger all starting events including
        /// <list type="bullet">
        /// <item>create and configure network module</item>
        /// <item>load configuration files and configure everything</item>
        /// <item>create and configure logger</item>
        /// </list>
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            InitiliazeLogger();
            Log.Information("Starting server... Everything will be created and configured.");

            MessageController messageController = new MessageController();
            
            ServerConnectionHandler serverConnectionHandler = new ServerConnectionHandler("127.0.0.1", 7890);

            ServerNetworkController serverNetworkController = new ServerNetworkController(serverConnectionHandler, messageController);

            // server
            messageController.DoDebug(113, "Hallo hier ist Günther");


            // server
            // messageController.DoDebug("Alex ist sehr fleißig");
        }

        /// <summary>
        /// creates and configures a new logger for the server
        /// </summary>
        private static void InitiliazeLogger()
        {
            Logger.CreateDefaultLogger();
        }
    }
}
