using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : EnemyBase {

	private int _attackCooldown;

	public MeleeEnemySword enemySwordScript;
	public GameObject meleeEnemySword;

    public override void AttackBehavior() {
        float angle = Mathf.Atan((transform.position.x - Target.transform.position.x) / (transform.position.y - Target.transform.position.y));
        if (transform.position.y - Target.transform.position.y >= 0)
            angle += Mathf.PI;
        transform.Translate(Speed * Mathf.Sin(angle), Speed * Mathf.Cos(angle), 0, Space.World);

		if(_attackCooldown == 0){
			enemySwordScript.activated = true;
		}
		if(_attackCooldown < enemySwordScript.cooldown / 2){
			enemySwordScript.activated = false;
		}
    }

	public override void CreateNeededSubobjects(){
		enemySwordScript = meleeEnemySword.GetComponent("MeleeEnemySword") as MeleeEnemySword;
		_attackCooldown = 0;
	}

	public override void LowerCooldowns(){
		if(_attackCooldown > 0){
			_attackCooldown--;
		}
	}
}

