using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapScript : MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	
	public int width = 100;

	public int startingRoomWidth = 3;

	public int roomNumber = 5;
	public int roomWidth = 3;
	public int minimumDistanceBetweenRooms = 10;
	
	
	private GameObject[,] spriteArray;
	
	// Use this for initialization
	void Start () {
		spriteArray = new GameObject[width,width];
		List<Vector2> roomNodes = initRoomNodes();
		
		createRooms(roomNodes);
	}
	
	List<Vector2> initRoomNodes(){
		List<Vector2> nodes = new List<Vector2>();
		for(int i = 0; i < startingRoomWidth; i++){ // creates an initial 3x3 room for the player to start in
			for(int j = 0; j < startingRoomWidth; j++){
				nodes.Add(new Vector2(j, i));
			}
		}

		nodes.Add(new Vector2(Random.Range(3, width-1), Random.Range(3, width-1)));
		nodes.Add(new Vector2(Random.Range(3, width-1), Random.Range(3, width-1)));
		nodes = initCorridors(nodes);

		return nodes;
	}

	List<Vector2> initCorridors(List<Vector2> nodes){ // adds "corridor" nodes to connect rooms to 1,1
		List<Vector2> tmpNodes = new List<Vector2>(nodes); // needed so that error isn't thrown about modifying a list that's being looped
		foreach(Vector2 tile in tmpNodes){
			if(tile.x != 1f && tile.y != 1f){
				if(!checkAdjacent(tile, new Vector2(1f, 1f))){
					for(int x = 1; x < (int)tile.x ; x++){
						nodes.Add(new Vector2((float)x, tile.y));
					}
					for(int y = 1; y < (int)tile.y ; y++){
						nodes.Add(new Vector2(1f , (float)y));
					}
				}
			}
		}
		return nodes;
	}

	bool checkAdjacent(Vector2 tile, Vector2 start){ // checks if tile is direct up, down, left, or right of start
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

	void createRooms(List<Vector2> rooms){

		foreach(Vector2 tile in rooms){

			spriteArray[(int)(tile.x), (int)(tile.y)] = Instantiate(walkable, new Vector3(tile.x, tile.y), transform.rotation) as GameObject;
			spriteArray[(int)(tile.x), (int)(tile.y)].transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}