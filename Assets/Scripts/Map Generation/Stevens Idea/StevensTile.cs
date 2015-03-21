using UnityEngine;
using System.Collections;

public class StevensTile {
	
	public TileType tileType;

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

    public int GetHashCode() {
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
}
