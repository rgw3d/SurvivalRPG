using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapScript : MonoBehaviour, MapGenInterface {
	
	public GameObject walkable;
	public GameObject background;
	
	public int width = 100;

	public int startingRoomWidth = 3;

	public int roomNumber = 5;
	public int roomWidth = 3;
	public int minimumDistanceBetweenRooms = 10;
	
	
	private GameObject[,] spriteArray;

	//"Tile" refers to a singular background object such as a theoretical 'TopRightCorner of a Red background at (4,7)'
	//		Tiles are also the objects that we use when referring to this singular location
	//"Room" refers to a grouping of Tiles that, when combined, create a sort of "Room" for the player to be in
	//"Corridor" refers to the Tiles inbetween Rooms that connect the Rooms together and allow the player to traverse between Rooms
	//"Node" refers to the center of a Room. Not an actual visible thing, but rather something that is used for the random generation


	// Use this for initialization
	void Start () {
		spriteArray = new GameObject[width,width];

		List<Tile> tileList = new List<Tile>();
		tileList = initRoomNodes(tileList);
		tileList = initCorridors(tileList); // we probably need to have two separate lists somehow, one for all the nodes,
		tileList = initRooms(tileList);		// and another for all the tiles. Combining the two is tricky because we need the nodes to create
		drawTiles(tileList);				// the rooms, but don't want the rooms for creating the corridors
	}
	
	List<Tile> initRoomNodes(List<Tile> nodes){

		nodes = initBaseNode(nodes);
		nodes = addRandomRoomNodes(nodes);
		return nodes;
	}

	Tile initBaseNode(List<Tile> nodes){
		nodes.Add(new Tile(1f,1f)); // change later to start wherever the player begins the level
		return nodes;
	}

	List<Tile> addRandomRoomNodes(List<Tile> nodes){
		nodes.Add(new Tile(Random.Range((int)startingRoomWidth, width-1), Random.Range((int)startingRoomWidth, width-1)));
		return nodes;
	}

	List<Tile> initCorridors(List<Tile> nodes){ // adds "corridor" nodes to connect rooms to 1,1
		List<Tile> tmpNodes = new List<Tile>(nodes); // needed so that error isn't thrown about modifying a list that's being for-each'd
		foreach(Tile tile in tmpNodes){
			if(tile.x != 1f && tile.y != 1f){
				if(!checkAdjacent(tile, new Tile(1f, 1f))){
					for(int x = 1; x < (int)tile.x ; x++){
						nodes.Add(new Tile((float)x, tile.y));
					}
					for(int y = 1; y < (int)tile.y ; y++){
						nodes.Add(new Tile(1f , (float)y + 1f));
					}
				}
			}
		}
		return nodes;
	}
	
	bool checkAdjacent(Tile tile, Tile start){ // checks if tile is direct up, down, left, or right of start
		if(tile.x + 1 == start.x && tile.y == start.y){
			return true;
		}
		else if(tile.x - 1 == start.x && tile.y == start.y){
			return true;
		}
		else if(tile.x == start.x && tile.y + 1 == start.y){
			return true;
		}
		else if(tile.x == start.x && tile.y - 1 == start.y){
			return true;
		}
		else{
			return false;
		}
	}

	List<Tile> initRooms(List<Tile> nodes){
		//idk this is only here to help organize
		return nodes;
	}

	void drawTiles(List<Tile> tiles){

		foreach(Tile tile in tiles){

			spriteArray[(int)(tile.x), (int)(tile.y)] = Instantiate(walkable, new Vector3(tile.x, tile.y), transform.rotation) as GameObject;
			spriteArray[(int)(tile.x), (int)(tile.y)].transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}