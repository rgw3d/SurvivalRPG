using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : Photon.MonoBehaviour {

    public GameObject playerChar;
    public float HealthStartingValue = 100;
    public float HealthValue;
    public float Speed = 0.05f;

    private int _pathfindTick = 0;
    public int PathfindCooldownValue = 60;
    public int LineOfSightDistance = 5;
    public int ActivationDistance = 25;

    private bool _isInLineOfSight = false;
    public LayerMask LineOfSightMask;

    private List<Vector3> _currentPath = new List<Vector3>();
    private int _indexOfPath = 0;
    private bool _isDonePathing = false;
    private PathfindingState _currentPathfindingState = PathfindingState.Inactive;

    private Vector3 _latestCorrectPos;//for networking
    private Vector3 _onUpdatePos;//networking
    private Quaternion _latestCorrectRot; //networking
    private Quaternion _onUpdateRot; //networking
    private float _lerpFraction;//networking

    private float _healthDelta;

    public enum PathfindingState {
        Inactive,
        Active,
        Attacking
    }

    void Start() {
        playerChar = GameObject.FindGameObjectWithTag("Player") as GameObject;
        HealthValue = HealthStartingValue;
    }


    void FixedUpdate() {
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
                PathFind();
                MoveToTargetAlongPath();
            }

            if (_currentPathfindingState == PathfindingState.Attacking) {
                AttackBehavior();
            }
        }
        else {
            SyncedMovement();
        }

        if (HealthValue <= 0) {
            Debug.Log("Destroying object");
            if (photonView.isMine)
                PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public bool InLineOfSight(GameObject target) {
        RaycastHit2D x = Physics2D.Linecast(transform.position, target.transform.position, LineOfSightMask.value);
        return x.transform.gameObject == playerChar;
    }

    public void PathFind() {
        if (_pathfindTick >= PathfindCooldownValue) {//recreate the path every x number of seconds where x is the tick / 60
            _currentPath = AStar.findABPath(transform.position, playerChar.transform.position);
            _indexOfPath = 0;
            _isDonePathing = false;
            _pathfindTick = 0;
        }
        _pathfindTick++;
    }

    public void MoveToTargetAlongPath() {
        //compare x of enemy to next tile
        //compare y of enemy to next tile
        if (!_isDonePathing) {
            float angle = Mathf.Atan((transform.position.x - _currentPath[_indexOfPath].x) / (transform.position.y - _currentPath[_indexOfPath].y));
            if (transform.position.y - _currentPath[_indexOfPath].y >= 0)
                angle += Mathf.PI;
            transform.Translate(Speed * Mathf.Sin(angle), Speed * Mathf.Cos(angle), 0);

            if (Mathf.Abs(transform.position.x - _currentPath[_indexOfPath].x) < .03f && Mathf.Abs(transform.position.y - _currentPath[_indexOfPath].y) < .03f) {
                _indexOfPath++;
            }
            //Debug.Log("indexOfPath is " + indexOfPath);
            if (_indexOfPath == _currentPath.Count) {
                _isDonePathing = true;
            }

        }
    }

    public abstract void AttackBehavior();

	public void OnAttacked(int damageTaken){
			HealthValue += -damageTaken;
            _healthDelta += -damageTaken;
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            float healthDelta = _healthDelta;
            _healthDelta = 0;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref healthDelta);
            
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            float healthDelta = 0;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref healthDelta);

            _latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            _onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            _latestCorrectRot = rot;
            _onUpdateRot = transform.rotation;
            _lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()

            
            HealthValue += healthDelta;

        }
    }

    public void SyncedMovement() {
        _lerpFraction = _lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _lerpFraction);    // set our pos between A and B
        transform.rotation = Quaternion.Slerp(_onUpdateRot, _latestCorrectRot, _lerpFraction);
    }
}
