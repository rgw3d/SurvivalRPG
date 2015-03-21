using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMap {

	public StevensTile[,] mapTiles;
	public List<StevensRoom> roomList;
    public readonly float mapWidth;
    public readonly float mapHeight;
	
	public StevensMap(int width, int height){
        mapWidth = width;
        mapHeight = height;
        mapTiles = new StevensTile[width, height];
		roomList = new List<StevensRoom>();
	}

    public string SerializeToSend() {
        string serializedValue = "";

        

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
