using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GameData;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.network.util.parser;

/// <summary>
/// This Class Handles all messages for the Client.
/// </summary>
public static class PlayerController
{
    /// <summary>
    /// This method triggers the MovementRequest
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    /// <param name="characterID">the id of the character</param>
    /// <param name="path">the path the character should take</param>
    public static void DoMovementRequest(int clientID, int characterID, LinkedList<Vector3> path)

    {

        List<Position> positions = ConvertPath(path);
        MovementRequestMessage movementRequestMessage = new MovementRequestMessage(clientID, characterID,positions);

        MessageConverter.FromMessage(movementRequestMessage);

    }

    /// <summary>
    /// This method does the action request
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    /// <param name="characterID">the id of the character</param>
    /// <param name="action">the action the character should use</param>
    /// <param name="target">the target of the action</param>
    public static void DoActionRequest(int clientID, int characterID, Enums.ActionType action, Node target /* missing target id param*/)
    {
        ActionRequestMessage actionRequestMessage = new ActionRequestMessage(1234, 12, action, new Position(1, 2), 1);
        MessageConverter.FromMessage(actionRequestMessage);
    }

    /// <summary>
    /// This method triggers the creation of a endTurnRequestmessage and forwards this message to the MessageConverter
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    /// <param name="characterID">the id of the character</param>
    public static void DoEndTurnRequest(int clientID, int characterID)
    {
        EndTurnRequestMessage endTurnRequestMessage = new EndTurnRequestMessage(clientID, characterID);
        MessageConverter.FromMessage(endTurnRequestMessage);
    }

    /// <summary>
    /// This method is used to tirgger the creation of a HouseRequestMessage and forward it to the MessageConverter
    /// </summary>
    /// <param name="houseName">the name of the requested house</param>
    public static void DoHouseRequest(string houseName)
    {
        HouseRequestMessage houseRequestMessage = new HouseRequestMessage(houseName);
        MessageConverter.FromMessage(houseRequestMessage);
    }

    /// <summary>
    /// This method is used to request the game state.
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    public static void DoGamestateRequest(int clientID)
    {
        GameStateRequestMessage gameStateRequestMessage = new GameStateRequestMessage(clientID);
        MessageConverter.FromMessage(gameStateRequestMessage);
    }

    public static void DoPauseRequest(int clientID)
    {
        PauseGameRequestMessage pauseGameRequestMessage = new PauseGameRequestMessage(clientID);
        MessageConverter.FromMessage(pauseGameRequestMessage);
    }

    /// <summary>
    /// This method is used to convert the Path from a list of Vector3 to a List of type Vector in oderer to convert this to JSON.
    /// </summary>
    /// <param name="selCharPath">the path to be converted.</param>
    /// <returns>The converted path</returns>
    private static List<Position> ConvertPath(LinkedList<Vector3> selCharPath)
    {
        List<Position> path = new List<Position>();
        foreach (Vector3 vec in selCharPath)
        {
            Position p = new Position(vec.x, vec.z);
            path.Add(p);
        }
        return path;
    }
}
