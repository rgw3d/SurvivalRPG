using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RichardMapGen1 : MonoBehaviour {


    public GameObject red;
    public GameObject background;
    public GameObject blue;

    public int mapWidth = 100;
    public int mapHeight = 100;
    public int nodeCreationAttempts = 50;
    public int numberOfClusters = 5;
    public int numberOfRoomsInClusters = 3;
    public int interClusterRange = 2;
    public int minimumHalfWidthOfRoom = 3;
    public int maximumHalfWidthOfRoom = 6;
    public int minimumHalfHeightOfRoom = 3;
    public int maximumHalfHeightOfRoom = 6;

    

    public int minimumDistanceBetweenNodes = 20;
    public List<GameObject> Players;

    public GameObject[,] mapTiles;
    
    public List<Tile> roomNodes;

    void Start() {
        if (Players == null) {//then find the player
            Players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        }

        roomNodes = initRoomNodes();
        List<List<Tile>> roomCoords = createRooms(roomNodes);
        mapTiles = new GameObject[mapWidth, mapHeight];
        mapTiles = drawRooms(roomCoords,mapTiles);

        //List<List<Tile>> pathCoords = createCorridors(roomNodes);
        //mapTiles = drawRooms(pathCoords, mapTiles);

        //addBlueBackground(mapTiles);
        
        //fillBackground(mapTiles);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            regenerate();
        }
        if (Input.GetKey(KeyCode.F)) {
            genCorridors();
        }

    }

    void genCorridors() {
        List<List<Tile>> pathCoords = createCorridors(roomNodes);
        mapTiles = drawRooms(pathCoords, mapTiles);
    }


    void regenerate() {
        foreach (GameObject x in mapTiles) {
            Destroy(x);
        }
        roomNodes = initRoomNodes();
        List<List<Tile>> roomCoords = createRooms(roomNodes);
        mapTiles = new GameObject[mapWidth, mapHeight];
        mapTiles = drawRooms(roomCoords, mapTiles);
        //addBlueBackground();
        //fillBackground(mapTiles);

       
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
            nodeList.Add(new Tile(player.transform.position.x, player.transform.position.y));
        }

        return nodeList;
    }


    /*
     * This method will create the rooms and return them grouped together in a list stored inside a list
     * 
     */
    public List<List<Tile>> createRooms(List<Tile> roomNodes) {

        List<List<Tile>> roomCoordinates = new List<List<Tile>>();

        foreach (Tile node in roomNodes) {
            int botXTest = (int)node.x - Random.Range(minimumHalfWidthOfRoom, maximumHalfWidthOfRoom);
            int botX = botXTest > 0 ? botXTest : (int)node.x;

            int botYTest = (int)node.y - Random.Range(minimumHalfHeightOfRoom, maximumHalfHeightOfRoom);
            int botY = botYTest > 0 ? botYTest : (int)node.y;

            int topXTest = (int)node.x + Random.Range(minimumHalfWidthOfRoom, maximumHalfWidthOfRoom);
            int topX = topXTest < mapWidth ? topXTest : (int)node.x;

            int topYTest = (int)node.y + Random.Range(minimumHalfHeightOfRoom, maximumHalfHeightOfRoom);
            int topY = topYTest < mapHeight ? topYTest : (int)node.x;


            Tile botLeft = new Tile(botX,botY);
            Tile topRight = new Tile(topX,topY);
            roomCoordinates.Add(fillArea(botLeft, topRight));
        }

        return roomCoordinates;
    }

    private List<Tile> fillArea(Tile botLeftCoord, Tile upRightCoord) {
        List<Tile> room = new List<Tile>();
        int areaWidth = (int)(upRightCoord.x - botLeftCoord.x);
        int areaHeight = (int)(upRightCoord.y - botLeftCoord.y);

        for (int x = 0; x < areaWidth; x++) {
            for (int y = 0; y < areaHeight; y++) {
                room.Add(new Tile(botLeftCoord.x + x, botLeftCoord.y + y));
            }
        }

        return room;
    }


    /*
     * This will draw all the rooms based on the locations that it recieves
     * 
     */
    public GameObject[,] drawRooms(List<List<Tile>> roomsCoordinates, GameObject[,] mapTiles) {
        
        foreach(List<Tile> room in roomsCoordinates){
            foreach (Tile node in room) {
                try {
                    if (mapTiles[(int)node.x, (int)node.y] == null) {
                        mapTiles[(int)node.x, (int)node.y] = Instantiate(red, new Vector3(node.x, node.y), transform.rotation) as GameObject;
                        mapTiles[(int)node.x, (int)node.y].transform.parent = gameObject.transform;
                    }
                }
                catch {

                }
            }
        }
        return mapTiles;

    }

    public List<List<Tile>> createCorridors(List<Tile> roomNodes) {
        
        List<List<Tile>> corridorsToFill = new List<List<Tile>>();
        Dictionary<Tile, bool> ConnectedNodes = new Dictionary<Tile, bool>();
        foreach (Tile tile in roomNodes) {
            try {
                ConnectedNodes.Add(tile, false);
            }
            catch { };
        }
        
        Tile nextTile = initBaseNode()[0];//grab the player starting point
        while (nextTile != null) {
            ConnectedNodes[nextTile] = true;
            List<Tile> OrderedList = orderedPoints(nextTile, roomNodes);
            Tile connectTo = null;
            foreach (Tile tile in OrderedList) {
                if (ConnectedNodes[tile] == false) {
                    connectTo = tile;
                    break;
                }
            }
            if (connectTo == null)
                break;

            List<Tile> tmpPath = new List<Tile>();
            tmpPath.AddRange(drawXPath(nextTile, connectTo));
            tmpPath.AddRange(drawYPath(nextTile,connectTo));
            corridorsToFill.Add(tmpPath);

            nextTile = connectTo;

        }

        return corridorsToFill;
    }

    public List<Tile> orderedPoints(Tile origin, List<Tile> nodes) {
        var orderedList = from tile in nodes.ToArray()
                                 orderby Mathf.Pow(Mathf.Abs(tile.x - origin.x) + Mathf.Abs(tile.y-origin.y),0.5f)
                                 select tile;
        return orderedList.ToList<Tile>();
    }

    public List<Tile> drawXPath(Tile start, Tile end){
        List<Tile> path = new List<Tile>();

        if (start.x < end.x) {
            for (float i = 0; i <= end.x-start.x; i++) {
                path.Add(new Tile(start.x + i, start.y));
            }
        }
        else if (start.x > end.x) {
            for (float i = 0; i <= start.x-end.x; i++) {
                path.Add(new Tile(end.x + i, start.y));
            }
        }
        return path;
    }

    public List<Tile> drawYPath(Tile start, Tile end){
        List<Tile> path = new List<Tile>();
        if (start.y < end.y) {
            for (float i = 0; i <= end.y-start.y; i++) {
                path.Add(new Tile(end.x , start.y+i));
            }
        }
        else if (start.y > end.y) {
            for (float i = 0; i <= start.y-end.y; i++) {
                path.Add(new Tile(end.x, end.y+i));
            }
        }
        return path;
    }



    /*public GameObject[,] addBlueBackground(GameObject[,] tileArray) {
        for (int c = 0; c < mapWidth; c++) {
            for (int r = 0; r < mapHeight; r++) {
                if (tileArray[c, r] == red) {
                    tileArray[c, r] = Instantiate(background, new Vector3(c, r), transform.rotation) as GameObject;
                }
            }
        }


    }
     * */
        

    /*
     * This should fill the background 
     * 
     */
    public GameObject[,] fillBackground(GameObject[,] tileArray) {
        for (int c = 0; c < mapWidth; c++) {
            for (int r = 0; r < mapHeight; r++) {
                if(tileArray[c,r] == null){
                    tileArray[c, r] = Instantiate(background, new Vector3(c,r), transform.rotation) as GameObject;
                }
            }
        }

        return tileArray;
        
    
    }

    
    


	
}
