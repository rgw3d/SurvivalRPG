using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	public int healAmount = 50;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			ChipmunkPlayerControl playerControl = other.GetComponent("ChipmunkPlayerControl") as ChipmunkPlayerControl;
			playerControl.onPlayerAttacked(-1 * healAmount);
			Destroy(gameObject);
		}
	}

}
