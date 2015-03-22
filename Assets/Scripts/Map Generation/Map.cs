using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

    public MapTile[,] mapTiles;
    public List<MapRoom> roomList = new List<MapRoom>();
    public readonly int mapWidth;
    public readonly int mapHeight;

    public Map(int width, int height) {
        mapWidth = width;
        mapHeight = height;
        mapTiles = new MapTile[width, height];
    }

    public Map(int width, int height, MapTile[,] tiles) {
        mapWidth = width;
        mapHeight = height;
        mapTiles = tiles;
    }

    public string SerializeMapTiles() {
        string serializedValue = "";
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapHeight; x++) {
                serializedValue += (int)mapTiles[x, y]._tileType + "";//hashcode of the tile (what TileType it is) and a cast to string
            }
        }
        Debug.Log(serializedValue);
        return serializedValue;
    }

    public override int GetHashCode() {
        int hashCode = 7 * mapTiles.GetHashCode();
        hashCode += roomList.GetHashCode();
        hashCode += mapWidth.GetHashCode();
        hashCode += mapHeight.GetHashCode();

        return hashCode;
    }
}
