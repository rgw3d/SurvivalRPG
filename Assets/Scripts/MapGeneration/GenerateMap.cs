using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : Photon.MonoBehaviour {


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
    //Return Map to MapRenderer

    public static Map Map;

    public int MapWidth = 100;
    public int MapHeight = 100;
    public int NumberOfRooms = 20;
    public int MinimumRoomWidth = 5;
    public int MaximumRoomWidth = 12;
    public int MinimumRoomHeight = 5;
    public int MaximumRoomHeight = 12;
    public int NumberTriesToGenRooms = 150;
    public int RoomIntersectionOffset = 1;

    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    void Start() {
        DelegateHolder.OnGenerateAndRenderMap += GenerateAndDisplayMap;
    }

    public void GenerateAndDisplayMap() {
        generateMap();
        DelegateHolder.TriggerMapGenerated(true);//assume that if the map is generated this way, then it is the host
    }

    public void generateMap() {
        Map = new Map(MapWidth, MapHeight);
        createMap();
        createRooms();
        createInitialRoom();
        createCorridors();
        createWalls();
        createGoal();
    }

    public void createMap() {//This fills the entire map with white tiles (blank tiles)
        for (int y = 0; y < MapHeight; y++) {
            for (int x = 0; x < MapWidth; x++) {
                Map.mapTiles[x, y] = new MapTile(MapTile.TileType.white, x, y);
            }
        }
    }


    public void createRooms() {//this creates the rooms, 
        int maxTries = NumberTriesToGenRooms;//and also changes the tiles to have the appropriate color (from white to red)
        int numberOfRooms = NumberOfRooms;
        for (int tries = 0; tries < maxTries; tries++) {
            int roomWidth = Random.Range(MinimumRoomWidth, MaximumRoomWidth);
            int roomHeight = Random.Range(MinimumRoomHeight, MaximumRoomHeight);
            int roomX = Random.Range(1, MapWidth - roomWidth - 1);
            int roomY = Random.Range(1, MapHeight - roomHeight - 1);
            MapRoom basicRoom = new MapRoom(roomX, roomY, roomX + roomWidth, roomY + roomHeight);

            bool intersected = false;
            foreach (MapRoom otherRoom in Map.roomList) {
                if (basicRoom.IntersectsWith(otherRoom, RoomIntersectionOffset)) {
                    intersected = true;
                }
            }

            if (!intersected) {
                Map.roomList.Add(basicRoom);
                for (int y = basicRoom.BottomY; y <= basicRoom.TopY; y++) {
                    for (int x = basicRoom.LeftX; x <= basicRoom.RightX; x++) {
                        Map.mapTiles[x, y].SetTileType(MapTile.TileType.red);
                    }
                }
                numberOfRooms--;
            }
            if (numberOfRooms <= 0) {
                break;
            }
        }
        if (numberOfRooms > 0) {
            Debug.Log("Couldn't place all the rooms!");
        }
    }

    public void createInitialRoom() { // probably obsolete once we determine how we want the player to spawn in

        photonView.RPC("PlacePlayer", PhotonTargets.OthersBuffered, Map.roomList[0].GetCenter());
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Map.roomList[0].GetCenter(), Quaternion.identity, 0);
        GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
        playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
        playerCamera.transform.localPosition = new Vector3(0, 0, -10);

    }

    [RPC]
    void PlacePlayer(Vector2 position) {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(position.x, position.y), Quaternion.identity, 0);
        GameObject playerCamera = Instantiate(cameraPrefab) as GameObject;
        playerCamera.transform.parent = player.transform;//set the camera to be a child of the player
        playerCamera.transform.localPosition = new Vector3(0, 0, -10);

    }

    public void createCorridors() {
        foreach (MapRoom r1 in Map.roomList) {
            if (!r1.isConnected) {
                MapRoom r2 = FindNearestNonConnectedRoom(r1);
                int r1X = Random.Range(r1.LeftX, r1.RightX + 1);
                int r1Y = Random.Range(r1.BottomY, r1.TopY + 1);
                int r2X = Random.Range(r2.LeftX, r2.RightX + 1);
                int r2Y = Random.Range(r2.BottomY, r2.TopY + 1);

                while (r1X != r2X) {
                    Map.mapTiles[r1X, r1Y].SetTileType(MapTile.TileType.red);
                    r1X += (r1X < r2X) ? 1 : -1;
                }
                while (r1Y != r2Y) {
                    Map.mapTiles[r1X, r1Y].SetTileType(MapTile.TileType.red);
                    r1Y += (r1Y < r2Y) ? 1 : -1;
                }
                r1.isConnected = true;
            }
        }
    }

    MapRoom FindNearestNonConnectedRoom(MapRoom r1) {
        List<MapRoom> notConnectedRooms = new List<MapRoom>();
        foreach (MapRoom notConRoom in Map.roomList) {
            if (r1 != notConRoom && !notConRoom.isConnected) {
                notConnectedRooms.Add(notConRoom);
            }
        }
        MapRoom nearestRoom = r1;
        float closestDistance = Mathf.Infinity;
        foreach (MapRoom r2 in notConnectedRooms) {
            if (r1.DistanceToRoom(r2) < closestDistance) {
                closestDistance = r1.DistanceToRoom(r2);
                nearestRoom = r2;
            }
        }
        return nearestRoom;
    }

    public void createWalls() {
        for (int y = 0; y < MapHeight; y++) {
            for (int x = 0; x < MapWidth; x++) {
                MapTile tile = Map.mapTiles[x, y];
                if (tile.GetTileType() == MapTile.TileType.red) {
                    if (Map.mapTiles[x - 1, y].GetTileType() == MapTile.TileType.white)//left
                        Map.mapTiles[x - 1, y].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x - 1, y + 1].GetTileType() == MapTile.TileType.white)//above left
                        Map.mapTiles[x - 1, y + 1].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x, y + 1].GetTileType() == MapTile.TileType.white)//above
                        Map.mapTiles[x, y + 1].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x + 1, y + 1].GetTileType() == MapTile.TileType.white)//above right
                        Map.mapTiles[x + 1, y + 1].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x + 1, y].GetTileType() == MapTile.TileType.white)//right
                        Map.mapTiles[x + 1, y].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x + 1, y - 1].GetTileType() == MapTile.TileType.white)//below right
                        Map.mapTiles[x + 1, y - 1].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x, y - 1].GetTileType() == MapTile.TileType.white)//below
                        Map.mapTiles[x, y - 1].SetTileType(MapTile.TileType.blue);

                    if (Map.mapTiles[x - 1, y - 1].GetTileType() == MapTile.TileType.white)//below left
                        Map.mapTiles[x - 1, y - 1].SetTileType(MapTile.TileType.blue);
                }
            }
        }
    }

    public void createGoal() {
        Vector2 center = Map.roomList[Map.roomList.Count - 1].GetCenter();
        Debug.Log("Center: " + center.x + "  " + center.y);
        Map.mapTiles[(int)center.x, (int)center.y].SetTileType(MapTile.TileType.green);
    }

}

