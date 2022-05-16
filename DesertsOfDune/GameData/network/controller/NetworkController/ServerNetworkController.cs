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

        public override bool HandleReceivedMessage(string message)
        {
            // get Message-object from message
            Message receivedMessage = MessageConverter.ToMessage(message);

            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), receivedMessage.getMessageType());

            switch (type)
            {
                case MessageType.DEBUG:
                    messageController.OnDebugMessage((DebugMessage)receivedMessage);
                    return true;

                default:
                    Console.WriteLine("Schade, hat nicht geklappt");
                    break;
            }

            return false;
        }

        public override bool HandleSendingMessage(Message message)
        {
            // parsing the message
            string parsedMessage = MessageConverter.FromMessage(message);

            //TODO: do not Broadcast every message, but check, whether it must be send to one single client
            ((ServerConnectionHandler)base.connectionHandler).serviceManager.Broadcast(parsedMessage);

            return true;
        }
    }
}
