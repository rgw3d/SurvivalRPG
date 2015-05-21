using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderMap: Photon.MonoBehaviour {

    public GameObject Ground;
    public GameObject Background;
    public GameObject Wall;
    public GameObject ExitTile;
	public GameObject Obstacle;

    private GameObject[,] _displayedMapArray;
    public static Map Map;


    void Start() {
        DelegateHolder.OnMapGenerated += ReRenderMap;//set ReRenderMap() to trigger when the map has been generated
    }

    public void ReRenderMap(bool isHost) {
        if (isHost) {//if we are the host, then we go to the mapGenerator to get the map
            Map = GenerateMap.Map;//set the map
        }

        if (_displayedMapArray != null)
            DestroyOldMap();

        _displayedMapArray = new GameObject[Map.mapWidth, Map.mapHeight];
        RenderTiles(Map.mapTiles, isHost);
        DelegateHolder.TriggerMapRendered(isHost);
    }

    void DestroyOldMap() {
        foreach (GameObject obj in _displayedMapArray) {
            if (obj != null)
                Destroy(obj);
        }
    }

    void RenderTiles(MapTile[,] tiles, bool isHost) {
        if (isHost) {//brodcast data
            Debug.Log("Sending RPC to set map tiles");
            photonView.RPC("SetMapFromServer", PhotonTargets.OthersBuffered, new System.Object[] { Map.mapWidth, Map.mapHeight, Map.SerializeMapTiles() });
        }
        for (int y = 0; y < Map.mapHeight; y++) {
            for (int x = 0; x < Map.mapWidth; x++) {
                MapTile.TileType tileType = tiles[x, y].GetTileType();
                GameObject tile = null;
                switch (tileType) {
                    case MapTile.TileType.Ground:
                        tile = Ground;
                        break;
                    case MapTile.TileType.Background:
                        tile = null;//we are not displaying the background tiles right now
                        break;
                    case MapTile.TileType.Wall:
                        tile = Wall;
                        break;
                    case MapTile.TileType.ExitTile:
                        tile = ExitTile;
                        break;
					case MapTile.TileType.Obstacle:
						tile = Obstacle;
						break;
                    default:
                        tile = Background;
                        break;
                }
                if (tile != null) {
                    _displayedMapArray[x, y] = Instantiate(tile, new Vector3((float)x, (float)y), transform.rotation) as GameObject;
                    _displayedMapArray[x, y].transform.parent = gameObject.transform;
                }
            }
        }
    }

    [RPC]
    void SetMapFromServer(int mapWidth, int mapHeight, string mapString) {
        Debug.Log("Recieved Map Data via RPC call");
        int stringindx = 0;
        if (_displayedMapArray != null)//Destroy old map
            DestroyOldMap();
        MapTile[,] mapTiles = new MapTile[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) 
                mapTiles[x, y] = new MapTile((int)System.Char.GetNumericValue((mapString[stringindx++])));
        }

        Map = new Map(mapWidth, mapHeight, mapTiles);
        DelegateHolder.TriggerMapGenerated(false);//false because it is not the host
    }

}