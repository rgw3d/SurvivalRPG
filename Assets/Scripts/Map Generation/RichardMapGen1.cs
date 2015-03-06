using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RichardMapGen1 : MonoBehaviour, MapGenInterface {


    public GameObject walkable;
    public GameObject background;

    public int mapWidth = 100;
    public int numberOfRooms = 5;
    public int minimumWidthOfRoom = 3;
    public int maximumWidthOfRoom = 6;
    public int minimumHeightOfRoom = 3;
    public int maximumHeightOfRoom = 6;

    public int minimumDistanceBetweenNodes = 15;
    public List<GameObject> Players;

    void Start() {
        if (Players == null) {//then find the player
            Players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        }

        List<Tile> roomNodes = initRoomNodes();
        createRooms(roomNodes);
    }
    

    /*
     * This method will be used to return the list of all the centers of rooms 
     * All the centers of the rooms are nodes, and they are returned in a 
     * list of Vector2s, containing the x and y coordinate
     */
    public List<Tile> initRoomNodes() {

        List<Tile> nodeList = initBaseNode();

        return new List<Tile>();
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
        return new GameObject[width, width];

    }


    /*
     * This should fill the background 
     * 
     */
    public GameObject[,] fillBackground(GameObject[,] tileArray) {
        return new GameObject[width, width];
    
    }

    
    


	
}
