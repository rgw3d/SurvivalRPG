using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RichardMapGen1 : MonoBehaviour, MapGenInterface {


    public GameObject walkable;
    public GameObject background;

    public int mapWidth = 100;
    public int mapHeight = 100;
    public int numberOfClusters = 5;
    public int numberOfRoomsInClusters = 3;
    public int interClusterRange = 2;
    public int minimumWidthOfRoom = 3;
    public int maximumWidthOfRoom = 6;
    public int minimumHeightOfRoom = 3;
    public int maximumHeightOfRoom = 6;

    public int nodeCreationAttempts = 50;

    public int minimumDistanceBetweenNodes = 20;
    public List<GameObject> Players;

    void Start() {
        if (Players == null) {//then find the player
            Players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        }

        List<Tile> roomNodes = initRoomNodes();
        List<List<Tile>> roomCoords = createRooms(roomNodes);
    }
    

    /*
     * This method will be used to return the list of all the centers of rooms 
     * All the centers of the rooms are nodes, and they are returned in a 
     * list of Vector2s, containing the x and y coordinate
     */
    public List<Tile> initRoomNodes() {

        List<Tile> nodeList = initBaseNode();

        /*
         * Essentially I will be making a small cluster of nodes that are spread out and then go from there
         * 
         */
        
        int tries = 0;
        for (int i = 0; i < numberOfClusters && tries<nodeCreationAttempts; i++) {
            Tile pos = new Tile(Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1));//get a random position
            bool farEnoughAway = true;//we need to teset to make sure that there is room
            foreach (Tile node in nodeList) {
                if (Mathf.Pow(Mathf.Pow(node.x - pos.x, 2) + Mathf.Pow(node.y - pos.y, 2), 0.5f) < minimumDistanceBetweenNodes) {
                    farEnoughAway = false;
                    tries++;
                    break;
                }
            }
            if (farEnoughAway) {
                nodeList.Add(pos);
                List<Tile> tmpTiles = new List<Tile>();
                int extraNodeCreationAttempts = 10;
                int extraNodeTries = 0;
                for (int j = 0; j < numberOfRoomsInClusters && extraNodeTries < extraNodeCreationAttempts; j++) {
                    Tile extraPos = new Tile(pos.x + Random.Range(-interClusterRange, interClusterRange), pos.y + Random.Range(-interClusterRange, interClusterRange));
                    bool canAddExtraNode = true;
                    foreach (Tile node in tmpTiles) {
                        if (extraPos.x == node.x && extraPos.y == node.y) {
                            canAddExtraNode = false;
                            extraNodeTries++;
                            break;
                        }
                    }
                    if (canAddExtraNode) {
                        tmpTiles.Add(extraPos);
                        nodeList.Add(extraPos);
                    }
                }
            }
        }


        return nodeList;
    }

    /*
   * This method will draw a room around the player. 
   * Player must start in room.
   * duh
   */
    public List<Tile> initBaseNode() {
        List<Tile> nodeList = new List<Tile>();
        foreach (GameObject player in Players) {
            nodeList.Add(new Tile(player.transform.localPosition.x, player.transform.localPosition.y));
        }

        return nodeList;
    }


    /*
     * This method will create the rooms and return them grouped together in a list stored inside a list
     * 
     */
    public List<List<Tile>> createRooms(List<Tile> roomNodes) {
        return new List<List<Tile>>();
    }


    /*
     * This will draw all the rooms based on the locations that it recieves
     * 
     */
    public GameObject[,] drawRooms(List<List<Tile>> roomsCoordinates) {
        return new GameObject[mapWidth,mapHeight];

    }


    /*
     * This should fill the background 
     * 
     */
    public GameObject[,] fillBackground(GameObject[,] tileArray) {
        return new GameObject[width, width];
    
    }

    
    


	
}
