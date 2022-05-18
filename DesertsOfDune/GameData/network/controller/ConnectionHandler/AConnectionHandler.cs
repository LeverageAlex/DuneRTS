using System;

using WebSocketSharp;

namespace GameData.network.controller
{
    /// <summary>
    /// abstract 
    /// </summary>
    public abstract class AConnectionHandler
    {
        private String SERVER_ADDRESS { get; }
        private int PORT { get; }

        private String PROTOCOLL_SUFFIX = "ws://";
        
        public NetworkController NetworkController { get; set; }

        protected AConnectionHandler(string ServerAddress, int Port)
        {
            SERVER_ADDRESS = ServerAddress;
            PORT = Port;
        }

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