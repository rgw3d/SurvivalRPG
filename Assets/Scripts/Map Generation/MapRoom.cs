using UnityEngine;
using System.Collections;

public class MapRoom {


    public int X;
    public int Y;
    public int XPlusRoomWidth;
    public int YPlusRoomHeight;

    public bool isConnected = false;

    // y , x, height, width
    public MapRoom(int x, int y, int top, int right){
		YPlusRoomHeight = top; //height
		Y = y; // y
		X = x;  //x
		XPlusRoomWidth = right; //width
	}

    public bool roomIntersectsWith(MapRoom otherRoom, int intersectionOffset) {
        return !(X > otherRoom.XPlusRoomWidth - intersectionOffset  //if any of these are true, return false.
            || XPlusRoomWidth < otherRoom.X + intersectionOffset
            || YPlusRoomHeight < otherRoom.Y + intersectionOffset
            || Y > otherRoom.YPlusRoomHeight - intersectionOffset);
    }

    public float distanceToRoom(MapRoom r2) {
        float r1CenterX = X + (XPlusRoomWidth / 2);
        float r1CenterY = Y + (YPlusRoomHeight / 2);
        float r2CenterX = r2.X + (r2.XPlusRoomWidth / 2);
        float r2CenterY = r2.Y + (r2.YPlusRoomHeight / 2);

        return (Mathf.Sqrt(Mathf.Pow(r1CenterX - r2CenterX, 2) + Mathf.Pow(r1CenterY - r2CenterY, 2)));
    }

    public override int GetHashCode() {
        int hashCode = 7 * YPlusRoomHeight;
        hashCode += Y;
        hashCode += X;
        hashCode += XPlusRoomWidth;
        return hashCode;
    }
}
