using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MapEditor : MonoBehaviour, TileSelectionObserver {

    public EditableTile prefabTile;

    public CameraMovement cameraMovement;

    public Button setFieldTypeDune;
    public Button setFieldTypeMountain;
    public Button setFieldTypeFlatSand;
    public Button setFieldTypePlateau;
    public Button setFieldTypeCity;

    public GameObject sizeDialog;

    private HashSet<EditableTile> selectedTiles = new HashSet<EditableTile>();
    private HashSet<EditableTile> cityTiles = new HashSet<EditableTile>();

    public MapModel mapModel { get; private set; }

    private void Start() {
        AddButtonListeners();
    }

    /// <summary>
    /// This method tries to build and show the map in the 3-dimensional space
    /// </summary>
    /// <param name="sizeX">The x size of the new Map, if no configuration is loaded</param>
    /// <param name="sizeY">The y size of the new Map, if no configuration is loaded</param>
    /// <returns></returns>
    public bool CreateMap(int sizeX, int sizeY) {

        if (sizeX == 0 || sizeY == 0 || sizeX*sizeY > 1000) {
            Debug.Log("Invalid Map size");
            return false;
        }

        mapModel = new MapModel(sizeX, sizeY);

        Transform map = new GameObject().transform;
        map.parent = transform;
        map.name = "Map";

        string playerPrefPath = PlayerPrefs.GetString("scenarioOpenPath");


        // Check if the file really exists
        if (!File.Exists(playerPrefPath)) {
            PlayerPrefs.SetString("scenarioOpenPath", "");
        }


        //if a filename was passed thru PlayerPrefs than load that file
        if (!playerPrefPath.Equals("")) {
            Debug.Log(playerPrefPath);
            string[,] mapString = ConfigJSONLink.getStringArrayFromSceneryObjectJSON(playerPrefPath);
            sizeX = mapString.GetLength(1);
            sizeY = mapString.GetLength(0);

            mapModel = new MapModel(sizeX, sizeY);

            ConfigJSONLink.MapModelFromJSON(mapModel, playerPrefPath);

            for (int x = 0; x < sizeX; x++) {
                for (int y = 0; y < sizeY; y++) {
                    EditableTile tile = Instantiate(prefabTile, new Vector3(), Quaternion.Euler(Vector3.right * 90));
                    tile.transform.parent = map;

                    tile.indexX = x;
                    tile.indexY = y;

                    tile.fieldType = mapModel.getTileAtPosition(x,y).fieldType;
                    tile.setShift(new Vector2(-sizeX / 2f + 0.5f, -sizeY / 2f + 0.5f));

                    tile.setSelectionObserver(this);

                    mapModel.setTileAtPosition(x, y, tile.model);
                }
            }
            return true;

        }

        // If there is no file to load ==> generate a random map
        // generate sizeX * sizeY tiles
        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                EditableTile tile = Instantiate(prefabTile, new Vector3(), Quaternion.Euler(Vector3.right * 90));
                tile.transform.parent = map;

                tile.indexX = x;
                tile.indexY = y;

                // random terrain generation
                if (Random.value * ((sizeX - x) / ((float)sizeX)) * ((sizeY - y) / ((float)sizeY)) > 0.25 &&
                    (x == 0 || y == 0 || mapModel.getTileAtPosition(x,y-1).fieldType == FieldType.MOUNTAINS || mapModel.getTileAtPosition(x-1,y).fieldType == FieldType.MOUNTAINS)) {
                    tile.fieldType = FieldType.MOUNTAINS;
                }
                else if (x != 0 && y != 0 && (x == sizeX - 1 || y == sizeY - 1 || (x > sizeX - 3 && y > sizeY - 3))) {
                    tile.fieldType = FieldType.PLATEAU;
                }
                else {
                    tile.fieldType = Random.value > 0.5 ? FieldType.FLAT_SAND : FieldType.DUNE;
                }

                tile.setShift(new Vector2(-sizeX / 2f + 0.5f, -sizeY / 2f + 0.5f));

                tile.setSelectionObserver(this);

                // update model
                mapModel.setTileAtPosition(x, y, tile.model);
            }
        }

        // adjust camera params for map size
        cameraMovement.radius = Mathf.Max(sizeX, sizeY) * 2;
        cameraMovement.maxRadius = Mathf.Max(cameraMovement.radius, 25);

        return true;
    }

    private void AddButtonListeners() {
        setFieldTypeDune.onClick.AddListener(() => SetTypeForSelectedTiles(FieldType.DUNE));
        setFieldTypeMountain.onClick.AddListener(() => SetTypeForSelectedTiles(FieldType.MOUNTAINS));
        setFieldTypeFlatSand.onClick.AddListener(() => SetTypeForSelectedTiles(FieldType.FLAT_SAND));
        setFieldTypePlateau.onClick.AddListener(() => SetTypeForSelectedTiles(FieldType.PLATEAU));
        setFieldTypeCity.onClick.AddListener(() => SetTypeForSelectedTiles(FieldType.CITY));
    }


    void TileSelectionObserver.OnTileSelected(EditableTile tile) {
        // deselect other tiles if shift not pressed
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) DeselectAllTiles();

        // select tile
        tile.SetSelected(true);
        selectedTiles.Add(tile);
    }

    void TileSelectionObserver.OnTileDeselected(EditableTile tile) {
        // deselect other tiles if shift not pressed
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) DeselectAllTiles();

        // only deselect the one tile
        else {
            tile.SetSelected(false);
            selectedTiles.Remove(tile);
        }
    }


    private void DeselectAllTiles() {
        foreach (EditableTile selectedTile in selectedTiles) {
            selectedTile.SetSelected(false);
        }
        selectedTiles.Clear();
    }


    public void SetTypeForSelectedTiles(FieldType type) {
        int maxChanges = int.MaxValue;
        int count = 0;

        // handle cities (max 2)
        if (type == FieldType.CITY && selectedTiles.Count + cityTiles.Count > 2) maxChanges = 2 - cityTiles.Count;

        foreach (EditableTile selectedTile in selectedTiles) {
            if (++count > maxChanges) break;

            // remember city tiles
            if (type == FieldType.CITY) cityTiles.Add(selectedTile);
            else cityTiles.Remove(selectedTile);

            // update field type
            selectedTile.fieldType = type;
        }

        DeselectAllTiles();
    }
}