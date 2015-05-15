using UnityEngine;
using System.Collections;

public class Fireball: Spell {

	public int damage;
	public float velocity;


	void FixedUpdate () {
		transform.Translate(new Vector3(velocity * transform.)));
	}
}
