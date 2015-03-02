using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public float x;
	public float y;
    public TileType tileType;

    public enum TileType {
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
    public Tile(float x, float y) {

    }

}
