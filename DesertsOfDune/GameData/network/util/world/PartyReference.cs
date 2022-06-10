using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class is used for the GameConfigMessage
    /// </summary>
    public class PartyReference
    {
        [JsonProperty]
        public string refr;

        public PartyReference(string refr)
        {
            this.refr = refr;
        }

    }
}
