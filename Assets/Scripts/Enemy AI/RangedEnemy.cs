using UnityEngine;
using System.Collections.Generic;

public class RangedEnemy : EnemyBase {

	private int _attackCooldown;

	public RangedEnemyTranquilizer tranquilizerPrefab;
	private GameObject _tranquilizer;

    public override void AttackBehavior() {
		if(_attackCooldown == 0 && _tranquilizer.rigidbody2D.velocity == Vector2.zero){
			_tranquilizer.transform.position = transform.position;
			_tranquilizer.transform.rotation = transform.rotation;
			_tranquilizer.rigidbody2D.velocity = Vector3.zero;
			_tranquilizer.rigidbody2D.AddRelativeForce(tranquilizerPrefab.velocity * Vector2.right);
			tranquilizerPrefab.activated = true;
			_attackCooldown = tranquilizerPrefab.cooldown;
		}
    }

	public override void LowerCooldowns(){
		if(_attackCooldown > 0){
			_attackCooldown--;
		}
	}

	public override void CreateNeededSubobjects(){
		_tranquilizer = PhotonNetwork.Instantiate(tranquilizerPrefab.name, new Vector2(-100, -100), Quaternion.identity, 0);
	}

}

