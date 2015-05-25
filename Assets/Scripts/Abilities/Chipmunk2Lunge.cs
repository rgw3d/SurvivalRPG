using UnityEngine;
using System.Collections.Generic;

public class Chipmunk2Lunge : Spell {

	public int lungeForce = -3300;
	public int recoverTime = 20;
	public int damage;
	public bool isLunging = false;
	private List<int> enemyIDs;

	void Start(){
		if(!GetComponentInParent<PhotonView>().isMine)
			collider2D.enabled = false;//should disable the collider completly on this side
		enemyIDs = new List<int>();
	}

	void OnTriggerStay2D(Collider2D other){
		if(isLunging){
			if(other.tag =="Enemy"){
				if(!enemyIDs.Contains(other.transform.GetInstanceID())){
					EnemyBase enemyBase = other.GetComponent("EnemyBase") as EnemyBase;
					enemyBase.OnEnemyAttacked(damage);
					enemyIDs.Add(other.transform.GetInstanceID());
				}
			}
		}
	}

	public void resetEnemyIDs(){
		enemyIDs = new List<int>();
	}
}
