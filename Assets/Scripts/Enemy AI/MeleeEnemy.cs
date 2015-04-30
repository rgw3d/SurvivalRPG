using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : MonoBehaviour {

	public GameObject playerChar;
	public float healthValue=100;
	public float speed=0.05f;

	private int tick = 60;

    private bool _isAttacking = false;

	List<Vector3> currentPath = new List<Vector3>();
	int indexOfPath = 0;
	bool isDonePathing = false;
	pathfindingState currentState = pathfindingState.Inactive;

	public Map Map;

	public bool lineOfSight = false;
    public LayerMask playerMask;

	public enum pathfindingState {
		Inactive,
		Active,
		Attacking
	}



    void Start() {
        playerChar = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (playerChar == null) {
            Debug.Log("player char is null");
        }
        
	}

    bool InLineOfSight(GameObject target) {
        RaycastHit2D x = Physics2D.Linecast(transform.position, target.transform.position ,playerMask.value);
		if(x.transform.collider2D == playerChar.collider2D){
			return true;
		}
		return false;
    }

	void FixedUpdate () {
		float distance = Vector3.Distance(transform.position, playerChar.transform.position);
		if(distance < 25){ // good luck m8
			lineOfSight = InLineOfSight(playerChar);
			if(lineOfSight){
				if(distance < 5){
					currentState = pathfindingState.Attacking;
				}
				else{
					tick = 60;
					currentState = pathfindingState.Active;
				}
			}
			else{
				if(currentState == pathfindingState.Attacking){
					tick = 60;
					currentState = pathfindingState.Active;
				}
			}
		}
		else{
			currentState = pathfindingState.Inactive;
		}

		if(currentState == pathfindingState.Active){
			findPath();
			moveToPlayerAlongPath();
		}

		if(currentState == pathfindingState.Attacking){
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

