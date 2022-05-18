using System;

using WebSocketSharp;
using WebSocketSharp.Server;

using Serilog;
using System.Threading;

namespace GameData.network.controller
{
    /// <summary>
    /// Service, which is used to communicate the game messages between the server and clients in the game
    /// </summary>
    public class GameService : WebSocketBehavior
    {
        
        private readonly ServerConnectionHandler _connectionHandler;

        public GameService(ServerConnectionHandler _serverConnectionHandler)
        {
            _connectionHandler = _serverConnectionHandler;
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _connectionHandler.OnClose(e, this.ID);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            _connectionHandler.OnError(e, this.ID);
        }

        protected override void OnMessage(MessageEventArgs e)
        { 
            _connectionHandler.OnMessage(e, this.ID);
        }

        protected override void OnOpen()
        {
            _connectionHandler.OnOpen(Context.UserEndPoint.ToString(), this.ID);
        }
    }

    /// <summary>
    /// connection handler for websocket server, which provides the possibility to conenct / send messages to
    /// and handles the connections to all connected (websocket) clients
    /// </summary>
    /// <remarks>
    /// This concrete connection handler implementation for the server inherit the abstract connection handler (<see cref="AConnectionHandler"/>).
    /// Therefore it needs to implement the callbacks for the different events, which can occur in a websocket connection.
    /// Furthermore this class create a new websocket server and service on creation.
    /// </remarks>
    public class ServerConnectionHandler : AConnectionHandler
    {
        /// <summary>
        /// session manager, which provide the sessions that are connected to the websocket server
        /// </summary>
        public WebSocketSessionManager sessionManager { get; set; }

        /// <summary>
        /// creates a new server connection handler and start the websocket server,
        /// which is identified by the given address
        /// </summary>
        /// <param name="ServerAddress">hostname or IPv4 address of the websocket server</param>
        /// <param name="Port">the port of the websocket server opened</param>
        public ServerConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            // initialize the websocket server (but not block the programm because to waiting for websocket server to be closed)
            Thread t = new Thread(InitializeWebSocketServer);
            t.Start();
        }

        /// <summary>
        /// create, initialize and start the websocket server
        /// </summary>
        public void InitializeWebSocketServer() {
            // initialize the websocket on the given url

            WebSocketServer webSocketServer = new WebSocketServer(GetURL());

            // add services
            webSocketServer.AddWebSocketService<GameService>("/", () => new GameService(this));

            // start the websocket server
            webSocketServer.Start();
            Log.Information("Started websocket on " + GetURL() + "/");

            // set the session mananger
            sessionManager = webSocketServer.WebSocketServices["/"].Sessions;

            // wait for the user to quit the websocket server by typing any key in the console
            // TODO: add logic, that the websocket server is closed, when the server is shut down

            Console.ReadKey();
            webSocketServer.Stop();
            Log.Warning("The websocket server was stopped!");
        }

        // TODO: finish the implementation of the callback functions

        /// <summary>
        /// callback, which is triggered when the websocket connection was closed
        /// It prints the reason. 
        /// </summary>
        /// <param name="e">close arguments which contain the reason for the closing</param>
        /// <param name="sessionID">not necessary</param>
        protected internal override void OnClose(CloseEventArgs e, String sessionID)
        {
            Log.Warning("The connection to the websocket server was closed by a client. The reason is: " + e.Reason);
        }

        /// <summary>
        /// callback, which is triggered, if an error occur on the connection to a client
        /// It prints the error
        /// </summary>
        /// <param name="e">error arguments which contain the error message</param>
        /// <param name="sessionID">not necessary</param>
        protected internal override void OnError(ErrorEventArgs e, String sessionID)
        {
            Log.Error("An error occured on the connection to " + GetURL() + ". The reason is: " + e.Message);
        }

        /// <summary>
        /// callback, which is triggered, if the websocket server notice a new message from one connected client.
        /// It forwards this message to network controller, which handles it
        /// </summary>
        /// <param name="e">message arguments, which contain the message string</param>
        /// <param name="sessionID">not necessary</param>
        protected internal override void OnMessage(MessageEventArgs e, String sessionID)
        {
            Log.Debug("Received new message from a client. The message is: " + e.Data);
            ((ServerNetworkController)NetworkController).HandleReceivedMessage(e.Data);
        }

        /// <summary>
        /// callback, which is triggered, if the a new client connected to the server
        /// It client address and its session ID
        /// </summary>
        /// <param name="addressConnected">the address of the client</param>
        /// <param name="sessionID">the session id of the new client</param>
        protected internal override void OnOpen(string addressConnected, string sessionID)
        {
            Log.Information("Registred new connection from: " + addressConnected);
            Log.Debug("The session ID of the new client is: " + sessionID);
        }
    }
}