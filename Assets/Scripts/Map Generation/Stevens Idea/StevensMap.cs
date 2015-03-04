using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMap {

	public StevensTile[,] mapTiles;
	public List<StevensRoom> roomList;
	
	public StevensMap(){

		roomList = new List<StevensRoom>();
	}
}
