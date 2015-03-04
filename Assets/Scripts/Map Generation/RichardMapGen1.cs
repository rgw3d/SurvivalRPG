using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RichardMapGen1 : MonoBehaviour, MapGenInterface {


    public GameObject walkable;
    public GameObject background;

    public int width = 100;
    public int roomNumber = 5;
    public int roomWidth = 3;
    public int minimumDistanceBetweenRooms = 10;

    void Start() {
        GameObject[,] tileArray = new GameObject[width, width];
        List<Tile> roomNodes = initRoomNodes();
        createRooms(roomNodes);
    }
    

    /*
     * This method will be used to return the list of all the centers of rooms 
     * All the centers of the rooms are nodes, and they are returned in a 
     * list of Vector2s, containing the x and y coordinate
     */
    public List<Tile> initRoomNodes() {

        return new List<Tile>();
    }

    /*
   * This method will draw a room around the player. 
   * Player must start in room.
   * duh
   */
    public List<Tile> initBaseNode(Tile location) {

        return new List<Tile>();
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
