using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapRenderer : Photon.MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	public GameObject wall;
	public GameObject goal;
	
	private int mapWidth;
	private int mapHeight;

	private GameObject[,] spriteArray;
    private StevensMap Map;

	public StevensMapGeneration mapGeneration;
	
	// Use this for initialization
	void Start () {

        if (mapGeneration == null) 
            mapGeneration = GetComponent<StevensMapGeneration>();
        Map = mapGeneration.Map;

        
		mapWidth = mapGeneration.mapWidth;//set width from the map gen script
		mapHeight = mapGeneration.mapHeight;//set height from the 
		spriteArray = new GameObject[mapWidth, mapHeight];
		
		//renderTiles(mapGen.map.mapTiles);
	}

	public void reRenderMap(){

		mapWidth = mapGeneration.mapWidth;
		mapHeight = mapGeneration.mapHeight;

        if (spriteArray != null) 
		    destroyOldMap();

		spriteArray = new GameObject[mapWidth, mapHeight];

		renderTiles(mapGeneration.Map.mapTiles);
	}

	void destroyOldMap(){
        foreach(GameObject obj in spriteArray){
            if(obj!= null)
                Destroy(obj);
        }
	}

	void renderTiles(StevensTile[,] tiles){
		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x < mapWidth; x++){
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
    void renderTile(Vector3 gameObject) {
        
        //spriteArray[gameObject.x, gameObject.y] = Instantiate(allTiles[x, y], allTiles[x, y].transform.position, Quaternion.identity) as GameObject;
        //spriteArray[x, y].transform.parent = gameObject.transform;
                
    }

    [RPC]
    void renderTile(GameObject[,] allTiles) {
        for(int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapWidth; x++) {
                if (allTiles[x,y] != null) {
                    spriteArray[x,y] = Instantiate(allTiles[x,y], allTiles[x,y].transform.position, Quaternion.identity) as GameObject;
                    spriteArray[x, y].transform.parent = gameObject.transform;
                }
            }
        }
    }

    
}