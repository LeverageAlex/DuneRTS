using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using GameData.network.util.world;

//[Serializable]
public class Specs
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Position> path;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Position target;

    [JsonConstructor]
    public Specs(Position target, List<Position> path )
    {
        this.path = path;
        this.target = target;

    }
}
