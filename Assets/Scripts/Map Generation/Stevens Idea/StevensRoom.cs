using UnityEngine;
using System.Collections;

public class StevensRoom {

	public int rTop;
	public int rBottom;
	public int rLeft;
	public int rRight;

	public bool isConnected = false;

    // y , x, height, width
	public StevensRoom(int bottom, int left, int top, int right){
		rTop = top;
		rBottom = bottom;
		rLeft = left;
		rRight = right;
	}

	public bool roomIntersectsWith(StevensRoom room2, int intersectionOffset){
		if(rLeft > room2.rRight - intersectionOffset){
			return false;
		}
		if(rRight < room2.rLeft + intersectionOffset){
			return false;
		}
		if(rTop < room2.rBottom + intersectionOffset){
			return false;
		}
		if(rBottom > room2.rTop - intersectionOffset){
			return false;
		}
		return true;
	}

	public float distanceToRoom(StevensRoom r2){
		float r1CenterX = rLeft + (rRight / 2);
		float r1CenterY = rBottom + (rTop / 2);
		float r2CenterX = r2.rLeft + (r2.rRight / 2);
		float r2CenterY = r2.rBottom + (r2.rTop / 2);

		return (Mathf.Sqrt(Mathf.Pow(r1CenterX - r2CenterX,2) + Mathf.Pow(r1CenterY - r2CenterY, 2)));
	}

    public override int GetHashCode() {
        int hashCode = rTop;
        hashCode += 7 * rBottom;
        hashCode += 13 * rLeft;
        hashCode += 17 * rRight;
        return hashCode;
    }
}
