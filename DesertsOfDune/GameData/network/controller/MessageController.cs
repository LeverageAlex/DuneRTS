using System;
using GameData.network.messages;
using GameData.network.util.parser;

namespace GameData.network.controller
{
    public class MessageController
    {
        public NetworkController controller { get; set; }

        public MessageController()
        {
        }

        public void OnDebugMessage(DebugMessage msg)
        {
            string messageContent = MessageConverter.FromMessage(msg);
            Console.WriteLine("Received debug message is: " + messageContent);
        }

        public void DoDebug(string explanation)
        {
            // create Debug message
            DebugMessage message = new DebugMessage(explanation);

            // send message
            controller.HandleSendingMessage(message);

        }
    }
}
