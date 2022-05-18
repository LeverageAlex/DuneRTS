using System;
using GameData.network.messages;
using Serilog;
using GameData.network.util.parser;
using Server.commandLineParser;
using GameData.network.util.world;

namespace GameData.network.controller
{
    /// <summary>
    /// Base class of all message controllers
    /// </summary>
    /// <remarks>
    /// The message controller implement the minimal functionality of any message controller.
    /// In this case, every message controller must deal with debug messages. \n
    /// In general the purpose of a message controller is to create the messages from the "context"
    /// and provide callbacks, that execute code and interact with the "context", if a certain message is received.
    /// So the message controller is kind of the interface from network (and outside world) with the inner "context", which
    /// is e. g. the server or the spectator client. \n
    /// Therefore is has do<Message>() and on<Message> methods for every message type, the context need to implement.
    /// The do-Methods always get information and create a message from this information and tell the network controller to send it
    /// and the on-Methods are always triggered by the network controller (respectively connection handler) and parse as well as
    /// process a message and so effect the "context". 
    /// </remarks>
    public class MessageController
    {
        /// <summary>
        /// parent network controller, that contains this message controller (ref. needed so give the message, which need
        /// to be send, to the fitting network controller)
        /// </summary>
        public NetworkController NetworkController { get; set; }

        /// <summary>
        /// creates a new message controller
        /// </summary>
        public MessageController()
        {
        }

        /// <summary>
        /// processes a debug message, so log this information (to console)
        /// </summary>
        /// <remarks>
        /// is triggered, if the network controller receives a message of the type "DEBUG" and forward it to this callback
        /// </remarks>
        /// <param name="message">the incoming debug message from network</param>
        /// TODO: check, what the purpose of the debug message is and adjust the implementation
        public void OnDebugMessage(DebugMessage message)
        {
            // extract the information from the debug message
            int code = message.code;
            string explanation = message.explanation;

            // log the information to console
            Log.Debug("Received debug message with code: " + code + " because of: " + explanation);
        }

        /// <summary>
        /// create debug message and trigger the network controller to send this message
        /// </summary>
        /// <param name="code">code number for categorization of the event</param>
        /// <param name="explanation">further explanation of the event / code number</param>
        /// <example>
        /// For instance, the variables could have the following values for describing a debug message
        /// <code>
        /// code = 404
        /// explanation = service is not avaible
        /// </code>
        /// </example>
        public void DoDebug(int code, string explanation)
        {
            // create Debug message
            DebugMessage message = new DebugMessage(code, explanation);

            // send message
            NetworkController.HandleSendingMessage(message);
        }
    }
}
