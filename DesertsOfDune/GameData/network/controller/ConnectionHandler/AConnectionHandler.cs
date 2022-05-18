using System;

using WebSocketSharp;

namespace GameData.network.controller
{
    /// <summary>
    /// Base class for the connection handler, which implement the websockts and handle all the connection related tasks
    /// </summary>
    /// <remarks>
    /// Therefore this class stores all information (adress, port, protocal) needed to identify the connection endpoint to connect to or to offer
    /// and provide this url.
    /// Furthermore it force the inheriting classes to implement callbacks for the different events, which can occur when using websockts:
    /// <list type="bullet">
    /// <item>opening a connection</item>
    /// <item>closing a connection</item>
    /// <item>receiving a message via the network</item>
    /// <item>notice an error</item>
    /// </list>
    /// </remarks>
    public abstract class AConnectionHandler
    {
        /// <summary>
        /// the hostname or the IPv4 address of the websocket (server)
        /// <example>
        /// SERVER_ADDRESS = "127.0.0.1"
        /// </example>
        /// </summary>
        private string SERVER_ADDRESS { get; }
        private int PORT { get; }
        private readonly string PROTOCOLL_SUFFIX = "ws://";

        /// <summary>
        /// reference to the network controller for forwarding received messages
        /// </summary>
        public NetworkController NetworkController { get; set; }

        /// <summary>
        /// sets the server address and the port of this websocket (server)
        /// </summary>
        /// <param name="ServerAddress">server adress as Hostname or IPv4</param>
        /// <param name="Port">the Port, which will be used for this connection</param>
        protected AConnectionHandler(string ServerAddress, int Port)
        {
            SERVER_ADDRESS = ServerAddress;
            PORT = Port;
        }

        /// <summary>
        /// builds the url from its compents
        /// </summary>
        /// <returns>the url of the connection as a string</returns>
        protected String GetURL()
        {
            return this.PROTOCOLL_SUFFIX + this.SERVER_ADDRESS + ":" + this.PORT;
        }

        abstract protected internal void OnOpen(String addressConnected, String sessionID);

        abstract protected internal void OnClose(CloseEventArgs e, String sessionID);

        abstract protected internal void OnError(ErrorEventArgs e, String sessionID);

        abstract protected internal void OnMessage(MessageEventArgs e, String sessionID);
    }
     
}