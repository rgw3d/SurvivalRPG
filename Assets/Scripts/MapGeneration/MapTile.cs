using UnityEngine;
using System.Collections;

public class MapTile {

    public TileType tileType;

	public int x;
	public int y;
	public int F; //=G+H
	public int G; //= Dist to init tile
	public int H; //= Dist to goal/ladder tile
	public MapTile parent; // the public

    public enum TileType {
        red = 0,
        white = 1,
        blue = 2,
        green = 3

    }

    public MapTile(TileType TileType, int X, int Y) {
        tileType = TileType;
		x = X;
		y = Y;
    }

    public MapTile(int tileValue) {
        switch (tileValue) {
            case 0:
                tileType = TileType.red;
                break;
            case 1:
                tileType = TileType.white;
                break;
            case 2:
                tileType = TileType.blue;
                break;
            case 3:
                tileType = TileType.green;
                break;
        }
    }

    public void SetTileType(TileType TileType) {
        tileType = TileType;
    }

    public TileType GetTileType() {
        return tileType;
    }

    public override string ToString() {
        switch (tileType) {
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
        return (int)tileType;
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

	public bool isOrthogonalTo(MapTile otherTile){
		if(x + 1 == otherTile.x && y == otherTile.y)
			return true;
		else if (x - 1 == otherTile.x && y == otherTile.y)
			return true;
		else if (x == otherTile.x && y - 1 == otherTile.y)
			return true;
		else if (x == otherTile.x && y + 1 == otherTile.y)
			return true;
		else
			return false;
	}
	
	public bool isDiagonalTo(MapTile otherTile){
		if(x + 1 == otherTile.x && y + 1 == otherTile.y)
			return true;
		else if (x - 1 == otherTile.x && y + 1 == otherTile.y)
			return true;
		else if (x - 1 == otherTile.x && y - 1 == otherTile.y)
			return true;
		else if (x + 1 == otherTile.x && y - 1 == otherTile.y)
			return true;
		else
			return false;
	}
}
