using UnityEngine;
using System.Collections;

public class MapTile {

    private TileType _tileType;

	public int X;
	public int Y;
	public int F; //=G+H
	public int G; //= Dist to init tile
	public int H; //= Dist to goal/ladder tile
	public MapTile Parent; // the public

    public enum TileType {
        background = 0,
        white = 1,
        wall = 2,
        goal = 3

    }

    public MapTile(TileType tileType, int x, int y) {
        _tileType = tileType;
		X = x;
		Y = y;
	}

    public MapTile(int tileValue) {
        switch (tileValue) {
            case 0:
                _tileType = TileType.background;
                break;
            case 1:
                _tileType = TileType.white;
                break;
            case 2:
                _tileType = TileType.wall;
                break;
            case 3:
                _tileType = TileType.goal;
                break;
        }
    }

    public void SetTileType(TileType TileType) {
        _tileType = TileType;
    }

    public TileType GetTileType() {
        return _tileType;
    }

    public override string ToString() {
        switch (_tileType) {
            case TileType.background:
                return "red";
            case TileType.white:
                return "white";
            case TileType.wall:
                return "blue";
            case TileType.goal:
                return "Green";
            default:
                return "white";
        }
    }

	public bool IsOrthogonalTo(MapTile otherTile){
        return (X + 1 == otherTile.X && Y == otherTile.Y)
            || (X - 1 == otherTile.X && Y == otherTile.Y)
            || (X == otherTile.X && Y - 1 == otherTile.Y)
            || (X == otherTile.X && Y + 1 == otherTile.Y);
	}
	
	public bool IsDiagonalTo(MapTile otherTile){
        return (X + 1 == otherTile.X && Y + 1 == otherTile.Y)
            || (X - 1 == otherTile.X && Y + 1 == otherTile.Y)
            || (X - 1 == otherTile.X && Y - 1 == otherTile.Y)
            || (X + 1 == otherTile.X && Y - 1 == otherTile.Y);
	}

    public override int GetHashCode() {
        return (int)_tileType;
    }

    public override bool Equals(System.Object obj) {
        // If parameter is null return false.
        if (obj == null)
            return false;

        //can it be cast
        MapTile  mt = obj as MapTile;
        if ((System.Object)mt == null)
            return false;

        // Return true if the fields match:
        return (GetTileType() == mt.GetTileType() && X == mt.X && Y == mt.Y);
    }
}
