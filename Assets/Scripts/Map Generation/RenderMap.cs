using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderMap: Photon.MonoBehaviour {

    public GameObject Ground;
    public GameObject Background;
    public GameObject Wall;
    public GameObject ExitTile;

    private GameObject[,] _displayedMapArray;
    private StevensMap _gameMap;
    private bool _isHost = true;

    public void ReRenderMap() {
        if (_isHost) {//if we are the host, then we go to the mapGenerator to get the map
            _gameMap = GenerateMap.Map;//set the map

            if (_displayedMapArray != null)
                DestroyOldMap();

            _displayedMapArray = new GameObject[_gameMap.mapWidth, _gameMap.mapHeight];
            RenderTiles(_gameMap.mapTiles);
        }
        else {//we are not the host, so dont update the map - assume that it has already been done (updated via RPC)
            if (_displayedMapArray != null)
                DestroyOldMap();

            _displayedMapArray = new GameObject[_gameMap.mapWidth, _gameMap.mapHeight];
            RenderTiles(_gameMap.mapTiles);
        }
    }

    void DestroyOldMap() {
        foreach (GameObject obj in _displayedMapArray) {
            if (obj != null)
                Destroy(obj);
        }
    }

    void RenderTiles(StevensTile[,] tiles) {
        if (_isHost) {//brodcast data
            Debug.Log("Sending RPC to set map tiles");
            photonView.RPC("SetMapFromServer", PhotonTargets.OthersBuffered, new System.Object[] { new Vector2(_gameMap.mapWidth, _gameMap.mapHeight), _gameMap.SerializeMapTiles() });
        }

        for (int y = 0; y < _gameMap.mapHeight; y++) {
            for (int x = 0; x < _gameMap.mapWidth; x++) {
                StevensTile.TileType tileType = tiles[x, y].tileType;
                GameObject tile = null;
                switch (tileType) {
                    case StevensTile.TileType.red:
                        tile = Ground;
                        break;
                    case StevensTile.TileType.white:
                        tile = null;//we are not displaying the background tiles right now
                        break;
                    case StevensTile.TileType.blue:
                        tile = Wall;
                        break;
                    case StevensTile.TileType.green:
                        tile = ExitTile;
                        break;
                    default:
                        tile = Background;
                        break;
                }
                if (tile != null) {
                    _displayedMapArray[x, y] = Instantiate(tile, new Vector3((float)x + .5f, (float)y + .5f), transform.rotation) as GameObject;
                    _displayedMapArray[x, y].transform.parent = gameObject.transform;
                }
            }
        }
    }

    [RPC]
    void SetMapFromServer(Vector2 HeightWidth, string mapString) {
        Debug.Log("Recieved Map Data via RPC call");
        int stringindx = 0;
        StevensTile[,] mapTiles = new StevensTile[(int)HeightWidth.x, (int)HeightWidth.y];
        for (int y = 0; y < HeightWidth.y; y++) {
            for (int x = 0; x < HeightWidth.x; x++) 
                mapTiles[x, y] = new StevensTile((int)System.Char.GetNumericValue((mapString[stringindx++])));
        }

        _gameMap = new StevensMap((int)HeightWidth.x, (int)HeightWidth.y, mapTiles);
        _isHost = false;
        ReRenderMap();
    }

}