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
		//float angle = findRotationAngle();
		Debug.Log(angle *180f /Mathf.PI);
		transform.Translate (speed *Mathf.Sin(angle), speed *Mathf.Cos(angle), 0);

		//transform.Rotate (0, 0, (findRotationAngle () - transform.localEulerAngles.z));
		//transform.Translate(Vector2.up *speed );
	}



	float findRotationAngle(){
		float returnAngle = 0;
		float xC=0;
		float yC=0;
		float x1 = this.transform.position.x;
		float y1 = this.transform.position.y;
		float x2 = playerChar.transform.position.x;
		float y2 = playerChar.transform.position.y;
		
		xC = x2 - x1;
		yC = y2 - y1;
		
		if (xC > 0 && yC > 0)
			returnAngle = 270 + (Mathf.Atan (yC / xC) / Mathf.PI * 180f);
		else if (xC > 0 && yC < 0)
			returnAngle = 270 + (Mathf.Atan (yC / xC) / Mathf.PI * 180f);
		else if (xC < 0 && yC < 0)
			returnAngle = 90 + (Mathf.Atan (yC / xC) / Mathf.PI * 180f);
		else if (xC < 0 && yC > 0)
			returnAngle = 90 + (Mathf.Atan (yC / xC) / Mathf.PI * 180f);
		else if (xC == 0) {
			if(yC<0)
				returnAngle = 180;
		}
		else if (yC == 0) {
			if(xC<0)
				returnAngle = 90;
		}
		return returnAngle / 180f * Mathf.PI;
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

