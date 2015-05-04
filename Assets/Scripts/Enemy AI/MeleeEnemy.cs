using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : Photon.MonoBehaviour {

	public GameObject playerChar;
	public float healthValue=100;
	public float speed=0.05f;

	private int _pathfindTick = 0;
    public int PathfindCooldownValue = 60;

    private bool _isAttacking = false;

	List<Vector3> currentPath = new List<Vector3>();
	int indexOfPath = 0;
	bool isDonePathing = false;
	pathfindingState currentState = pathfindingState.Inactive;

	public bool CanSeePlayer = false;
    public LayerMask LineOfSightMask;

    private Vector3 _latestCorrectPos;
    private Vector3 _onUpdatePos;
    private float _lerpFraction;

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
        RaycastHit2D x = Physics2D.Linecast(transform.position, target.transform.position ,LineOfSightMask.value);
		if(x.transform.gameObject == playerChar){
			return true;
		}
		return false;
    }

	void FixedUpdate () {

        if (photonView.isMine) {
            float distance = Vector3.Distance(transform.position, playerChar.transform.position);
            if (distance < 25) { // good luck m8
                CanSeePlayer = InLineOfSight(playerChar);
                if (CanSeePlayer) {
                    if (distance < 5) {
                        currentState = pathfindingState.Attacking;
                    }
                    else {
                        _pathfindTick = PathfindCooldownValue;
                        currentState = pathfindingState.Active;
                    }
                }
                else {
                    if (currentState == pathfindingState.Attacking) {
                        _pathfindTick = PathfindCooldownValue;
                        currentState = pathfindingState.Active;
                    }
                }
            }
            else {
                currentState = pathfindingState.Inactive;
            }

            if (currentState == pathfindingState.Active) {
                findPath();
                moveToPlayerAlongPath();
            }

            if (currentState == pathfindingState.Attacking) {
                nearbyMoveToPlayer();
            } 
        }
        else {
            SyncedMovement();
        }

	}

	void OnTriggerEnter2D(Collider2D collider){
		/*if(collider.gameObject == GameObject.FindGameObjectWithTag("Player")){
			Destroy(GameObject.FindGameObjectWithTag("Enemy"));
		}*/

	}

    void PlayerAttackStance(bool isAttacking) {
        _isAttacking = isAttacking;
    }

	void findPath(){
		if(_pathfindTick >= PathfindCooldownValue){//recreate the path every x number of seconds where x is the tick / 60
			currentPath = AStar.findABPath(transform.position, playerChar.transform.position);
			indexOfPath = 0;
			isDonePathing = false;
			_pathfindTick = 0;
		}
		_pathfindTick++;
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


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            _onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            _lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;          // this sample doesn't smooth rotation


        }
    }

    private void SyncedMovement() {

        _lerpFraction = _lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _lerpFraction);    // set our pos between A and B
    }
	


}

