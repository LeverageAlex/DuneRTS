using System;
using WebSocketSharp;

using Serilog;
using System.Threading;

namespace GameData.network.controller
{
    /// <summary>
    /// connection handler for websocket (client), which handles the connection to the websocket server
    /// </summary>
    /// <remarks>
    /// This concrete connection handler implementation for the client inherit the abstract connection handler (<see cref="AConnectionHandler"/>).
    /// Therefore it needs to implement the callbacks for the different events, which can occur in a websocket connection and add them to the internal event handler.
    /// Furthermore this class create a new websocket on creation and connect to it, until it will be closed.
    /// </remarks>
    public class ClientConnectionHandler : AConnectionHandler
    {
        /// <summary>
        /// the reference to the websocket
        /// </summary>
        public WebSocket WebSocket { get; set; }
        private Thread _thread;
        private bool keepThreadAlive = true;
     //   private readonly ClientSocketErrorHandler ClientPasser;

        /// <summary>
        /// creates a new client connection handler and initializes the connection to the websocket server,
        /// which is identified by the given address
        /// </summary>
        /// <param name="ServerAddress">hostname or IPv4 address of the websocket server, to connect to</param>
        /// <param name="Port">the port of the websocket server opened for a new websocket connection</param>
        public ClientConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            // initialize the websocket and connect it to the server (but not block the programm because to waiting for websocket to be closed
            //   this.ClientPasser = ClientPasser;
            keepThreadAlive = true;
            _thread = new Thread(InitializeWebSocket);
            _thread.Start();

        }

        /// <summary>
        /// creates a new websocket with the given server address, set the event callbacks and connect to the websocket server
        /// </summary>
        public void InitializeWebSocket()
        {
            while (true)
            {

                WebSocket = new WebSocket(GetURL());
                //  WebSocket.Connect();
                Log.Debug("Created new websocket, which will connect to " + GetURL());

                SetCallbackFunctionsToInternalEventHandler();

                // connect to websocket server
                ConnectToWebsocketServer();


                while (keepThreadAlive)
                {
                    Thread.Sleep(300);
                }
                Log.Debug("Killed Current Connection");
                WebSocket.Close();
                while (!keepThreadAlive)
                {
                    Thread.Sleep(70);
                }
                Log.Debug("Restarting socket");
            }

            // wait for a signal from the user, to close the connection to the server
            // TODO: implement logic, which holds the websocket alive and close it by user client
           // Console.ReadKey();
            //CloseConnectionToWebsocketServer(CloseStatusCode.Away, "User closed the connection via the command line");

        }

        public void ReconnectSocket(string ServerAddress, int Port)
        {
            this.SERVER_ADDRESS = ServerAddress;
            this.PORT = Port;
            keepThreadAlive = true;

        }

        /// <summary>
        /// set the callbacks functions (onOpen, ...) for the internal event handler to the implementation from abstract connection handler
        /// </summary>
        private void SetCallbackFunctionsToInternalEventHandler()
        {
            WebSocket.OnClose += (sender, e) =>
            {
                OnClose(e, "");
           //     ClientPasser.HandleOnCloseMessage();
            };

            WebSocket.OnError += (sender, e) =>
            {
                OnError(e, "");
          //     ClientPasser.HandleErrorMessage();
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                OnMessage(e, "");
            };

            WebSocket.OnOpen += (sender, e) =>
            {
                OnOpen(GetURL(), "");
            };
        }

        /// <summary>
        /// connects to the websocket server
        /// </summary>
        public void ConnectToWebsocketServer()
        {
            WebSocket.Connect();
     //       Log.Information("The client " + WebSocket.Credentials.Domain + " connect to the websocket server at " + GetURL());
        }

        // TODO: hide the CloseStatusCode and implement own variant, which can be exposed --> map own codes on CloseStatusCode
        /// <summary>
        /// closes the connection to the websocket server and provide a close code
        /// </summary>
        /// <param name="statusCode">code of the reason, why the connection was closed</param>
        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode)
        {
            WebSocket.Close(statusCode);

        }

        public void CloseConnectionToWebsocketServer()
        {
            keepThreadAlive = false;
        }

        public bool ConnectionIsAlive()
        {
            return WebSocket.IsAlive;
        }

        /// <summary>
        /// closes the connection to the websocket server and provide a close code
        /// </summary>
        /// <param name="statusCode">code of the reason, why the connection was closed</param>
        /// <param name="reason">detailed explanation of the reason, why the connection was closed</param>
        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode, String reason)
        {
            WebSocket.Close(statusCode, reason);
        }

        // TODO: finish the implementation of the callback functions

        /// <summary>
        /// callback, which is triggered, if the connection to the server was closed.
        /// It prints the reason, why the connection was closed
        /// </summary>
        /// <param name="e">close arguments which contain the reason for the closing</param>
        /// <param name="sessionID">not necessary, because every websocket server is unique</param>
        protected internal override void OnClose(CloseEventArgs e, string sessionID)
        {
            Log.Warning("The connection to the websocket server was closed by the server. The reason is: " + e.Reason);
        }

        /// <summary>
        /// callback, which is triggered, if an error occur related with the websocket connection
        /// It prints the error.
        /// </summary>
        /// <param name="e">error arguments which contain the error message</param>
        /// <param name="sessionID">not necessary, because every websocket server is unique</param>
        protected internal override void OnError(ErrorEventArgs e, string sessionID)
        {
            Log.Error("An error occured on the connection to the Websocket server. The error is: " + e.Message);
            //WebSocket.Close();
        }

        /// <summary>
        /// callback, which is triggered, if the websocket notice a message from the server.
        /// It forwards this message string to the network controller, which handles it.
        /// </summary>
        /// <param name="e">message arguments which contain the message string</param>
        /// <param name="sessionID">not necessary, because every websocket server is unique</param>
        protected internal override void OnMessage(MessageEventArgs e, string sessionID)
        {
            Log.Debug("Received new message from the Websocket server. The message is: " + e.Data);
            NetworkController.HandleReceivedMessage(e.Data, sessionID);
        }

        /// <summary>
        /// callback, which is triggered, if the connection to the server was opened.
        /// It prints the server, the connection was established to.
        /// </summary>
        /// <param name="addressConnected">the address of the websocket server connected to</param>
        /// <param name="sessionID">not necessary, because every websocket server is unique</param>
        protected internal override void OnOpen(string addressConnected, string sessionID)
        {
            Log.Information("Connected to websocket server with address: " + addressConnected);
        }
    }
}