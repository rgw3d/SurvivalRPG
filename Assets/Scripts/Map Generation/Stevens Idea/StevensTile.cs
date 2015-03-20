using UnityEngine;
using System.Collections;

public class StevensTile {
	
	public TileType tileType;
    public TileSubType tileSubType;

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

    public StevensTile(StevensTile.TileType Type) {
		tileType = Type;
    }

	public void setTileType(StevensTile.TileType Type){
		tileType = Type;
	}

    public string toString() {
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
}
