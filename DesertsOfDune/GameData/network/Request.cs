using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

[Serializable]
public class Request
{
    public string type;
    public String version;
    public int clientID;
    public int characterID;
    public Specs specs;
    // TODO import characterturnhandler.Action out of unity to use it here.
    //public CharacterTurnHandler.Actions action;
    public int targetID;

    /// <summary>
    /// The constructor of the class Request
    /// </summary>
    /// <param name="requestType">This parameter ensures the result json of the request type is a string.</param>
    public Request(RequestType requestType)
    {
        this.type = Enum.GetName(typeof(RequestType), requestType);
    }

    /// <summary>
    /// This enum represents the different types of Request the client can make.
    /// </summary>
    public enum RequestType
    {
        HOUSE_REQUEST,
        TURN_REQUEST,
        MOVEMENT_REQUEST,
        ACTION_REQUEST,
        END_TURN_REQEST,
        PAUS_REQEST,
        GAMESTATE_REQUEST
    }
}
