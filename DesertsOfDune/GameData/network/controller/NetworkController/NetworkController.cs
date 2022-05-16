using System;
using GameData.network.messages;
using GameData.network.util.enums;

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

        abstract public bool HandleReceivedMessage(string message);
    }
}
