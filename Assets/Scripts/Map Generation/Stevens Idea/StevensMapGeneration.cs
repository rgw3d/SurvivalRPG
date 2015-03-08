﻿using UnityEngine;
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
		createCorridors();
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

	void createCorridors(){
		foreach(StevensRoom r1 in map.roomList){
			if(!r1.isConnected){
				StevensRoom r2 = findNearestConnectedRoom(r1);
				int r1X = Random.Range(r1.rLeft, r1.rRight + 1);
				int r1Y = Random.Range(r1.rBottom, r1.rTop + 1);
				int r2X = Random.Range(r2.rLeft, r2.rRight + 1);
				int r2Y = Random.Range(r2.rBottom, r2.rTop + 1);

				while(r1X != r2X){
					map.mapTiles[r1X,r1Y].tileType = StevensTile.TileType.red;

					r1X += (r1X < r2X) ? 1 : -1;  
				}
				while(r1Y != r2Y){
					map.mapTiles[r1X,r1Y].tileType = StevensTile.TileType.red;
					
					r1Y += (r1Y < r2Y) ? 1 : -1;  
				}
				r1.isConnected = true;
			}
		}
	}

	StevensRoom findNearestConnectedRoom(StevensRoom r1){
		List<StevensRoom> connectedRooms = new List<StevensRoom>();
		foreach(StevensRoom conRoom in map.roomList){
			if(r1 != conRoom && !conRoom.isConnected){
				connectedRooms.Add(conRoom);
			}
		}
		StevensRoom nearestRoom = r1;
		float closestDistance = Mathf.Infinity;
		foreach(StevensRoom r2 in connectedRooms){
			if(r1.distanceToRoom(r2) < closestDistance){
				closestDistance = r1.distanceToRoom(r2);
				nearestRoom = r2;
			}
		}
		return nearestRoom;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

