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

	public int numRooms = 10;
	public int minRoomWidth = 4;
	public int maxRoomWidth = 8;
	public int minRoomHeight = 4;
	public int maxRoomHeight = 8;
	public int numTriesToMakeRooms = 20;
	public int roomIntersectionOffset = 1;

	public GameObject player;

	// Use this for initialization
	void Start () {

		map = new StevensMap();
		map.mapTiles = new StevensTile[mapWidth, mapHeight];

		createMap(mapWidth, mapHeight);
		createInitialRoom();
		createRooms(numRooms);
	}

	void createMap(int width, int length){
		for(int y = 0; y < length; y++){
			for(int x = 0; x < width; x++){
				map.mapTiles[x,y] = new StevensTile(StevensTile.TileType.white);
			}
		}
	}

	void createInitialRoom(){ // probably obsolete once we determine how we want the player to spawn in
		int playerX = Mathf.FloorToInt(player.transform.position.x - .5f);
		int playerY = Mathf.FloorToInt(player.transform.position.y - .5f);

		map.mapTiles[playerX,playerY].setTileType(StevensTile.TileType.red);

	}

	void createRooms(int numberOfRooms){
		int maxTries = numTriesToMakeRooms;
		for(int tries = 0; tries < maxTries; tries++){
			int roomWidth = Random.Range(minRoomWidth,maxRoomWidth);
			int roomHeight = Random.Range(minRoomHeight,maxRoomHeight);
			int roomLocX = Random.Range(0,mapWidth - roomWidth);
			int roomLocY = Random.Range(0,mapHeight - roomHeight);
			StevensRoom basicRoom = new StevensRoom(roomLocY,roomLocX,roomLocY + roomHeight,roomLocX + roomWidth);

			bool intersected = false;
			foreach(StevensRoom otherRoom in map.roomList){
				if(basicRoom.roomIntersectsWith(otherRoom, roomIntersectionOffset)){
					intersected = true;
				}
			}

			if(!intersected){
				map.roomList.Add(basicRoom);
				for(int y = basicRoom.rBottom; y <= basicRoom.rTop; y++){
					for(int x = basicRoom.rLeft; x <= basicRoom.rRight; x++){
						map.mapTiles[x,y].tileType = StevensTile.TileType.red;
					}
				}
				numberOfRooms--;
			}
			if(numberOfRooms <= 0){
				break;
			}
		}
		if(numberOfRooms > 0){
			Debug.Log("Couldn't place all the rooms!");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}

