using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using GameData.network.util.world;

/// <summary>
/// This class is used to support different kinds of messages.
/// </summary>
public class Specs
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Position> path;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Position target;

    /// <summary>
    /// Constructor of the Class Specs
    /// </summary>
    /// <param name="target">defines the target Position of a action</param>
    /// <param name="path">defines the path of a action</param>
    [JsonConstructor]
    public Specs(Position target, List<Position> path )
    {
        this.path = path;
        this.target = target;
    }
}
