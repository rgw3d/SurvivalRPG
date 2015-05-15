using UnityEngine;
using System.Collections;

public class Fireball: Spell {

	public int damage;
	public float velocity;


	void FixedUpdate () {
		//transform.Translate();
	}

	void OnTriggerStay(Collider2D other){
		if(other.tag == "Enemy" || other.tag == "Wall"){
			Destroy(gameObject);
		}
	}
}
