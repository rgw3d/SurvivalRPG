﻿using UnityEngine;
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
}