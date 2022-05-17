using System;
using System.Collections.Generic;
using GameData.network.messages;
using GameData.network.util.parser;
using GameData.network.util.world;

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
            //EndTurnRequestMessage message = new EndTurnRequestMessage(123, 1234);
            //  MapField mapfield = new MapField(true, true, 12, new Position(1, 2));
            //MapField[][] arr = new MapField[1][];
            //arr[0][0] = mapfield;
            // MapChangeMessage message = new MapChangeMessage(new MapChangeReasons(), null);
            MapField[][] a = new MapField[2][];
            MapChangeDemandMessage message = new MapChangeDemandMessage(new MapChangeReasons(), a);

            // send message
            controller.HandleSendingMessage(message);

        }

        
    }
}
