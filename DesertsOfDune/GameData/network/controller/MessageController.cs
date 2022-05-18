using System;
using GameData.network.messages;
using GameData.network.util.parser;
using GameData.server;
using Server.commandLineParser;

namespace GameData.network.controller
{
    public class MessageController
    {
        public NetworkController controller { get; set; }

        public MessageController()
        {
        }

        public void OnDebugMessage(EndTurnRequestMessage msg)
        {
            string messageContent = MessageConverter.FromMessage(msg);
            Console.WriteLine("Received debug message is: " + messageContent);
            DoDebug("Ich antworte hiermit auf " + msg);
            Console.WriteLine("Send answer");
        }

        public void DoDebug(string explanation)
        {
            // create Debug message
            //EndTurnRequestMessage message = new EndTurnRequestMessage(123, 1234);

            // send message
            //controller.HandleSendingMessage(message);

            CommandLineParser.ParseArgument("-- name");

        }

        
    }
}
