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
	}

	public void reRenderMap(){
        if (isHost) {//if we are the host, then we go to the mapGenerator to get the map
            _gameMap = mapGeneration.Map;//set the map

            if (_displayedMapArray != null)
                destroyOldMap();

            _displayedMapArray = new GameObject[_gameMap.mapWidth, _gameMap.mapHeight];
            renderTiles(_gameMap.mapTiles);
        }
        else {
            if (_displayedMapArray != null)
                destroyOldMap();

            _displayedMapArray = new GameObject[_gameMap.mapWidth, _gameMap.mapHeight];
            renderTiles(_gameMap.mapTiles);
        }
	}

	void destroyOldMap(){
        foreach(GameObject obj in _displayedMapArray){
            if(obj!= null)
                Destroy(obj);
        }
	}

	void renderTiles(StevensTile[,] tiles){
		for(int y = 0; y < _gameMap.mapHeight; y++){
			for(int x = 0; x < _gameMap.mapWidth; x++){
				StevensTile.TileType tileType = tiles[x,y].tileType;
				GameObject tile = Background;
				switch(tileType){
				case StevensTile.TileType.red:
					tile = Ground;
					break;
				case StevensTile.TileType.white:
					tile = null;
					break;
				case StevensTile.TileType.blue:
					tile = Wall;
					break;
				case StevensTile.TileType.green:
					tile = ExitTile;
					break;
				}
				if(tile != null){
					_displayedMapArray[x,y] = Instantiate(tile, new Vector3((float)x + .5f, (float)y + .5f), transform.rotation) as GameObject;
					_displayedMapArray[x,y].transform.parent = gameObject.transform;
				}
			}
		}
        if (isHost) {
            Debug.Log("Sending RPC to set map tiles");
            photonView.RPC("SetMapFromServer", PhotonTargets.OthersBuffered, new System.Object[] { new Vector2(_gameMap.mapWidth, _gameMap.mapHeight), _gameMap.SerializeMapTiles() });
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

        _gameMap = new StevensMap((int)HeightWidth.x, (int)HeightWidth.y, mapTiles);
        isHost = false;
        reRenderMap();
    }
     
}