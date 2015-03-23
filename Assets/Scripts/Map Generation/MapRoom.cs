using UnityEngine;
using System.Collections;

public class MapRoom {

    public int LeftX;
    public int BottomY;
    public int RightX;
    public int TopY;

    private Vector2 _center;
    public bool isConnected = false;

    // y , x, height, width
    public MapRoom(int x, int y, int right, int top){
        LeftX = x;  //x
        BottomY = y; // y
        RightX = right; //width+x
		TopY = top; //height+y
        _center = new Vector2((LeftX + RightX) / 2, (BottomY + TopY) / 2);
	}

    public bool IntersectsWith(MapRoom otherRoom, int intersectionOffset) {
        return !(LeftX > otherRoom.RightX - intersectionOffset  //if any of these are true, return false.
            || RightX < otherRoom.LeftX + intersectionOffset
            || TopY < otherRoom.BottomY + intersectionOffset
            || BottomY > otherRoom.TopY - intersectionOffset);
    }

    public float DistanceToRoom(MapRoom r2) {
        return (Mathf.Sqrt(Mathf.Pow(GetCenter().x - r2.GetCenter().x, 2) + Mathf.Pow(GetCenter().y - r2.GetCenter().y, 2)));
    }

    public Vector2 GetCenter() {
        return _center;
    }

    public override int GetHashCode() {
        int hashCode = 7 * TopY;
        hashCode += BottomY;
        hashCode += LeftX;
        hashCode += RightX;
        return hashCode;
    }

    public override bool Equals(System.Object obj) {
        // If parameter is null return false.
        if (obj == null) 
            return false;

        MapRoom mR = obj as MapRoom;
        if ((System.Object)mR == null) 
            return false;

        // Return true if the fields match:
        return (LeftX == mR.LeftX && BottomY == mR.BottomY && RightX == mR.RightX && TopY == mR.TopY);
    }
}
