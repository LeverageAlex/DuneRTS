using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using Serilog;

namespace GameData.network.controller
{
    /// <summary>
    /// Network controller for the (websocket) client, which handles incoming and outgoing messages
    /// </summary>
    /// <remarks>
    /// This concrete network controller implementation inherit the abstract NetworkController (<see cref="NetworkController"/>).
    /// Thats why it needs a reference to the connection handler (<see cref="ClientConnectionHandler"/>) and message controller (<see cref="MessageController"/>) used
    /// and implement the "HandlingSendingMessage"-method.
    /// </remarks>
    public class ClientNetworkController : NetworkController
    {
        /// <summary>
        /// creates a client network controller instance
        /// </summary>
        /// <remarks>
        /// sets the reference to the client connection handler and message controller by setting there NetworkController to the "this" reference
        /// </remarks>
        /// <param name="connectionHandler">reference to the client connection handler used to send and receive messages</param>
        /// <param name="messageController">reference to the message controller used handle the messages received and to send</param>
        public ClientNetworkController(ClientConnectionHandler connectionHandler, MessageController messageController) : base(connectionHandler, messageController, WebSocketType.WEBSOCKET_CLIENT)
        {
            base.messageController.NetworkController = this;
            base.connectionHandler.NetworkController = this;
        }

        /// <summary>
        /// handles messages, that need to be send and send them to the connected websocket server
        /// </summary>
        /// <param name="message">message, which should be send</param>
        /// <returns>true, if the message could successfully send</returns>
        public override bool HandleSendingMessage(Message message)
        {
            // parsing the message
            string parsedMessage = MessageConverter.FromMessage(message);

            // check, whether the parsing was successful
            if (parsedMessage != null)
            {
                // broadcast parsed message to all active sessions so clients
                ((ClientConnectionHandler)connectionHandler).WebSocket.Send(parsedMessage);
                return true;
            }
            else
            {
                Log.Warning("Could not send message " + message.ToString() + " from " + webSocketType.ToString() + " because it could not be converted to a JSON-String");
                return false;
            }
        }

        /// <summary>
        /// implemententation not necessary, because the client can only send to one server
        /// </summary>
        public override bool HandleSendingMessage(Message message, string clientID)
        {
            throw new NotImplementedException();
        }
    }
}
