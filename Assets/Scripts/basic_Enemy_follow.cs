using UnityEngine;
using System.Collections;

public class basic_Enemy_follow : MonoBehaviour {

	public GameObject playerChar;
	public float healthValue=100;
	public float speed=0.01f;

    private bool _isAttacking = false;

    public event ChangePlayerStat OnChangeStat;

    void Start() {
        PlayerControl playerController = FindObjectOfType<PlayerControl>();
        playerController.OnPlayerAttack += PlayerAttackStance;//add the method to the event, and the event is made from the delegate
    }

	void FixedUpdate () {
		moveTowardsPlayer ();
	}

    void PlayerAttackStance(bool isAttacking) {
        _isAttacking = isAttacking;
    }

	void moveTowardsPlayer(){
		//compare x of enemy to player
		//compare y of enemy to palyer
		float angle = Mathf.Atan ( (transform.position.x - playerChar.transform.position.x )  / (transform.position.y - playerChar.transform.position.y));
		if (transform.position.y - playerChar.transform.position.y > 0)
			angle += Mathf.PI;
		transform.Translate (speed *Mathf.Sin(angle), speed *Mathf.Cos(angle), 0);
	}

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject == playerChar && _isAttacking) {
            if (col.gameObject.transform.position.x> transform.position.x)
                rigidbody2D.AddForce((col.gameObject.transform.position + transform.position) * 200);
            else
                rigidbody2D.AddForce( (col.gameObject.transform.position + transform.position) * -200  );
        }
        else if (col.gameObject == playerChar && !_isAttacking)
            OnChangeStat(StatType.Health, -.1f);
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject == playerChar && _isAttacking) {
            rigidbody2D.AddForce(col.gameObject.rigidbody2D.velocity * 10);
        }
    }


	void setGameObject(GameObject x) {
		playerChar = x;
	}
	


}

