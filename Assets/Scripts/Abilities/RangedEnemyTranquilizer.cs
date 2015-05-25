using UnityEngine;
using System.Collections;

public class RangedEnemyTranquilizer : MonoBehaviour {

	public int damage;
	public int cooldown;
	public float velocity;
	public bool activated = false;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player" || other.tag == "Wall"){
			activated = false;
			rigidbody2D.velocity = Vector3.zero;
			transform.position = new Vector2(-100,-100);
			if(other.tag =="Player"){
				ChipmunkPlayerControl playerControl = other.GetComponent("ChipmunkPlayerControl") as ChipmunkPlayerControl;
				playerControl.onPlayerAttacked(damage);
			}
			
		}
	}
}
