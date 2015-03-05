using UnityEngine;
using System.Collections;

public class StevensRoom {

	public int rTop;
	public int rBottom;
	public int rLeft;
	public int rRight;

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
}
