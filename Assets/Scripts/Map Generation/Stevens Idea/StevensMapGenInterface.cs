using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface StevensMapGenInterface {


    /*
    * This method will draw a room around the player. 
    * Player must start in room.
    * duh
    */
    List<Tile> initBaseNode(List<Tile> tileList);

    /*
     * This method will be used to return the list of all the centers of rooms 
     * All the centers of the rooms are nodes, and they are returned in a 
     * list of Vector2s, containing the x and y coordinate
     */
    List<Tile> initRoomNodes();


    /*
     * This method will create the rooms and return them grouped together in a list stored inside a list
     * 
     */ 
    List<List<Tile>> createRooms();

    /*
     * This will draw all the rooms based on the locations that it recieves
     * 
     */ 
    void drawRooms(List<List<Tile>> roomsCoordinates);


    /*
     * This should fill the background 
     * 
     */ 
    void fillBackground();
    
    }







