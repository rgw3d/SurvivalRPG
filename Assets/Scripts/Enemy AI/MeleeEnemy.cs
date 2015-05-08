using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : EnemyBase {

    public override void AttackBehavior() {
        float angle = Mathf.Atan((transform.position.x - playerChar.transform.position.x) / (transform.position.y - playerChar.transform.position.y));
        if (transform.position.y - playerChar.transform.position.y >= 0)
            angle += Mathf.PI;
        transform.Translate(Speed * Mathf.Sin(angle), Speed * Mathf.Cos(angle), 0);
    }

    void OnTriggerEnter2D(Collider2D collider) {
      //  if (collider.tag == "Player")
        //    Destroy(gameObject);

    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject == playerChar) {
            rigidbody2D.AddForce(col.gameObject.rigidbody2D.velocity * 10);
        }
    }
	
}

