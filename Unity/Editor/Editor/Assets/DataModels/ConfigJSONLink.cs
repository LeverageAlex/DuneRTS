using Newtonsoft.Json;
using SFB;
using System;
using System.Diagnostics;
using System.IO;

public static class ConfigJSONLink
{
    /// <summary>
    /// This method saves the map that is held by the mapModel into a file.
    /// </summary>
    /// <param name="mapModel">The mapModel that contains the map</param>
    /// <param name="filename">The path the file should be saved at. If null the user could choose via file browser</param>
    /// <returns>true, if the process was successfull</returns>
    public static bool JSONFromMapModel(MapModel mapModel, string filename) {
        TileInterface[,] map = mapModel.tiles;

        string[,] serializeableMap = convertTileToTilename(map);

        
        SceneryObject exportHelperObject = new SceneryObject(serializeableMap);

        string jsonText = JsonConvert.SerializeObject(exportHelperObject, Formatting.Indented);

        

        string path = filename;
        if (filename == null) {
            path = StandaloneFileBrowser.SaveFilePanel("Choose name", "", "Configuration", "scenario.json");
        }

        path.Replace("scenario.scenario", "scenario");  // Correct the ending if the user is not capable of giving the file a proper name

        

        if(path.Length < 1 || path == null) {
            return false;
        }

        File.WriteAllText(path, jsonText);

        return true;
    }


    /// <summary>
    /// This method fills the map of the map model with its tiles based on the schema that is saved at the path filename
    /// </summary>
    /// <param name="mapModel"></param>
    /// <param name="filename"></param>
    public static bool MapModelFromJSON(MapModel mapModel, string filename) {
        string[,] mapString = getStringArrayFromSceneryObjectJSON(filename);

        TileModel[,] map = convertTilenameToTileModel(mapString);

        if (map == null) return false;


        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                mapModel.setTileAtPosition(x, y, map[y, x]);
            }
        }
        return true;
    }


    /// <summary>
    /// This method reads a json file and tries to convert the scenario configuration that is stored in it,
    /// into a 2-dimensional string representing the map
    /// </summary>
    /// <param name="filename">The file to read from</param>
    /// <returns>Return the map as a 2-dimensional string array</returns>
    public static string[,] getStringArrayFromSceneryObjectJSON(string filename) {
        string json = File.ReadAllText(filename);

        SceneryObject sceneryObject = JsonConvert.DeserializeObject<SceneryObject>(json);

        return sceneryObject.scenario;
    }

    /// <summary>
    /// The method converts tile information of the map from TileInterface its string representative.
    /// </summary>
    /// <param name="map"></param>
    /// <returns>string[,] of the tilenames </returns>
    private static string[,] convertTileToTilename(TileInterface[,] map) {

        string[,] tilenameArray = new string[map.GetLength(0), map.GetLength(1)];

        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                tilenameArray[y,x] = map[y, x].fieldType.ToString();
            }
        }

        return tilenameArray;
    }

    /// <summary>
    /// The method converts string information of the map to a tile model.
    /// </summary>
    /// <param name="map"></param>
    /// <returns>string[,] of the tilenames </returns>
    private static TileModel[,] convertTilenameToTileModel(string[,] map)
    {

        TileModel[,] tilenameArray = new TileModel[map.GetLength(0), map.GetLength(1)];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                FieldType type;

                //Check if field is known
                if (!System.Enum.TryParse<FieldType>(map[y, x], out type))
                {
                    return null;
                }
                tilenameArray[y, x] = new TileModel(type, x, y);
            }
        }
        return tilenameArray;
    }


    /// <summary>
    /// This method converts the partie configuration data set in the editor into a file, that is than saved
    /// </summary>
    /// <param name="partieConfigModel">The model that the parameters are retrieved from</param>
    /// <returns>Returns true, if the process of saving the configuration was successfull</returns>
    public static bool JSONFromMatchConfig(PartieConfigModel partieConfigModel)
    {
        // initialize Object and convert to JSON String
        PartieObject exportHelperObject = new PartieObject(getCharacterValues(partieConfigModel, CharacterType.NOBLE),
            getCharacterValues(partieConfigModel, CharacterType.MENTAT),
            getCharacterValues(partieConfigModel, CharacterType.BENE_GESSERIT),
            getCharacterValues(partieConfigModel, CharacterType.FIGHTER),
            partieConfigModel.numberOfRounds,
            partieConfigModel.actionTimeUserClient,
            partieConfigModel.actionTimeAiClient,
            partieConfigModel.highGroundBonusRatio,
            partieConfigModel.lowGroundMalusRatio,
            partieConfigModel.kanlySuccessProbability,
            partieConfigModel.spiceMinimum,
            partieConfigModel.cellularAutomate,
            partieConfigModel.sandWormSpeed,
            partieConfigModel.sandWormSpawnDistance,
            partieConfigModel.cloneProbability,
            partieConfigModel.minPauseTime,
            partieConfigModel.maxStrikes,
            partieConfigModel.crashProbability);

        Debug.WriteLine("[JSON CONVERT] Number of Rounds: " + partieConfigModel.numberOfRounds);
        Debug.WriteLine("[JSON CONVERT] actionTime: " + partieConfigModel.actionTimeUserClient);
        Debug.WriteLine("[JSON CONVERT] highGroundBonusRatio: " + partieConfigModel.highGroundBonusRatio);

        string jsonText = JsonConvert.SerializeObject(exportHelperObject, Formatting.Indented);

        // Set path to save config file
        string path = StandaloneFileBrowser.SaveFilePanel("Choose name", "", "Configuration", "party.json");
        path.Replace("party.party", "party");  // Correct the ending if the user is not capable of giving the file a proper name

        // Save config file
        if (path.Length < 1 || path == null) {
            return false;
        }

        File.WriteAllText(path, jsonText);

        return true;
    }

    /// <summary>
    /// Method to get all the characters values from the partie config model
    /// </summary>
    /// <param name="partieConfigModel">The model that holds all the information</param>
    /// <param name="type">The type of character the data should be collected from</param>
    /// <returns>A character Object used for serialization</returns>
    public static character getCharacterValues(PartieConfigModel partieConfigModel, CharacterType type) {
        return new character(partieConfigModel.getHealthPoints(type),
            partieConfigModel.getMovementPoints(type),
            partieConfigModel.getActionPoint(type),
            partieConfigModel.getAttackDamage(type),
            partieConfigModel.getInventorySize(type),
            partieConfigModel.getHealingHP(type));
    }

}

// The 3 following classes are only used to make the szerialization process much easier

public class SceneryObject {
    public string[,] scenario;

    public SceneryObject(string[,] json) {
        this.scenario = json;
    }
}

public class PartieObject {
    public character noble;
    public character mentat;
    public character beneGesserit;
    public character fighter;
    public int numbOfRounds;
    public int actionTimeUserClient;
    public int actionTimeAiClient;
    public float highGroundBonusRatio;
    public float lowerGroundMalusRatio;
    public float kanlySuccessProbability;
    public int spiceMinimum;
    public string cellularAutomaton;
    public int sandWormSpeed;
    public int sandWormSpawnDistance;
    public float cloneProbability;
    public int minPauseTime;
    public int maxStrikes;
    public float crashProbability;

    public PartieObject(character noble,
                        character mentat,
                        character beneGesserit,
                        character fighter,
                        int numbOfRounds,
                        int actionTimeUserClient,
                        int actionTimeAiClient,
                        float highGroundBonusRatio,
                        float lowerGroundMalusRatio,
                        float kanlySuccessProbability,
                        int spiceMinimum,
                        string cellularAutomaton,
                        int sandWormSpeed,
                        int sandWormSpawnDistance,
                        float cloneProbability,
                        int minPauseTime,
                        int maxStrikes,
                        float crashProbability) {
        this.noble = noble;
        this.mentat = mentat;
        this.beneGesserit = beneGesserit;
        this.fighter = fighter;
        this.numbOfRounds = numbOfRounds;
        this.actionTimeUserClient = actionTimeUserClient;
        this.actionTimeAiClient = actionTimeAiClient;
        this.highGroundBonusRatio = highGroundBonusRatio;
        this.lowerGroundMalusRatio = lowerGroundMalusRatio;
        this.kanlySuccessProbability = kanlySuccessProbability;
        this.spiceMinimum = spiceMinimum;
        this.cellularAutomaton = cellularAutomaton;
        this.sandWormSpeed = sandWormSpeed;
        this.sandWormSpawnDistance = sandWormSpawnDistance;
        this.cloneProbability = cloneProbability;
        this.minPauseTime = minPauseTime;
        this.maxStrikes = maxStrikes;
        this.crashProbability = crashProbability;
    }

}

public class character    {

    public int maxHP;
    public int maxMP;
    public int maxAP;
    public int damage;
    public int inventorySize;
    public int healingHP;

    public character(int maxHP, int maxMP, int maxAP, int damage, int inventorySize, int healingHP) {
        this.maxHP = maxHP;
        this.maxMP = maxMP;
        this.maxAP = maxAP;
        this.damage = damage;
        this.inventorySize = inventorySize;
        this.healingHP = healingHP;
    }

}