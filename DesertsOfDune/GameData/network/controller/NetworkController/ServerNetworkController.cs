using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using Serilog;

namespace GameData.network.controller
{
    /// <summary>
    /// Network controller for the (websocket) server, which handles incoming and outgoing messages
    /// </summary>
    /// <remarks>
    /// This concrete network controller implementation inherit the abstract NetworkController (<see cref="NetworkController"/>).
    /// Thats why it needs a reference to the connection handler (<see cref="ServerConnectionHandler"/>) and message controller (<see cref="MessageController"/>) used
    /// and implement the "HandlingSendingMessage"-method.
    /// </remarks>
    public class ServerNetworkController : NetworkController
    {
        /// <summary>
        /// creates a server network controller instance
        /// </summary>
        /// <remarks>
        /// sets the reference to the server connection handler and message controller by setting there NetworkController to the "this" reference
        /// </remarks>
        /// <param name="connectionHandler">reference to the server connection handler used to send and receive messages</param>
        /// <param name="messageController">reference to the message controller used handle the messages received and to send</param>
        public ServerNetworkController(ServerConnectionHandler connectionHandler, MessageController messageController) : base(connectionHandler, messageController, WebSocketType.WEBSOCKET_SERVER)
        {
            base.messageController.NetworkController = this;
            base.connectionHandler.NetworkController = this;
        }

        /// <summary>
        /// handles messages, that need to be send and send them to all corresponding clients
        /// </summary>
        /// <param name="message">message, which should be send</param>
        /// <returns>true, if the message could successfully send</returns>
        /// TODO: do not broadcast every message, but check, whether is needed to be send to a certain client and send it only to this client
        public override bool HandleSendingMessage(Message message)
        {
            // parsing the message
            string parsedMessage = MessageConverter.FromMessage(message);

            // check, whether the parsing was successful
            if (parsedMessage != null)
            {
                // broadcast parsed message to all active sessions so clients
                ((ServerConnectionHandler)connectionHandler).sessionManager.Broadcast(parsedMessage);
                return true;
            }
            else
            {
                Log.Warning("Could not send message " + message.ToString() + " from " + webSocketType.ToString() + " because it could not be converted to a JSON-String");
                return false;
            }
        }
    }
}
