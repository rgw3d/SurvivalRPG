using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StevensMapRenderer : MonoBehaviour {
	
	public GameObject walkable;
	public GameObject background;
	
	private int mapWidth;
	private int mapHeight;

	private GameObject[,] spriteArray;
	
	// Use this for initialization
	void Start () {

		StevensMapGeneration mapGen = GetComponent<StevensMapGeneration>();
		mapWidth = mapGen.mapWidth;
		mapHeight = mapGen.mapHeight;

		spriteArray = new GameObject[mapWidth, mapHeight];
		
		RenderTiles(mapGen.map.mapTiles);
	}

	void RenderTiles(StevensTile[,] tiles){
		for(int y = 0; y < mapHeight; y++){
			for(int x = 0; x < mapWidth; x++){
				StevensTile.TileType tileType = tiles[x,y].tileType;
				GameObject tile = background;
				switch(tileType){
				case StevensTile.TileType.red:
					tile = walkable;
					break;
				case StevensTile.TileType.white:
					tile = background;
					break;
				}

				spriteArray[x,y] = Instantiate(tile, new Vector3((float)x + .5f, (float)y + .5f), transform.rotation) as GameObject;
				spriteArray[x,y].transform.parent = gameObject.transform;
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}