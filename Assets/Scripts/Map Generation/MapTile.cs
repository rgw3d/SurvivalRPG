using UnityEngine;
using System.Collections;

public class MapTile {

    private TileType _tileType;

    public enum TileType {
        red = 0,
        white = 1,
        blue = 2,
        green = 3

    }

    public MapTile(TileType tileType) {
        _tileType = tileType;
    }

    public MapTile(int tileValue) {
        switch (tileValue) {
            case 0:
                _tileType = TileType.red;
                break;
            case 1:
                _tileType = TileType.white;
                break;
            case 2:
                _tileType = TileType.blue;
                break;
            case 3:
                _tileType = TileType.green;
                break;
        }
    }

    public void SetTileType(TileType tileType) {
        _tileType = tileType;
    }

    public TileType GetTileType() {
        return _tileType;
    }

    public override string ToString() {
        switch (_tileType) {
            case TileType.red:
                return "red";
            case TileType.white:
                return "white";
            case TileType.blue:
                return "blue";
            case TileType.green:
                return "Green";
        }
        return "white";
    }

    public override int GetHashCode() {
        return (int)_tileType;
    }

    public override bool Equals(System.Object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        //can it be cast
        MapTile p = obj as MapTile;
        if ((System.Object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return (GetTileType() == p.GetTileType());
    }
}
