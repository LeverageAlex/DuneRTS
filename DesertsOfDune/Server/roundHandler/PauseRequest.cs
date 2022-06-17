using System;
using Server.Configuration;

namespace Server.roundHandler
{
    /// <summary>
    /// Represents the pause of a client
    /// </summary>
    /// <remarks>
    /// So contains the pause request, the time of request and the id of the client, who requested the pause
    /// </remarks>
    public class PauseRequest
    {
        /// <summary>
        /// the system time, the pause was started by a client
        /// </summary>
        /// <remarks>
        /// This is necessary to know, when the other client can also resume the game
        /// </remarks>
        public DateTime TimePauseStarted { get; }

        /// <summary>
        /// states, whether this request is a "pause" request (true) or a "resume" request (false)
        /// </summary>
        public bool RequestedPause { get; }

        /// <summary>
        /// the ID of the client, who requested the pause / resumption
        /// </summary>
        public int ClientID { get; }

        /// <summary>
        /// creates a new pause request
        /// </summary>
        /// <param name="pause">true, if this request is a "pause" request and false, if it is a "resumption" request</param>
        /// <param name="clientID">the ID of the client, who requested</param>
        public PauseRequest(bool pause, int clientID)
        {
            TimePauseStarted = DateTime.Now;
            RequestedPause = pause;
            ClientID = clientID;
        }

        /// <summary>
        /// determines, whether the other client can resume the game, because the requesting client
        /// exceeded his maximal pause time
        /// </summary>
        /// <remarks>
        /// The maximal pause time is exceeded, if
        /// (<current time> - <start time of pause>).toSeconds >= maximal pause time (from party configuration)
        /// </remarks>
        /// <returns>true, if the other client can resume the game, if he wants</returns>
        public bool CanPauseRepealedByOtherClient()
        {
            // fetch the maximal pause time from configuration
            int maxPauseTime = PartyConfiguration.GetInstance().minPauseTime;
            return (DateTime.Now - TimePauseStarted).TotalMilliseconds >= maxPauseTime;
        }
    }
}
