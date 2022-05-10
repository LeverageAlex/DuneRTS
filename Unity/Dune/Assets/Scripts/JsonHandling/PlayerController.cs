using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
/// <summary>
/// This Class Handles all messages for the Client.
/// </summary>
public static class PlayerController
{
    private static string version = "v1";
    /// <summary>
    /// This method triggers the MovementRequest
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    /// <param name="characterID">the id of the character</param>
    /// <param name="path">the path the character should take</param>
   public static void DoRequestMovement(int clientID, int characterID, LinkedList<Vector3> path)
    {
        Specs specs = new Specs();
        specs.path = ConvertPath(path);
        Request request = new Request(Request.RequestType.MOVEMENT_REQUEST);
        request.version = version;
        request.clientID = 1234;
        request.characterID = characterID;
        request.specs = specs;

        // for testing perpeces only
        string data = JsonConvert.SerializeObject(request, new JsonSerializerSettings());
        Debug.Log("Updated: " + data);

    } 

    public static void DoRequestAction()
    {
        Request request = new Request(Request.RequestType.ACTION_REQUEST);

    }

    /// <summary>
    /// This method is used to convert the Path from a list of Vector3 to a List of type Vector in oderer to convert this to JSON.
    /// </summary>
    /// <param name="selCharPath">the path to be converted.</param>
    /// <returns>The converted path</returns>
    private static List<Vector> ConvertPath(LinkedList<Vector3> selCharPath)
    {
        List<Vector> path = new List<Vector>();
        foreach (Vector3 vec in selCharPath)
        {
            Vector v = new Vector(vec.x, vec.z);
            path.Add(v);
        }
        return path;
    }
}
