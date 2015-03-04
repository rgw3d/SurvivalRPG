using UnityEngine;
using System.Collections;

public class StevensTile {

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

    public StevensTile(float xLoc, float yLoc) {
		x = xLoc;
		y = yLoc;
    }

}
