using System;
//using WebSocketSharp;

//using Serilog;

namespace GameData.network.controller
{
    public class ClientConnectionHandler : AConnectionHandler
    {
        /*
        private WebSocket _webSocket;

        public ClientConnectionHandler(string ServerAddress, int Port) : base(ServerAddress, Port)
        {
        }

        /// <summary>
        /// Initializes the websocket at the given address (see Constructor)
        /// </summary>
        public void InitializeWebSocket()
        {
            _webSocket = new WebSocket(base.GetURL());
            _webSocket.OnClose += (sender, e) =>
            {
                OnClose(e, "");
            };

            _webSocket.OnError += (sender, e) =>
            {
                OnError(e, "");
            };

            _webSocket.OnMessage += (sender, e) =>
            {
                OnMessage(e, "");
            };

            _webSocket.OnOpen += (sender, e) =>
            {
                OnOpen("", base.GetURL());
            };
        }

        public void ConnectToWebsocketServer()
        {
            _webSocket.Connect();
        }

        // TODO: hide the CloseStatusCode and implement own variant, which can be exposed --> map own codes on CloseStatusCode
        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode)
        {
            _webSocket.Close(statusCode);
        }

        public void CloseConnectionToWebsocketServer(CloseStatusCode statusCode, String reason)
        {
            _webSocket.Close(statusCode, reason);
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
        }

        protected internal override void OnOpen(string sessionID, string addressConnected)
        {
            Log.Information("Connected to " + addressConnected);
        }
        */
    }
}