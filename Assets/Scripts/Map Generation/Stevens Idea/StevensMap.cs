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

    public string SerializeToSend() {
        string serializedValue = GetHashCode()+" ";//hashcode and a space
        serializedValue += mapWidth+" ";//map width and a space
        serializedValue += mapHeight + " ";//map height and a space

        

        return "wow";
    }

    public int GetHashCode() {
        int hashCode = mapTiles.GetHashCode();
        hashCode += roomList.GetHashCode();
        hashCode += mapWidth.GetHashCode();
        hashCode += mapHeight.GetHashCode();

        return hashCode;
    }
}
