﻿using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour {
	
	public int swordDirection;
	public bool isAttacking = false;
	private List<int> enemyIDs;

	void Start(){
        if(GetComponentInParent<PhotonView>().isMine)
            DelegateHolder.OnPlayerAttack += isPlayerAttacking;
        else
            collider2D.enabled = false;//should disable the collider completly on this side
	}

	void OnTriggerStay2D(Collider2D other){
		if(isAttacking){
			if(other.tag =="Enemy"){
				if(!enemyIDs.Contains(other.transform.GetInstanceID())){
					Debug.Log("Attacked an enemy in direction " + swordDirection);
					EnemyBase enemyBase = other.GetComponent("EnemyBase") as EnemyBase;
					enemyBase.OnAttacked(20);
					enemyIDs.Add(other.transform.GetInstanceID());
				}
			}
		}
	}

	void isPlayerAttacking(bool isAttacking){
		this.isAttacking = isAttacking;
        if(isAttacking)
			enemyIDs = new List<int>();
	}
}
