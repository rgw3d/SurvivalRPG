using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapGeneration : Photon.MonoBehaviour {


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

	public StevensMap Map;
    public StevensMapRenderer stevensMapRenderer;
	
	public int mapWidth = 30;
	public int mapHeight = 30;

	public int numRooms = 10;
	public int minRoomWidth = 4;
	public int maxRoomWidth = 8;
	public int minRoomHeight = 4;
	public int maxRoomHeight = 8;
	public int numTriesToMakeRooms = 20;
	public int roomIntersectionOffset = 1;

	public GameObject playerPrefab;
    public GameObject cameraPrefab;
    

	void Start () {
        if (stevensMapRenderer == null) {
            stevensMapRenderer = GetComponent<StevensMapRenderer>();
        }
	}

    public void GenerateAndDisplayMap() {
        generateMap();
        stevensMapRenderer.reRenderMap();
    }

	public void generateMap(){
		Map = new StevensMap(mapWidth,mapHeight);
		createMap();
		createRooms();
		createInitialRoom();
		createCorridors();
		createWalls();
		createGoal();
	}

	public void createMap(){//This fills the entire map with white tiles (blank tiles)
		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x < mapWidth; x++){
				Map.mapTiles[x,y] = new StevensTile(StevensTile.TileType.white);
			}
		}
	}
	

	public void createRooms(){//this creates the rooms, 
        int maxTries = numTriesToMakeRooms;//and also changes the tiles to have the appropriate color (from white to red)
        int numberOfRooms = numRooms;
		for(int tries = 0; tries < maxTries; tries++){
			int roomWidth = Random.Range(minRoomWidth,maxRoomWidth);
			int roomHeight = Random.Range(minRoomHeight,maxRoomHeight);
			int roomLocX = Random.Range(1,mapWidth - roomWidth - 1);
			int roomLocY = Random.Range(1,mapHeight - roomHeight - 1);
			StevensRoom basicRoom = new StevensRoom(roomLocY,roomLocX,roomLocY + roomHeight,roomLocX + roomWidth);

			bool intersected = false;
			foreach(StevensRoom otherRoom in Map.roomList){
				if(basicRoom.roomIntersectsWith(otherRoom, roomIntersectionOffset)){
					intersected = true;
				}
			}

			if(!intersected){
				Map.roomList.Add(basicRoom);
				for(int y = basicRoom.rBottom; y <= basicRoom.rTop; y++){
					for(int x = basicRoom.rLeft; x <= basicRoom.rRight; x++){
						Map.mapTiles[x,y].tileType = StevensTile.TileType.red;
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

    public void createInitialRoom() { // probably obsolete once we determine how we want the player to spawn in

        int x = Mathf.FloorToInt(Map.roomList[0].rLeft + ((Map.roomList[0].rRight - Map.roomList[0].rLeft) / 2));
        int y = Mathf.FloorToInt(Map.roomList[0].rBottom + ((Map.roomList[0].rTop - Map.roomList[0].rBottom) / 2));
        photonView.RPC("spawnPosition", PhotonTargets.OthersBuffered, new Vector2(x, y));
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(x, y), Quaternion.identity, 0);
        GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
        playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
        playerCamera.transform.localPosition = new Vector3(0, 0,-10);


    }

    [RPC]
    void spawnPosition(Vector2 position) {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(position.x, position.y), Quaternion.identity, 0);
        GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
        playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
        playerCamera.transform.localPosition = new Vector3(0, 0,-10);

    }

	public void createCorridors(){
		foreach(StevensRoom r1 in Map.roomList){
			if(!r1.isConnected){
				StevensRoom r2 = findNearestNotConnectedRoom(r1);
				int r1X = Random.Range(r1.rLeft, r1.rRight + 1);
				int r1Y = Random.Range(r1.rBottom, r1.rTop + 1);
				int r2X = Random.Range(r2.rLeft, r2.rRight + 1);
				int r2Y = Random.Range(r2.rBottom, r2.rTop + 1);

				while(r1X != r2X){
					Map.mapTiles[r1X,r1Y].tileType = StevensTile.TileType.red;

					r1X += (r1X < r2X) ? 1 : -1;  
				}
				while(r1Y != r2Y){
					Map.mapTiles[r1X,r1Y].tileType = StevensTile.TileType.red;
					
					r1Y += (r1Y < r2Y) ? 1 : -1;  
				}
				r1.isConnected = true;
			}
		}
	}

	StevensRoom findNearestNotConnectedRoom(StevensRoom r1){
		List<StevensRoom> notConnectedRooms = new List<StevensRoom>();
		foreach(StevensRoom notConRoom in Map.roomList){
			if(r1 != notConRoom && !notConRoom.isConnected){
				notConnectedRooms.Add(notConRoom);
			}
		}
		StevensRoom nearestRoom = r1;
		float closestDistance = Mathf.Infinity;
		foreach(StevensRoom r2 in notConnectedRooms){
			if(r1.distanceToRoom(r2) < closestDistance){
				closestDistance = r1.distanceToRoom(r2);
				nearestRoom = r2;
			}
		}
		return nearestRoom;
	}

	public void createWalls(){
		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x < mapWidth; x++){
				StevensTile tile = Map.mapTiles[x,y];
				if(tile.tileType == StevensTile.TileType.red){
					if(Map.mapTiles[x-1,y].tileType == StevensTile.TileType.white)//left
						Map.mapTiles[x-1,y].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x-1,y+1].tileType == StevensTile.TileType.white)//above left
						Map.mapTiles[x-1,y+1].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x,y+1].tileType == StevensTile.TileType.white)//above
						Map.mapTiles[x,y+1].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x+1,y+1].tileType == StevensTile.TileType.white)//above right
						Map.mapTiles[x+1,y+1].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x+1,y].tileType == StevensTile.TileType.white)//right
						Map.mapTiles[x+1,y].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x+1,y-1].tileType == StevensTile.TileType.white)//below right
						Map.mapTiles[x+1,y-1].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x,y-1].tileType == StevensTile.TileType.white)//below
						Map.mapTiles[x,y-1].tileType = StevensTile.TileType.blue;

					if(Map.mapTiles[x-1,y-1].tileType == StevensTile.TileType.white)//below left
						Map.mapTiles[x-1,y-1].tileType = StevensTile.TileType.blue;
				}
			}
		}
	}

	public void createGoal(){
		int last = Map.roomList.Count - 1;
		int x = Mathf.FloorToInt(Map.roomList[last].rLeft + ((Map.roomList[last].rRight - Map.roomList[last].rLeft) / 2));
		int y = Mathf.FloorToInt(Map.roomList[last].rBottom + ((Map.roomList[last].rTop - Map.roomList[last].rBottom) / 2));

		Map.mapTiles[x,y].tileType = StevensTile.TileType.green;
	}

}

