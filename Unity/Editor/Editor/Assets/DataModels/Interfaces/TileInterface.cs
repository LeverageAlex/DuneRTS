using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TileInterface {
    FieldType fieldType { get; set; }
    int indexX { get; set; }
    int indexY { get; set; }
    List<TileInterface> neighbours { get; set; }
    CharacterInterface characterOnTile { get; set; }
    bool hasSandstorm { get; set; }
    bool isSandstormEye { get; set; }
    int spiceAmount { get; set; }
    bool hasSandworm { get; set; }
}
