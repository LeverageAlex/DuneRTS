using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GameData.network.util.world;

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
        /// the client secret of the ai client playing this party (used for rejoin)
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// the id of the ai client playing this party
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// the amount of spice, collected and transfered to the city by all characters
        /// </summary>
        public int CitySpice { get; set; }

        /// <summary>
        /// the number of used family atomics in this party
        /// </summary>
        public int UsedFamilyAtomics { get; }

        public AIPlayerMessageController MessageController { get; private set; }

        public World World { get; private set; }

        public Character CurrentCharacter { get; set; }

        /// <summary>
        /// hide default constructor for implementing the singleton pattern and sets the message controller used in the party by getting it from the main class
        /// </summary>
        private Party()
        {
            this.World = new World();
            this.UsedFamilyAtomics = 0;
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

        /// <summary>
        /// creates a new party
        /// </summary>
        /// <param name="messageController">the message controller used in this party</param>
        public static void CreateParty(AIPlayerMessageController messageController)
        {
            Party party = GetInstance();
            party.MessageController = messageController;
        }

        /// <summary>
        /// joins a party by sending a join request
        /// </summary>
        /// <param name="clientName">the name of this ai</param>
        public void JoinParty(string clientName)
        {
            MessageController.DoSendJoin(clientName);
        }
    }
}
