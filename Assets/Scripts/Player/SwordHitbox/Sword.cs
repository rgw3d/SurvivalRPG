using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour {
	
	public swordDirection;
	public bool isAttacking = false;

	void Start(){
		DelegateHolder.OnPlayerAttack += isPlayerAttacking;
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag == "Enemy"){
			enemiesInHitbox.Add(other.gameObject);
			Debug.Log("Added " + other.tag);
		}

	}

	void isPlayerAttacking(int direction, bool isAttacking){
		this.isAttacking = isAttacking;
	}
}
