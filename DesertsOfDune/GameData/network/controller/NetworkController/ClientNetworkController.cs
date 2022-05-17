using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;

namespace GameData.network.controller
{
    public class ClientNetworkController : NetworkController
    {
        public ClientNetworkController(ClientConnectionHandler connectionHandler, MessageController messageController) : base(connectionHandler, messageController, WebSocketType.WEBSOCKET_CLIENT)
        {
            connectionHandler.networkController = this;
            messageController.controller = this;
        }

        public override bool HandleSendingMessage(Message message)
        {
            // parsing message
            string parsedMessage = MessageConverter.FromMessage(message);
            ((ClientConnectionHandler)connectionHandler).webSocket.Send(parsedMessage);

            Console.WriteLine("Send: " + parsedMessage);

            return true;
        }
    }
}
