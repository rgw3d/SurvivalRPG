using UnityEngine;
using System.Collections.Generic;

public class basic_Enemy_follow : MonoBehaviour {

	public GameObject playerChar;
	public float healthValue=100;
	public float speed=0.01f;

	private int tick = 60;

    private bool _isAttacking = false;

	List<Vector3> currentPath = new List<Vector3>();
	int indexOfPath = 0;
	bool isDonePathing = false;
	pathfindingState currentState = pathfindingState.Inactive;

	public Map Map;

    public LayerMask playerMask;

	public enum pathfindingState {
		Inactive,
		Active,
		Nearby
	}

    void Start() {
        playerChar = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (playerChar == null) {
            Debug.Log("player char is null");
        }
        
        
	}

    bool IsGrounded() {
        RaycastHit2D x = Physics2D.Linecast(transform.position, playerChar.transform.position,playerMask.value, 10f);

        return false;
    }

	void FixedUpdate () {

		float distance = Vector3.Distance(transform.position, playerChar.transform.position);

		if(distance > 5 && distance <= 10){
			if(currentState == pathfindingState.Nearby){
				tick = 60;
				currentState = pathfindingState.Active;
			}

		}

		if(distance <= 5){
			currentState = pathfindingState.Nearby;
		}

		if(currentState == pathfindingState.Active){
			findPath();
			moveToPlayerAlongPath();
		}

		if(currentState == pathfindingState.Nearby){
			nearbyMoveToPlayer();
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject == GameObject.FindGameObjectWithTag("Player")){
			Destroy(GameObject.FindGameObjectWithTag("Enemy"));
		}

	}

    void PlayerAttackStance(bool isAttacking) {
        _isAttacking = isAttacking;
    }

	void findPath(){
		if(tick == 60){//recreate the path every x number of seconds where x is the tick / 60
			currentPath = AStar.findABPath(transform.position, playerChar.transform.position);
			indexOfPath = 0;
			isDonePathing = false;
			tick = 0;
		}
		tick++;
	}

	void moveToPlayerAlongPath(){

		//compare x of enemy to next tile
		//compare y of enemy to next tile
		if(!isDonePathing){
			float angle = Mathf.Atan ( (transform.position.x - currentPath[indexOfPath].x )  / (transform.position.y - currentPath[indexOfPath].y));
			if (transform.position.y - currentPath[indexOfPath].y >= 0)
				angle += Mathf.PI;
			transform.Translate (speed *Mathf.Sin(angle), speed *Mathf.Cos(angle), 0);

			if(Mathf.Abs(transform.position.x - currentPath[indexOfPath].x) < .03f && Mathf.Abs(transform.position.y - currentPath[indexOfPath].y) < .03f){
				indexOfPath++;
			
			}
			//Debug.Log("indexOfPath is " + indexOfPath);
			if(indexOfPath == currentPath.Count){
				isDonePathing = true;
			}
			
		}
	}

	void nearbyMoveToPlayer(){
		float angle = Mathf.Atan ( (transform.position.x - playerChar.transform.position.x )  / (transform.position.y - playerChar.transform.position.y));
		if (transform.position.y - playerChar.transform.position.y >= 0)
			angle += Mathf.PI;
		transform.Translate (speed *Mathf.Sin(angle), speed *Mathf.Cos(angle), 0);
	}

    /*void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject == playerChar && _isAttacking) {
            if (col.gameObject.transform.position.x > transform.position.x)
                rigidbody2D.AddForce((col.gameObject.transform.position + transform.position) * 200);
            else
                rigidbody2D.AddForce((col.gameObject.transform.position + transform.position) * -200);
        }
        //else if (col.gameObject == playerChar && !_isAttacking)
            //DelegateHolder.TriggerPlayerStatChange(StatType.Health, -.1f);
    }*/

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject == playerChar && _isAttacking) {
            rigidbody2D.AddForce(col.gameObject.rigidbody2D.velocity * 10);
        }
    }


	void setGameObject(GameObject x) {
		playerChar = x;
	}
	


}

