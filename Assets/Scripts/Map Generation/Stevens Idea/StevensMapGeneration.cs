using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapGeneration : MonoBehaviour {


	//"Tile" refers to a singular background object such as a theoretical 'TopRightCorner of a Red background at (4,7)'
	//		Tiles are also the objects that we use when referring to this singular location
	//"Room" refers to a grouping of Tiles that, when combined, create a sort of "Room" for the player to be in
	//"Corridor" refers to the Tiles inbetween Rooms that connect the Rooms together and allow the player to traverse between Rooms
	//"Node" refers to the center of a Room. Not an actual visible thing, but rather something that is used for the random generation


	//MapGen:
	//CreateInitialNode:
	//		CreateNodes:
	//		CreateRooms:
	//		CreateCorridors:
	//		(EstablishTileSubtypes?)
	//Return Map to MapRenderer

	public StevensTile[,] map;
	public List<StevensRoom> roomList;

	public int mapWidth = 30;
	public int mapHeight = 30;

	// Use this for initialization
	void Start () {
		map = new StevensTile[mapWidth, mapHeight];
		roomList = new List<StevensRoom>();

		createMap(mapWidth, mapHeight);
		
	}

	void createMap(int width, int length){
		for(int y = 0; y < length; y++){
			for(int x = 0; x < width; x++){
				map[x,y] = new StevensTile(StevensTile.TileType.white);
			}
		}
		map[13,13] = new StevensTile(StevensTile.TileType.red);
	}



	// Update is called once per frame
	void Update () {
		
	}
}

