using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface StevensMapGenInterface {


	/*
	* We need a map to actually create these things on
	*/
	void createMap(int width, int length);

    /*
    * This method will draw a room around the player. 
    * Player must start in room.
    * duh
    */
	void createInitialRoom();

 
    /*
     * This method will create the rooms 
     * 
     */ 
	void createRooms(int numberOfRooms);

    /*
     * This will create all the corridors between the rooms
     * 
     */ 
	void createCorridors();


    /*
     * This method causes the map to turn any "blank"(white) tile into a wall based on where it is relative to a floor tile.
     * 
     */ 
	void createWalls();
}