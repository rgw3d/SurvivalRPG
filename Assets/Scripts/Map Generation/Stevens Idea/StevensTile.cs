using UnityEngine;
using System.Collections;

public class StevensTile {
	
	public TileType tileType;

	public int x;
	public int y;
	public int F;
	public int G;
	public int H;
	public StevensTile parent;

	public enum TileType {
		red=0,
		white=1,
		blue=2,
		green=3

	}

    public StevensTile(StevensTile.TileType type) {
		tileType = type;
    }
	
    public StevensTile(int type) {
        switch (type) {
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

    public StevensTile(StevensTile.TileType Type, int X, int Y) {
		tileType = Type;
		x = X;
		y = Y;

    }

	public void setTileType(StevensTile.TileType Type){
		tileType = Type;
	}
	
    public override string ToString() {
        switch(tileType){
            case StevensTile.TileType.red:
                return "red";
            case StevensTile.TileType.white:
                return "white";
            case StevensTile.TileType.blue:
                return "blue";
            case StevensTile.TileType.green:
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

        StevensTile p = obj as StevensTile;
        if ((System.Object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return (tileType == p.tileType);
    }

	public bool isOrthogonalTo(StevensTile otherTile){
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

	public bool isDiagonalTo(StevensTile otherTile){
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
