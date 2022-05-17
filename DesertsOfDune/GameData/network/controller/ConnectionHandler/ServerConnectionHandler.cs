using System;

using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSharp.Net;

using Serilog;
using System.Threading;

namespace GameData.network.controller
{
    public class GameService : WebSocketBehavior
    {
        
        private ServerConnectionHandler _connectionHandler;

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
        public WebSocketSessionManager sessionManager { get; set; }
        

        public ServerConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            // initialize the websocket server
            Thread t = new Thread(InitializeWebSocketServer);
            t.Start();
        }

        /// <summary>
        /// Initializes the Websocket-Server. This includes the creation of the Websocket-Object
        /// and the start of the Websocket-Server.
        /// </summary>
        public void InitializeWebSocketServer() {
            // initialize the websocket on the given url
            Console.WriteLine("Starting to initialize Websocket server");
            _webSocketServer = new WebSocketServer(GetURL());

            // add services
            _webSocketServer.AddWebSocketService<GameService>("/", () => new GameService(this));

            // start the websocket server
            _webSocketServer.Start();
            sessionManager = _webSocketServer.WebSocketServices["/"].Sessions;
            Console.WriteLine("The Websocket server was initilized");
            

            // wait for the user to quit the websocket server by typing any key in the console
            // TODO: add logic, that the websocket server is closed, when the server is shut down

            Console.ReadKey();
            _webSocketServer.Stop();
            Console.WriteLine("Shutdown the Websocket server");
        }

        protected internal override void OnClose(CloseEventArgs e, String sessionID)
        {
            Console.WriteLine("The connection to the Websocket server was closed by a client. The reason is: " + e.Reason);
        }

        protected internal override void OnError(ErrorEventArgs e, String sessionID)
        {
            Console.WriteLine("Failed to establish connection to Websocket server on: " + GetURL());
            Console.WriteLine("The reason for the failed try to connect is: " + e.Message);
        }

        protected internal override void OnMessage(MessageEventArgs e, String sessionID)
        {
            Console.WriteLine("Received a message from client. The message is: " + e.Data);
            Console.WriteLine(((ServerNetworkController)networkController).GetType());
            ((ServerNetworkController)networkController).HandleReceivedMessage(e.Data);
            base.networkController.HandleReceivedMessage(e.Data);
        }

        protected internal override void OnOpen(String addressConnected, String sessionID)
        {
            Console.WriteLine("Registred new connection from " + addressConnected + " to Websocket server");
            Console.WriteLine("The ID of the new user is: " + sessionID);
        }
    }
}