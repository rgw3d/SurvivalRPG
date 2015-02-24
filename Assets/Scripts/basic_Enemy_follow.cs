using UnityEngine;
using System.Collections;

public class basic_Enemy_follow : MonoBehaviour {

	public GameObject playerChar;
	public float healthValue=100;
	public float speed=0.1f;

	void FixedUpdate () {
		moveTowardsPlayer ();
		//sprayBullets ();
	}

	void moveTowardsPlayer(){
		//compare x of enemy to player
		//compare y of enemy to palyer
		float angle = Mathf.Atan ( (transform.position.x - playerChar.transform.position.x )  / (transform.position.y - playerChar.transform.position.y));
		if (transform.position.y - playerChar.transform.position.y > 0)
			angle += Mathf.PI;
		transform.Translate (speed *Mathf.Sin(angle), speed *Mathf.Cos(angle), 0);
	}

	void addDamage (float damage) {
		healthValue -= damage;
		if (healthValue < 0) {
			//scoreControl.Score += 1000;
			//spawnEnemies.totalEnemies--;
			Destroy (gameObject);
		}
	}

	void setGameObject(GameObject x) {
		playerChar = x;
	}
	


}

