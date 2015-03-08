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



}
