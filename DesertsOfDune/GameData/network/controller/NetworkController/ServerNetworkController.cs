using System;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.parser;
using Serilog;
using Serilog.Context;

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

        // TODO: check, if message could be send (especially to a certain client)

        /// <summary>
        /// handles messages, that need to be send and send them to all corresponding clients
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
                // logging the sending message
                using (LogContext.PushProperty("Direction", "send"))
                {
                    // Log.Information("{ message: " + parsedMessage + " }, \"direction\": \"send\" } \n");
                }

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

        /// <summary>
        /// handles messages, that need to be send to a certain client
        /// </summary>
        /// <param name="message">message, which should be send</param>
        /// <param name="clientID">the session ID of the user, who should receive this message</param>
        /// <returns></returns>
        public override bool HandleSendingMessage(Message message, string clientID)
        {
            // parsing the message
            string parsedMessage = MessageConverter.FromMessage(message);

            // check, whether the parsing was successful
            if (parsedMessage != null)
            {
                // logging the sending message
                using (LogContext.PushProperty("Direction", "send"))
                {
                    // Log.Information("{ message: " + parsedMessage + " }, \"direction\": \"send\" } \n");
                }

                // broadcast parsed message to all active sessions so clients
                ((ServerConnectionHandler)connectionHandler).sessionManager.SendTo(parsedMessage, clientID);
                return true;
            }
            else
            {
                Log.Warning("Could not send message " + message.ToString() + " from " + webSocketType.ToString() + " to " + clientID + " because it could not be converted to a JSON-String");
                return false;
            }
        }

        /// <summary>
        /// template method for handling the received messages for the server and forward them to message controller
        /// </summary>
        /// <param name="message">message, which was received</param>
        /// <param name="sessionID">ID of the session.</param>
        /// <returns>true, if the message could successfully parsed and forwarded to the message controller</returns>
        public override bool HandleReceivedMessage(string message, string sessionID)
        {
            Log.Debug("Server parsing the received message: " + message);

            // get Message-Object from JSON-String message
            Message receivedMessage = MessageConverter.ToMessage(message);

            // logging the received message
            using (LogContext.PushProperty("Direction", "receive"))
            {
                Log.Information("{ message: " + message + " }, \"direction\": \"receive\" } \n");
            }

            // get the type of the message for determine the controller methods needed to handle this message
            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), receivedMessage.GetMessageTypeAsString());
            if (GamePaused)
            {
                if (type == MessageType.PAUSE_REQUEST)
                {
                    messageController.OnPauseGameRequestMessage((PauseGameRequestMessage)receivedMessage, sessionID);
                }
                else
                {
                    Log.Warning($"Cannot process message ({message}), because the game is paused, so only pause request messages are received!");
                }
            }
            else
            {

                switch (type)
                {
                    case MessageType.DEBUG:
                        messageController.OnDebugMessage((DebugMessage)receivedMessage);
                        return true;
                    case MessageType.JOIN:
                        messageController.OnJoinMessage((JoinMessage)receivedMessage, sessionID);
                        return true;
                    case MessageType.REJOIN:
                        messageController.OnRejoinMessage((RejoinMessage)receivedMessage, sessionID);
                        return true;
                    case MessageType.HOUSE_REQUEST:
                        messageController.OnHouseRequestMessage((HouseRequestMessage)receivedMessage, sessionID);
                        return true;
                    case MessageType.MOVEMENT_REQUEST:
                        messageController.OnMovementRequestMessage((MovementRequestMessage)receivedMessage);
                        return true;
                    case MessageType.ACTION_REQUEST:
                        messageController.OnActionRequestMessage((ActionRequestMessage)receivedMessage);
                        return true;
                    case MessageType.TRANSFER_REQUEST:
                        messageController.OnTransferRequestMessage((TransferRequestMessage)receivedMessage);
                        return true;
                    case MessageType.END_TURN_REQUEST:
                        messageController.OnEndTurnRequestMessage((EndTurnRequestMessage)receivedMessage);
                        return true;
                    case MessageType.GAMESTATE_REQUEST:
                        messageController.OnGameStateRequestMessage((GameStateRequestMessage)receivedMessage);
                        return true;
                    case MessageType.PAUSE_REQUEST:

                        messageController.OnPauseGameRequestMessage((PauseGameRequestMessage)receivedMessage, sessionID);
                        return true;
                    case MessageType.HELI_REQUEST:
                        messageController.OnHeliRequestMessage((HeliRequestMessage)receivedMessage);
                        return true;
                    default:
                        Log.Error("Incoming parsed message has invalid type (" + type + ") and could not be forwarded to the message controller!");
                        break;
                }
            }
            return false;
        }
    }
}
