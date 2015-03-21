using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMap {

	public StevensTile[,] mapTiles;
	public List<StevensRoom> roomList = new List<StevensRoom>();
    public readonly int mapWidth;
    public readonly int mapHeight;
	
	public StevensMap(int width, int height){
        mapWidth = width;
        mapHeight = height;
        mapTiles = new StevensTile[width, height];
	}

    public StevensMap(int width, int height, StevensTile[,] tiles) {
        mapWidth = width;
        mapHeight = height;
        mapTiles = tiles;
    }

    public string SerializeMapTiles() {
        string serializedValue = "";
        for(int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapHeight; x++)
                serializedValue += mapTiles[x,y].GetHashCode() + "";//hashcode of the tile (what TileType it is) and a cast to string
        }
        
        return serializedValue;
    }

    public int GetHashCode() {
        int hashCode = 7 * mapTiles.GetHashCode();
        hashCode += roomList.GetHashCode();
        hashCode += mapWidth.GetHashCode();
        hashCode += mapHeight.GetHashCode();

        return hashCode;
    }
}
