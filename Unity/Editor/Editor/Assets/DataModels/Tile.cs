using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, TileInterface {
    public GameObject prefabDuneTile;
    public GameObject prefabMountainTile;
    public GameObject prefabFlatSandTile;
    public GameObject prefabPlateauTile;
    public GameObject prefabCityTile;

    protected GameObject duneTile;
    protected GameObject mountainTile;
    protected GameObject flatSandTile;
    protected GameObject plateauTile;
    protected GameObject cityTile;

    protected GameObject activeTile;

    public TileModel model;

    private Vector2 shift;


    void Awake() {
        int indexX = (int)transform.position.x;
        int indexY = (int)transform.position.z;

        duneTile = Instantiate(prefabDuneTile);
        mountainTile = Instantiate(prefabMountainTile);
        flatSandTile = Instantiate(prefabFlatSandTile);
        plateauTile = Instantiate(prefabPlateauTile);
        cityTile = Instantiate(prefabCityTile);

        duneTile.transform.parent = this.transform;
        mountainTile.transform.parent = this.transform;
        flatSandTile.transform.parent = this.transform;
        plateauTile.transform.parent = this.transform;
        cityTile.transform.parent = this.transform;

        model = new TileModel(FieldType.DUNE, indexX, indexY);
    }

    public FieldType fieldType {
        set{ 
            FieldType fieldType = value;
            switch (fieldType) {
                case FieldType.DUNE:
                    duneTile.SetActive(true);
                    flatSandTile.SetActive(false);
                    mountainTile.SetActive(false);
                    plateauTile.SetActive(false);
                    cityTile.SetActive(false);

                    activeTile = flatSandTile;
                    break;
                case FieldType.FLAT_SAND:
                    flatSandTile.SetActive(true);
                    duneTile.SetActive(false);
                    mountainTile.SetActive(false);
                    plateauTile.SetActive(false);
                    cityTile.SetActive(false);

                    activeTile = duneTile;
                    break;
                case FieldType.MOUNTAINS:
                    mountainTile.SetActive(true);
                    flatSandTile.SetActive(false);
                    duneTile.SetActive(false);
                    plateauTile.SetActive(false);
                    cityTile.SetActive(false);

                    activeTile = mountainTile;
                    break;
                case FieldType.CITY:
                    cityTile.SetActive(true);
                    flatSandTile.SetActive(false);
                    mountainTile.SetActive(false);
                    plateauTile.SetActive(false);
                    duneTile.SetActive(false);

                    activeTile = cityTile;
                    break;
                case FieldType.PLATEAU:
                    plateauTile.SetActive(true);
                    flatSandTile.SetActive(false);
                    mountainTile.SetActive(false);
                    duneTile.SetActive(false);
                    cityTile.SetActive(false);

                    activeTile = plateauTile;
                    break;
            }

            // Change the data model
            this.model.fieldType = fieldType;
        }

        get {
            return this.model.fieldType;
        } 
        
    }

    public int indexX {
        set {
            int indexX = value;
            transform.position = new Vector3(indexX + shift.x, transform.position.y, this.model.indexY + shift.y);
            model.indexX = indexX;
        }

        get {
            return this.model.indexX;
        }
    }

    public int indexY {
        set {
            int indexY = value;
            transform.position = new Vector3(this.model.indexX + shift.x, transform.position.y, indexY + shift.y);
            this.model.indexY = indexY;
        }

        get {
            return this.model.indexY;
        }
    }

    public List<TileInterface> neighbours { get; set; }
    public CharacterInterface characterOnTile { get; set; }
    public bool hasSandstorm { get; set; }
    public bool isSandstormEye { get; set; }
    public int spiceAmount { get; set; }
    public bool hasSandworm { get; set; }

    public void setShift(Vector2 shift) {
        this.shift = shift;
        transform.position = new Vector3(this.model.indexX + shift.x, transform.position.y, this.model.indexY + shift.y);
    }

}


