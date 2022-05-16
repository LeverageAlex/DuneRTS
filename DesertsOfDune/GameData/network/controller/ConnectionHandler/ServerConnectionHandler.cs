using System;

using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSharp.Net;

using Serilog;

namespace GameData.network.controller
{
    public class GameService : WebSocketBehavior
    {
        
        private ServerConnectionHandler _connectionHandler;
        public WebSocketSessionManager sessionManager { get; }

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

    public class ServerConnectionHandler : AConnectionHandler
    {
        private WebSocketServer _webSocketServer;
        public WebSocketServiceManager serviceManager { get; set; }
        

        public ServerConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            // initialize the websocket server
            InitializeWebSocketServer();
        }

        /// <summary>
        /// Initializes the Websocket-Server. This includes the creation of the Websocket-Object
        /// and the start of the Websocket-Server.
        /// </summary>
        public void InitializeWebSocketServer() {
            // initialize the websocket on the given url
            Console.WriteLine("Starting to initialize Websocket server");
            _webSocketServer = new WebSocketServer(base.GetURL());

            // add services
            _webSocketServer.AddWebSocketService<GameService>("/", () => new GameService(this));
            serviceManager = _webSocketServer.WebSocketServices;

            // start the websocket server
            _webSocketServer.Start();
            Console.WriteLine("The Websocket server was initilized");
            

            // wait for the user to quit the websocket server by typing any key in the console
            // TODO: add logic, that the websocket server is closed, when the server is shut down

            // Console.ReadKey();
            // _webSocketServer.Stop();
        }

        protected internal override void OnClose(CloseEventArgs e, String sessionID)
        {
            Log.Information("The connection to the Websocket server was closed by a client. The reason is: " + e.Reason);
        }

        protected internal override void OnError(ErrorEventArgs e, String sessionID)
        {
            Log.Error("Failed to establish connection to Websocket server on: " + base.GetURL());
            Log.Verbose("The reason for the failed try to connect is: " + e.Message);
        }

        protected internal override void OnMessage(MessageEventArgs e, String sessionID)
        {
            Log.Information("Received a message from client. The message is: " + e.Data);
            base.networkController.HandleReceivedMessage(e.Data);
        }

        protected internal override void OnOpen(String addressConnected, String sessionID)
        {
            Log.Information("Registred new connection from " + addressConnected + " to Websocket server");
            Log.Information("The ID of the new user is: " + sessionID);
        }
    }
}