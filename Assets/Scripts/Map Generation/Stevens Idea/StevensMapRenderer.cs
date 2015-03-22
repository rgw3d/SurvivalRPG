using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapRenderer : Photon.MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	public GameObject wall;
	public GameObject goal;

	private GameObject[,] spriteArray;
    private StevensMap Map;

	public StevensMapGeneration mapGeneration;
    private bool isHost = true;


	
	// Use this for initialization
	void Start () {

        if (mapGeneration == null) 
            mapGeneration = GetComponent<StevensMapGeneration>();
       /* if (mapGeneration != null) {
            Map = mapGeneration.Map;//set the map
            spriteArray = new GameObject[Map.mapWidth, Map.mapHeight];
        }
        * */
        
		
	}

	public void reRenderMap(){
        if (isHost) {//If the map generator is not null (on the client side)
            Map = mapGeneration.Map;//set the map

            if (spriteArray != null)
                destroyOldMap();

            spriteArray = new GameObject[Map.mapWidth, Map.mapHeight];
            renderTiles(Map.mapTiles);
        }
        else {
            if (spriteArray != null)
                destroyOldMap();

            spriteArray = new GameObject[Map.mapWidth, Map.mapHeight];
            renderTiles(Map.mapTiles);
        }
	}

	void destroyOldMap(){
        foreach(GameObject obj in spriteArray){
            if(obj!= null)
                Destroy(obj);
        }
	}

	void renderTiles(StevensTile[,] tiles){
		for(int y = 0; y < Map.mapHeight; y++){
			for(int x = 0; x < Map.mapWidth; x++){
				StevensTile.TileType tileType = tiles[x,y].tileType;
				GameObject tile = background;
				switch(tileType){
				case StevensTile.TileType.red:
					tile = walkable;
					break;
				case StevensTile.TileType.white:
					tile = null;
					break;
				case StevensTile.TileType.blue:
					tile = wall;
					break;
				case StevensTile.TileType.green:
					tile = goal;
					break;
				}
				if(tile != null){
					spriteArray[x,y] = Instantiate(tile, new Vector3((float)x + .5f, (float)y + .5f), transform.rotation) as GameObject;
					spriteArray[x,y].transform.parent = gameObject.transform;
				}
			}
		}
        if (isHost) {
            Debug.Log("Sending RPC to set map tiles");
            photonView.RPC("SetMapFromServer", PhotonTargets.OthersBuffered, new System.Object[] { new Vector2(Map.mapWidth, Map.mapHeight), Map.SerializeMapTiles() });
        }

	}
    [RPC]
    void SetMapFromServer(Vector2 HeightWidth, string mapString) {
        Debug.Log("Recieved Map Data via RPC call");
        Debug.Log(mapString);
        int stringindx = 0;
        StevensTile[,] mapTiles = new StevensTile[(int)HeightWidth.x, (int)HeightWidth.y];
        for (int y = 0; y < HeightWidth.y; y++) {
            for (int x = 0; x < HeightWidth.x; x++) {
                mapTiles[x, y] = new StevensTile((int)System.Char.GetNumericValue((mapString[stringindx++])));
                //Debug.Log("Tile type: "+System.Convert.ToInt32(mapString[stringindx-1]));
            }
        }

        Map = new StevensMap((int)HeightWidth.x, (int)HeightWidth.y, mapTiles);
        isHost = false;
        reRenderMap();
    }

     
}