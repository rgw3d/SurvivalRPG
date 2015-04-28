using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour {
	
	public int swordDirection;
	public bool isAttacking = false;

	void Start(){
		DelegateHolder.OnPlayerAttack += isPlayerAttacking;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(isAttacking){
			if(other.tag == "Enemy"){
				Debug.Log("Attacked an enemy in direction " + swordDirection);
			}
		}


	}

	void isPlayerAttacking(int direction, bool isAttacking){
		if(swordDirection == direction){
			this.isAttacking = isAttacking;
		}
	}
}
