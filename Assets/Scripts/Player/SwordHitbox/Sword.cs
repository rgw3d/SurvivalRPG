using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour {
	
	public int swordDirection;
	public bool isAttacking = false;


	void Start(){
        if(GetComponentInParent<PhotonView>().isMine)
            DelegateHolder.OnPlayerAttack += isPlayerAttacking;
        else
            collider2D.enabled = false;//should disable the colliders completly on this side
	}

	void OnTriggerStay2D(Collider2D other){
		if(isAttacking){
			if(other.tag =="Enemy"){
				Debug.Log("Attacked an enemy in direction " + swordDirection);

			}
		}

	}

	void isPlayerAttacking(int direction, bool isAttacking){
		if(swordDirection == direction){
			this.isAttacking = isAttacking;
		}
		else{
			this.isAttacking = false;
		}
	}
}
