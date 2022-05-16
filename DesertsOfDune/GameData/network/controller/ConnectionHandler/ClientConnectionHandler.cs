using System;
using WebSocketSharp;

using Serilog;

namespace GameData.network.controller
{
    public class ClientConnectionHandler : AConnectionHandler
    {
        public WebSocket webSocket { get; set; }

        public ClientConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
            InitializeWebSocket();
        }

        /// <summary>
        /// Initializes the websocket at the given address (see Constructor)
        /// </summary>
        public void InitializeWebSocket()
        {
            webSocket = new WebSocket(GetURL());
            webSocket.OnClose += (sender, e) =>
            {
                OnClose(e, "");
            };

            webSocket.OnError += (sender, e) =>
            {
                OnError(e, "");
            };

            webSocket.OnMessage += (sender, e) =>
            {
                OnMessage(e, "");
            };

            webSocket.OnOpen += (sender, e) =>
            {
                OnOpen(base.GetURL(), "");
            };

            ConnectToWebsocketServer();
        }

        public void ConnectToWebsocketServer()
        {
            webSocket.Connect();
        }

        // TODO: hide the CloseStatusCode and implement own variant, which can be exposed --> map own codes on CloseStatusCode
        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode)
        {
            webSocket.Close(statusCode);
        }

        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode, String reason)
        {
            webSocket.Close(statusCode, reason);
        }

        protected internal override void OnClose(CloseEventArgs e, string sessionID)
        {
            Log.Information("The connection to the Websocket server was close by the server. The reason is: " + e.Reason);
        }

        protected internal override void OnError(ErrorEventArgs e, string sessionID)
        {
            Log.Error("An error occured on the connection to the Websocket server. The error is: " + e.Message);
        }

        protected internal override void OnMessage(MessageEventArgs e, string sessionID)
        {
            Log.Information("Received new message from the Websocket server. The message is: " + e.Data);
            networkController.HandleReceivedMessage(e.Data);
        }

        protected internal override void OnOpen(string addressConnected, string sessionID)
        {
            Log.Information("Connected to " + addressConnected);
        }
    }
}