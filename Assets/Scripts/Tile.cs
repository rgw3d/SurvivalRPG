using UnityEngine;
using System.Collections;

public class Tile {

	public float x;
	public float y;
	public TileType tileType;
    public TileSubType tileSubType;

	public enum TileType {
		red,
		white
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

    public Tile() {

    }
    public Tile(float xLoc, float yLoc) {
		x = xLoc;
		y = yLoc;
    }

    public override bool Equals(System.Object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Tile p = obj as Tile;
        if ((System.Object)p == null) {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public bool Equals(Tile p) {
        // If parameter is null return false:
        if ((object)p == null) {
            return false;
        }

        return (x == p.x) && (y == p.y);
    }

    public override int GetHashCode() {
        int hash = 13;
        hash = (hash * 7) + x.GetHashCode();
        hash = (hash * 7) + y.GetHashCode();
        return hash;
    }



}
