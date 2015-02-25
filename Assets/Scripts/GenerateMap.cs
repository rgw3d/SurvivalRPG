using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

    public GameObject walkable;
    public GameObject background;

    public int width = 100;
    public int roomNumber = 5;
    public int roomWidth = 3;
    public int minmumDistanceBetweenRooms = 10;


    private GameObject[,] spriteArray;

	// Use this for initialization
	void Start () {
        spriteArray = new GameObject[width,width];
        List<Vector2> roomNodes = initRoomNodes();
        createRooms(roomNodes);
        
	}

    private List<Vector2> initRoomNodes(){
        List<Vector2> nodes = new List<Vector2>();
        nodes.Add(new Vector2(0, 0));
        int maxTries = 10;
        int tries = 0;
        //randomly select some nodes to be rooms

        for (int i = 0; i < roomNumber && tries<maxTries; i++){
            Vector2 pos = new Vector2(Random.Range(0, width-1), Random.Range(0, width-1));
			bool addNode = true;
            foreach(Vector2 node in nodes){
                if (Mathf.Pow(Mathf.Pow(node.x - pos.x, 2) + Mathf.Pow(node.y - pos.y, 2), 0.5f) < minmumDistanceBetweenRooms){
                    tries++;
                    i--;
					addNode = false;
					break;
                }
            }
			if (addNode){
            	nodes.Add(pos);
			}
        }
        return nodes;
    }

    void createRooms(List<Vector2> positions) {
        //now to create the rooms of the desired width.
        //Assumed that there is enough room somewhere
        //just determine what path is needed to take, like the node is the top right, top left, middle, or bottom left/right
        //check to see what directions can be extended from the node. meaning, if you can go up, left, bot, and right for a distance of roomWidth-1 than its mid
        //you can see the process for this.  if it only two directions, then it is a cornor, and a room can be created with those parameters

        foreach(Vector2 pos in positions){
            int dist = roomWidth-2;//imagine a 3x3 grid, and the middle pice. the width = 3. need to check if width-2 piece from the center is valid
            bool middle = validCoords(new Vector2(pos.x - dist, pos.y)) && validCoords(new Vector2(pos.x + dist, pos.y)) 
                && validCoords(new Vector2(pos.x, pos.y - dist)) && validCoords(new Vector2(pos.x, pos.y + dist));
            if (middle) {
				fillArea(new Vector2(pos.x-dist,pos.y-dist),new Vector2(pos.x+dist,pos.y+dist));
            }
            else {//determine what cornor to use
                dist = roomWidth - 1;//imageine a 3x3 grid, and a cornor. width = 3. need to check if width-1 piece from the cornor is valid
                bool up = validCoords(new Vector2(pos.x, pos.y+dist));
                bool down = validCoords(new Vector2(pos.x, pos.y - dist));
                bool right = validCoords(new Vector2(pos.x+dist, pos.y));
                bool left = validCoords(new Vector2(pos.x-dist, pos.y + dist));

                if (up && right) {
                    fillArea(pos, new Vector2(pos.x + dist, pos.y + dist));
                }
                else if (up && left) {
                    fillArea(new Vector2(pos.x-dist,pos.y), new Vector2(pos.x, pos.y + dist));
                }
                else if (down && right) {
                    fillArea(new Vector2(pos.x-dist,pos.y-dist), pos);
                }
                else if (down && left) {
                    fillArea(new Vector2(pos.x,pos.y-dist), new Vector2(pos.x + dist, pos.y));
                }
            }

            
        }
        
    }

    bool validCoords(Vector2 pos) {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < width;
    }

    void fillArea(Vector2 botLeftCoord, Vector2 upRightCoord) {
		int areaWidth = (int)(upRightCoord.x - botLeftCoord.x);
        int areaHeight = (int)(upRightCoord.y - botLeftCoord.y);

        for (int x = 0; x < areaWidth; x++) {
            for (int y = 0; y < areaHeight; y++) {
                spriteArray[(int)(botLeftCoord.x + x), (int)(botLeftCoord.y + y)] = Instantiate(walkable, new Vector3(botLeftCoord.x + x, botLeftCoord.y + y), transform.rotation) as GameObject;
                spriteArray[(int)(botLeftCoord.x + x), (int)(botLeftCoord.y + y)].transform.parent = gameObject.transform;
            }
        }
    }

    void createPaths(List<Vector2> positions){

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
