﻿using UnityEngine;
using System.Collections;

public class RichardMapGen1 : MonoBehaviour, MapGenInterface {


    void Start() {

    }


    /*
    * This method will draw a room around the player. 
    * Player must start in room.
    * duh
    */
    Vector2 initBaseNode();

    /*
     * This method will be used to return the list of all the centers of rooms 
     * All the centers of the rooms are nodes, and they are returned in a 
     * list of Vector2s, containing the x and y coordinate
     */
    List<Vector2> initRoomNodes();


    /*
     * This method will create the rooms and return them grouped together in a list stored inside a list
     * 
     */ 
    List<List<Vector2>> createRooms();

    /*
     * This will draw all the rooms based on the locations that it recieves
     * 
     */ 
    void drawRooms(List<List<Vector2>> roomsCoordinates);


    /*
     * This should fill the background 
     * 
     */ 
    void fillBackground();


	
}
