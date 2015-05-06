using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemy : Photon.MonoBehaviour {

	public GameObject playerChar;
	public float HealthStartingValue=100;
    public float HealthValue;
	public float Speed=0.05f;

	private int _pathfindTick = 0;
    public int PathfindCooldownValue = 60;
    public int LineOfSightDistance = 5;
    public int ActivationDistance = 25;

	private List<Vector3> _currentPath = new List<Vector3>();
	private int _indexOfPath = 0;
	private bool _isDonePathing = false;
	private PathfindingState _currentPathfindingState = PathfindingState.Inactive;

	private bool _isInLineOfSight = false;
    public LayerMask LineOfSightMask;

    private Vector3 _latestCorrectPos;//for networking
    private Vector3 _onUpdatePos;//networking
    private float _lerpFraction;//networking

	public enum PathfindingState {
		Inactive,
		Active,
		Attacking
	}

    void Start() {
        playerChar = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (playerChar == null) {
            Debug.Log("player char is null");
        }
        HealthValue = HealthStartingValue;
        
	}

    bool InLineOfSight(GameObject target) {
        RaycastHit2D x = Physics2D.Linecast(transform.position, target.transform.position ,LineOfSightMask.value);
        return x.transform.gameObject == playerChar;
    }

	void FixedUpdate () {

        if (photonView.isMine) {
            float distance = Vector3.Distance(transform.position, playerChar.transform.position);
            if (distance < ActivationDistance) { // good luck m8
                _isInLineOfSight = InLineOfSight(playerChar);
                if (_isInLineOfSight) {
                    if (distance < LineOfSightDistance) {
                        _currentPathfindingState = PathfindingState.Attacking;
                    }
                    else {
                        _pathfindTick = PathfindCooldownValue;
                        _currentPathfindingState = PathfindingState.Active;
                    }
                }
                else {
                    if (_currentPathfindingState == PathfindingState.Attacking) {
                        _pathfindTick = PathfindCooldownValue;
                        _currentPathfindingState = PathfindingState.Active;
                    }
                }
            }
            else {
                _currentPathfindingState = PathfindingState.Inactive;
            }

            if (_currentPathfindingState == PathfindingState.Active) {
                findPath();
                moveToPlayerAlongPath();
            }

            if (_currentPathfindingState == PathfindingState.Attacking) {
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

	void findPath(){
		if(_pathfindTick >= PathfindCooldownValue){//recreate the path every x number of seconds where x is the tick / 60
			_currentPath = AStar.findABPath(transform.position, playerChar.transform.position);
			_indexOfPath = 0;
			_isDonePathing = false;
			_pathfindTick = 0;
		}
		_pathfindTick++;
	}

	void moveToPlayerAlongPath(){
		//compare x of enemy to next tile
		//compare y of enemy to next tile
		if(!_isDonePathing){
			float angle = Mathf.Atan ( (transform.position.x - _currentPath[_indexOfPath].x )  / (transform.position.y - _currentPath[_indexOfPath].y));
			if (transform.position.y - _currentPath[_indexOfPath].y >= 0)
				angle += Mathf.PI;
			transform.Translate (Speed *Mathf.Sin(angle), Speed *Mathf.Cos(angle), 0);

			if(Mathf.Abs(transform.position.x - _currentPath[_indexOfPath].x) < .03f && Mathf.Abs(transform.position.y - _currentPath[_indexOfPath].y) < .03f){
				_indexOfPath++;
			}
			//Debug.Log("indexOfPath is " + indexOfPath);
			if(_indexOfPath == _currentPath.Count){
				_isDonePathing = true;
			}
			
		}
	}

	void nearbyMoveToPlayer(){
		float angle = Mathf.Atan ( (transform.position.x - playerChar.transform.position.x )  / (transform.position.y - playerChar.transform.position.y));
		if (transform.position.y - playerChar.transform.position.y >= 0)
			angle += Mathf.PI;
		transform.Translate (Speed *Mathf.Sin(angle), Speed *Mathf.Cos(angle), 0);
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
        if (col.gameObject == playerChar) {
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

