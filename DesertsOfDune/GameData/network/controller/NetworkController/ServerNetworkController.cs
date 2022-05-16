using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using static GameData.network.messages.Enums;

namespace GameData.network.controller
{
    public class ServerNetworkController : NetworkController
    {
        public ServerNetworkController(ServerConnectionHandler connectionHandler, MessageController messageController) : base(connectionHandler, messageController, WebSocketType.WEBSOCKET_SERVER)
        {
            base.messageController.controller = this;
            base.connectionHandler.networkController = this;
        }

        public override bool HandleSendingMessage(Message message)
        {
            // parsing the message
            string parsedMessage = MessageConverter.FromMessage(message);

            //TODO: do not Broadcast every message, but check, whether it must be send to one single client
            Console.WriteLine("Broadcast message from server");
            ((ServerConnectionHandler)connectionHandler).serviceManager.Broadcast(parsedMessage);

            return true;
        }
    }
}
