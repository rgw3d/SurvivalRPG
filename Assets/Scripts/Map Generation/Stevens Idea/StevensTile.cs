using UnityEngine;
using System.Collections;

public class StevensTile {
	
	public TileType tileType;
    public TileSubType tileSubType;
	public int x;
	public int y;
	public int F;
	public int G;
	public int H;
	public StevensTile parent;
	
	public enum TileType {
		red,
		white,
		blue,
		green
	}

    public enum TileSubType {
        center,
        topEdge,
        botEdge,
        rightEdge,
        leftEdge,
        topRightCorner,
        topLeftCorner,
        botRightCorner,
        botLeftCorner
    }

    public StevensTile(StevensTile.TileType Type, int X, int Y) {
		tileType = Type;
		x = X;
		y = Y;
    }

	public void setTileType(StevensTile.TileType Type){
		tileType = Type;
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
