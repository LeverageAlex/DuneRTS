using UnityEngine;
using System.Collections.Generic;

public class MapModel {

    private int sizeX;
    private int sizeY;

    public ScenarioConfigModel szenarioConfig { get; set; }

    public TileModel[,] tiles;


    public MapModel(int sizeX, int sizeY) {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        this.tiles = new TileModel[sizeY, sizeX];
    }

    public int getDimensionX() {
        return this.sizeX;
    }

    public int getDimensionY() {
        return this.sizeY;
    }

    public void setTileAtPosition(int x, int y, TileModel tile) {
        this.tiles[y,x] = tile;
    }

    public TileModel getTileAtPosition(int x, int y) {
        return this.tiles[y,x];
    }

    
    public override string ToString() {
        return tiles.ToString();
    }
}