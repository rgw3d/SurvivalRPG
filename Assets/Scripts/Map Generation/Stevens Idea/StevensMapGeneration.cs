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

	public StevensMap map;
	
	public int mapWidth = 30;
	public int mapHeight = 30;

	public GameObject player;

	// Use this for initialization
	void Start () {

		map = new StevensMap();
		map.mapTiles = new StevensTile[mapWidth, mapHeight];

		createMap(mapWidth, mapHeight);
		createInitialRoom();
	}

	void createMap(int width, int length){
		for(int y = 0; y < length; y++){
			for(int x = 0; x < width; x++){
				map.mapTiles[x,y] = new StevensTile(StevensTile.TileType.white);
			}
		}
	}

	void createInitialRoom(){
		int playerX = Mathf.FloorToInt(player.transform.position.x - .5f);
		int playerY = Mathf.FloorToInt(player.transform.position.y - .5f);

		map.mapTiles[playerX,playerY].setTileType(StevensTile.TileType.red);
	}


	// Update is called once per frame
	void Update () {
		
	}
}

