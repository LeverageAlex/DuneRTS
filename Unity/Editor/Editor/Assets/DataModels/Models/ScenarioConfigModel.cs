using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioConfigModel {

    public FieldType[,] map{get;}


    public ScenarioConfigModel(FieldType[,] map) {
        this.map = map;
    }

    public ScenarioConfigModel() { }


    public int getDimensionX() {
        return map.GetLength(1);
    }

    public int getDimensionY() {
        return map.GetLength(0);
    }

    public void setMap(int posX, int posY, FieldType fieldType) {
        map[posY, posX] = fieldType;
    }
}
