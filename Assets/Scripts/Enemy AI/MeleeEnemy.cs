using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : EnemyBase {

    public override void AttackBehavior() {
        float angle = Mathf.Atan((transform.position.x - Target.transform.position.x) / (transform.position.y - Target.transform.position.y));
        if (transform.position.y - Target.transform.position.y >= 0)
            angle += Mathf.PI;
        transform.Translate(Speed * Mathf.Sin(angle), Speed * Mathf.Cos(angle), 0, Space.World);
    }

	public override void CreateNeededSubobjects(){

	}

	public override void LowerCooldowns(){

	}
}

