using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using static GameData.network.messages.Enums;

namespace GameData.network.controller
{
    public abstract class NetworkController
    {
        protected AConnectionHandler connectionHandler { get; }
        protected MessageController messageController { get; }
        protected readonly WebSocketType webSocketType;

        public NetworkController(AConnectionHandler connectionHandler, MessageController messageController, WebSocketType webSocketType)
        {
            this.connectionHandler = connectionHandler;
            this.messageController = messageController;
            this.webSocketType = webSocketType;
        }

        abstract public bool HandleSendingMessage(Message message);

        public bool HandleReceivedMessage(string message)
        {
            // get Message - object from message
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
    }
}
