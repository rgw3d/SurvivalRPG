using UnityEngine;
using System.Collections.Generic;

public class RangedEnemy : EnemyBase {

	private int _attackCooldown;

	public RangedEnemyTranquilizer tranquilizerPrefab;
	private GameObject _tranquilizer;

    public override void AttackBehavior() {
		if(_attackCooldown == 0){
			_tranquilizer.transform.position = transform.position;
			_tranquilizer.transform.rotation = transform.rotation;
			_tranquilizer.rigidbody2D.velocity = Vector3.zero;
			_tranquilizer.rigidbody2D.AddRelativeForce(tranquilizerPrefab.velocity * -1 * Vector2.right);
			tranquilizerPrefab.activated = true;
			_attackCooldown = tranquilizerPrefab.cooldown;
		}
    }

	public override void lowerCooldowns(){
		if(_attackCooldown > 0){
			_attackCooldown--;
		}
	}

	public override void createNeededSubobjects(){
		_tranquilizer = PhotonNetwork.Instantiate(tranquilizerPrefab.name, new Vector2(-100, -100), Quaternion.identity, 0);
	}
	
    void OnTriggerStay2D(Collider2D col) {
        if (PlayerList.Contains(col.gameObject)) {
            rigidbody2D.AddForce(col.gameObject.rigidbody2D.velocity * 10);
        }
    }
	
	
	
}

