using UnityEngine;
using System.Collections;

public class Fireball: Spell {

	public int damage;
	public float velocity;
	public bool activated = false;

	void Start(){
		if(!GetComponentInParent<PhotonView>().isMine)
			collider2D.enabled = false;//should disable the collider completly on this side
	}
 
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Enemy" || other.tag == "Wall"){
			activated = false;
			rigidbody2D.velocity = Vector3.zero;
			transform.position = new Vector2(-100,-100);
			if(other.tag =="Enemy"){
				EnemyBase enemyBase = other.GetComponent("EnemyBase") as EnemyBase;
				enemyBase.OnEnemyAttacked(damage);
			}

		}
	}
}
