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

        public void OnDebugMessage(EndTurnRequestMessage msg)
        {
            string messageContent = MessageConverter.FromMessage(msg);
            Console.WriteLine("Received debug message is: " + messageContent);
        }

        public void DoDebug(string explanation)
        {
            // create Debug message
            EndTurnRequestMessage message = new EndTurnRequestMessage(123, 1234);

            // send message
            controller.HandleSendingMessage(message);

        }

        
    }
}
