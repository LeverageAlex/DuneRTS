using System;

namespace AIClient
{
    /// <summary>
    /// Represents one party, an ai client is playing.
    /// </summary>
    /// <remarks>
    /// Therefore it stores all informationen, necessary for preparing and playing one party und provides methods for the
    /// preparation and the execution of a game, so methods, which trigger the appropiate events.
    /// The necessary information are:
    /// <list type="bullet">
    /// <item>the party configuration entries</item>
    /// <item>the map and characters, so the entire "world"</item>
    /// <item>the chosen great house</item>
    /// </list>
    /// </remarks>
    public class Party
    {
        private static Party singleton;

        /// <summary>
        /// hide default constructor for implementing the singleton pattern
        /// </summary>
        private Party()
        {
        }

        /// <summary>
        /// retrieves an instance of this party for implementing the singleton pattern
        /// </summary>
        /// <returns>the reference to this party instance</returns>
        public static Party GetInstance()
        {
            if (singleton == null)
            {
                singleton = new Party();
            }
            return singleton;
        }

        public 
    }
}
