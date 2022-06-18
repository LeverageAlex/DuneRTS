using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using Serilog;

namespace GameData.network.controller
{
    /// <summary>
    /// Base class for the network controller (in the clients and server).
    /// The network controller manage all receiving and sending of messages to the recipient
    /// </summary>
    /// <remarks>
    /// The network controller is used for handling the received and messages and the ones, which need to be send.
    /// Therefore is has two methods and handling the messages and hold the references to the connection handler and the message controller.
    /// </remarks>
    public abstract class NetworkController
    {
        /// <summary>
        /// reference to the (concrete implemented) connection handler used by this network controller (and in general network module)
        /// The connection handler is used for forwaring the messages to send to the correct websocket "endpoint"
        /// *HINT* this connection handler is read only and cannot be changed later
        /// </summary>
        public AConnectionHandler connectionHandler { get; }
        /// <summary>
        /// reference to the (concrete implemented) message controller used by this network controller.
        /// The message controller is used for forwarding the incoming messages to the correct methods. 
        /// *HINT* this message controller is read only and cannot be changed later
        /// </summary>
        protected MessageController messageController { get; }
        protected readonly WebSocketType webSocketType;

        public bool GamePaused { get; set; }

        /// <summary>
        /// sets the references needed by the network controller
        /// </summary>
        /// <param name="connectionHandler"></param>
        /// <param name="messageController"></param>
        /// <param name="webSocketType"></param>
        protected NetworkController(AConnectionHandler connectionHandler, MessageController messageController, WebSocketType webSocketType)
        {
            this.connectionHandler = connectionHandler;
            this.messageController = messageController;
            this.webSocketType = webSocketType;
        }

        /// <summary>
        /// template method for handling sending messages and forward them to the connection handler
        /// </summary>
        /// <param name="message">message, which should be send</param>
        /// <returns>true, if the messages could be sucessfully forwareded to the connection handler and send</returns>
        abstract public bool HandleSendingMessage(Message message);

        abstract public bool HandleSendingMessage(Message message, string clientID);

        /// <summary>
        /// template method for handling the received messages and forward them to message controller
        /// </summary>
        /// <param name="message">message, which was received</param>
        /// <param name="sessionID">ID of the session.</param>
        /// <returns>true, if the message could successfully parsed and forwarded to the message controller</returns>
        public abstract bool HandleReceivedMessage(string message, string sessionID);
    }
}
