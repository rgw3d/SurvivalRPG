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
                float angle = Vector3.Angle(gameObject.transform.parent.transform.position,other.transform.position ) *180 /Mathf.PI;
                Debug.Log("Angle: " + angle);
                other.rigidbody2D.AddForce(new Vector2(100 * Mathf.Sin(angle), 100 * Mathf.Cos(angle)));
				Debug.Log("Attacked an enemy in direction " + swordDirection);
				//DelegateHolder.TriggerPlayerStatChange // why do we need delegates? We only need to tell the one GO about the change

				EnemyBase enemyBase = other.GetComponent("EnemyBase") as EnemyBase;
				enemyBase.OnAttacked(20);
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
