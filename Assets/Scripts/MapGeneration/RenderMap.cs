using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderMap: Photon.MonoBehaviour {

    public GameObject Ground;
    public GameObject Background;
    public GameObject Wall;
    public GameObject ExitTile;

    private GameObject[,] _displayedMapArray;
    private Map _gameMap;

    void Start() {
        DelegateHolder.OnMapGenerated += ReRenderMap;//set ReRenderMap() to trigger when the map has been generated
    }

    public void ReRenderMap(bool isHost) {
        if (isHost) {//if we are the host, then we go to the mapGenerator to get the map
            _gameMap = GenerateMap.Map;//set the map
        }

        if (_displayedMapArray != null)
            DestroyOldMap();

        _displayedMapArray = new GameObject[_gameMap.mapWidth, _gameMap.mapHeight];
        RenderTiles(_gameMap.mapTiles, isHost);
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
            photonView.RPC("SetMapFromServer", PhotonTargets.OthersBuffered, new System.Object[] { _gameMap.mapWidth, _gameMap.mapHeight, _gameMap.SerializeMapTiles() });
        }

        for (int y = 0; y < _gameMap.mapHeight; y++) {
            for (int x = 0; x < _gameMap.mapWidth; x++) {
                MapTile.TileType tileType = tiles[x, y].GetTileType();
                GameObject tile = null;
                switch (tileType) {
                    case MapTile.TileType.red:
                        tile = Ground;
                        break;
                    case MapTile.TileType.white:
                        tile = null;//we are not displaying the background tiles right now
                        break;
                    case MapTile.TileType.blue:
                        tile = Wall;
                        break;
                    case MapTile.TileType.green:
                        tile = ExitTile;
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
        MapTile[,] mapTiles = new MapTile[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) 
                mapTiles[x, y] = new MapTile((int)System.Char.GetNumericValue((mapString[stringindx++])));
        }

        _gameMap = new Map(mapWidth, mapHeight, mapTiles);
        DelegateHolder.TriggerMapGenerated(false);//false because it is not the host
    }

}