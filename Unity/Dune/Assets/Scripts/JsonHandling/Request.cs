using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Request
{
    public RequestType type;
    public String version;
    public int clientID;
    public int characterID;
    [SerializeField]
    public List<List<Vector3>> specs;

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
