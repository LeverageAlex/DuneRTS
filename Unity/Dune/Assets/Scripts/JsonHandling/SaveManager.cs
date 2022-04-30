using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";

    /// <summary>
    /// This method saves the character to the MyData.txt File.
    /// </summary>
    /// <param name="character"></param>
    public static void Save(Character character)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(character);
        File.WriteAllText(dir + fileName, json);
    }

    public static void SaveRequest(Request request)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(request);
        File.WriteAllText(dir + fileName, json);
    }

    /// <summary>
    /// This method loads a character form the File where the json representation of the character is stored.
    /// </summary>
    /// <returns></returns>
    public static Character Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        Character character = new Character();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            character = JsonUtility.FromJson<Character>(json);
        } 
        else
        {
            Debug.Log("Save File does not exist.");
        }
        return character;
    }
}
