using UnityEngine;
using System.Collections;

public class MeleeEnemySword : MonoBehaviour {

	public int damage;
	public int cooldown;
	public bool activated = false;
	


	void OnTriggerEnter2D(Collider2D other){
		if(activated){
			if(other.tag == "Player"){
				activated = false;
				ChipmunkPlayerControl playerControl = other.GetComponent("ChipmunkPlayerControl") as ChipmunkPlayerControl;
				playerControl.onPlayerAttacked(damage);
			}
		}
	}
}
