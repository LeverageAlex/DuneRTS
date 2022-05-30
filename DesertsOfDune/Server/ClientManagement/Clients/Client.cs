using System;
namespace Server.ClientManagement.Clients
{
    /// <summary>
    /// Base class for all user clients.
    /// </summary>
    /// <remarks>
    /// A user client can be a
    /// <list type="bullet">
    /// <item>human player</item>
    /// <item>ai player</item>
    /// <item>spectator (live)</item>
    /// <item>spectator (watching replay)</item>
    /// </list>
    /// </remarks>
    public abstract class Client
    {
        /// <summary>
        /// the name of the client
        /// </summary>
        public string ClientName { get; }

        /// <summary>
        /// a secret string, which is used as an identifier for the reconnect to "authorize" the client
        /// </summary>
        public string ClientSecret { get; }

        /// <summary>
        /// boolen, which is true, if the client is active (so a player) or not (so a spectator)
        /// </summary>
        public bool IsActivePlayer { get; }

        /// <summary>
        /// the client id used for identifying the client
        /// </summary>
        public int ClientID { get; }

        /// <summary>
        /// the session id of the connection of the client
        /// </summary>
        public string SessionID { get; set; }

        protected Client(string clientName, bool isActivePlayer, string sessionID)
        {
            ClientName = clientName;
            ClientSecret = CreateClientSecret();
            IsActivePlayer = isActivePlayer;
            ClientID = CreateClientID();
            SessionID = sessionID;
        }

        /// <summary>
        /// creates a (approximatley) unique id, which is parsed as a string. 
        /// </summary>
        /// <remarks>
        /// For further information see Guid.NewGuid in the System namespace</remarks>
        /// <returns>the client secret</returns>
        private string CreateClientSecret()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        /// <summary>
        /// creates a random integer number, with is used as a client ID
        /// </summary>
        /// <returns>the client id </returns>
        private int CreateClientID()
        {
            // TODO: check, that it is unique
            return new Random().Next(Int32.MaxValue);
        }
    }
}
