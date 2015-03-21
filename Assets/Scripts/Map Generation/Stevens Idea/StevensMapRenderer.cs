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
	
	// Use this for initialization
	void Start () {

        if (mapGeneration == null) 
            mapGeneration = GetComponent<StevensMapGeneration>();
        Map = mapGeneration.Map;//set the map

		spriteArray = new GameObject[Map.mapWidth, Map.mapHeight];
		
	}

	public void reRenderMap(){
        Map = mapGeneration.Map;//set the map

        if (spriteArray != null) 
		    destroyOldMap();

		spriteArray = new GameObject[Map.mapWidth, Map.mapHeight];
		renderTiles(Map.mapTiles);
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
                    //PhotonNetwork.Instantiate(tileType.ToString(), new Vector3((float)x + .5f, (float)y + .5f), transform.rotation,0);
                    
                    //Debug.Log("Instantiating objects");
				}
			}
		}
        photonView.RPC("renderTile", PhotonTargets.OthersBuffered, spriteArray);

	}

    [RPC]
    void SetMapFromServer(Vector2 HeightWidth, string mapString) {

    }

    [RPC]
    void renderTile(Vector3 gameObject) {
        
        //spriteArray[gameObject.x, gameObject.y] = Instantiate(allTiles[x, y], allTiles[x, y].transform.position, Quaternion.identity) as GameObject;
        //spriteArray[x, y].transform.parent = gameObject.transform;
                
    }

    

    
}