using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel : TileInterface {
    public FieldType fieldType { get; set; }

    public int indexX { get; set; }
    public int indexY { get; set; }
    public List<TileInterface> neighbours { get; set; }

    public CharacterInterface characterOnTile { get; set; }
    public bool hasSandstorm { get; set; }
    public bool isSandstormEye { get; set; }
    public int spiceAmount { get; set; }
    public bool hasSandworm { get; set; }



    public TileModel(FieldType fieldType, int indexX, int indexY) {
        this.fieldType = fieldType;
        this.indexX = indexX;
        this.indexY = indexY;

        this.neighbours = null;
        this.hasSandstorm = false;
        this.isSandstormEye = false;
        this.spiceAmount = 0;
        this.hasSandstorm = false;
    }

}
