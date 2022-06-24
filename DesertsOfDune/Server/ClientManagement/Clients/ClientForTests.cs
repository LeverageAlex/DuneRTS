using System;
using System.Collections.Generic;
using GameData.Clients;
using GameData.network.util.world;

namespace Server.ClientManagement.Clients
{
    /// <summary>
    /// Used for the Tests (set some default values allow operations)
    /// </summary>
    public class ClientForTests : Player
    {
        public ClientForTests(List<Character> characters) : base("TestClient", "", false)
        {
            UsedGreatHouse = new GreatHouse(characters);
        }
    }
}

