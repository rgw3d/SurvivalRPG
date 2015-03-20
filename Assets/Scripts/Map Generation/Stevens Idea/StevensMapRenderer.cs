using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapRenderer : MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	public GameObject wall;
	public GameObject goal;
	
	private int mapWidth;
	private int mapHeight;

	private GameObject[,] spriteArray;

	public StevensMapGeneration mapGen;
	
	// Use this for initialization
	void Start () {
        
		//mapWidth = mapGen.mapWidth;
		//mapHeight = mapGen.mapHeight;
		//spriteArray = new GameObject[mapWidth, mapHeight];
		
		//renderTiles(mapGen.map.mapTiles);
	}


	public void reRenderMap(){

		mapWidth = mapGen.mapWidth;
		mapHeight = mapGen.mapHeight;

        if (spriteArray != null) 
		    destroyOldMap();

		spriteArray = new GameObject[mapWidth, mapHeight];

		renderTiles(mapGen.map.mapTiles);
	}

	void destroyOldMap(){
		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x < mapWidth; x++){
				Destroy(spriteArray[x,y]);
			}
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
					//spriteArray[x,y] = Instantiate(tile, new Vector3((float)x + .5f, (float)y + .5f), transform.rotation) as GameObject;
					//spriteArray[x,y].transform.parent = gameObject.transform;
                    PhotonNetwork.Instantiate(tileType.ToString(), new Vector3((float)x + .5f, (float)y + .5f), transform.rotation,0);
                    //Debug.Log("Instantiating objects");
				}
			}
		}
	}
}