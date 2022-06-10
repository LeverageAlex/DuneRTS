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

        /// <summary>
        /// template method for handling the received messages for the client and forward them to message controller
        /// </summary>
        /// <param name="message">message, which was received</param>
        /// <param name="sessionID">ID of the session.</param>
        /// <returns>true, if the message could successfully parsed and forwarded to the message controller</returns>
        public override bool HandleReceivedMessage(string message, string sessionID)
        {
            Log.Debug("Client parsing the received message: " + message);

            // get Message-Object from JSON-String message
            Message receivedMessage = MessageConverter.ToMessage(message);

            // get the type of the message for determine the controller methods needed to handle this message
            MessageType type = (MessageType)Enum.Parse(typeof(MessageType), receivedMessage.GetMessageTypeAsString());

            switch (type)
            {
                case MessageType.DEBUG:
                    messageController.OnDebugMessage((DebugMessage)receivedMessage);
                    return true;
                case MessageType.JOINACCEPTED:
                    messageController.OnJoinAccepted((JoinAcceptedMessage)receivedMessage);
                    return true;
                case MessageType.ERROR:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.GAMECFG:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.HOUSE_OFFER:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.HOUSE_ACKNOWLEDGEMENT:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.TURN_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.MOVEMENT_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.ACTION_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.TRANSFER_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.CHARACTER_STAT_CHANGE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.MAP_CHANGE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.ATOMICS_UPDATE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.SPAWN_CHARACTER_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.CHANGE_PLAYER_SPICE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.SANDWORM_SPAWN_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.SANDWORM_MOVE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.SANDWORM_DESPAWN_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.ENDGAME:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.GAME_END:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.GAMESTATE:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.STRIKE:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.GAME_PAUSE_DEMAND:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                case MessageType.UNPAUSE_GAME_OFFER:
                    //todo: call method from client
                    Console.WriteLine(message);
                    return true;
                default:
                    Log.Error("Incoming parsed message has invalid type (" + type + ") and could not be forwarded to the message controller!");
                    break;
            }

            return false;
        }
    }
}
