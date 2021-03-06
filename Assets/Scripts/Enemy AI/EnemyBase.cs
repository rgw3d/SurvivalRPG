﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : Photon.MonoBehaviour {

    public List<GameObject> PlayerList;
    public GameObject Target;
    public float HealthStartingValue = 100;
    public float HealthValue;
    public float Speed = 0.05f;
	public int rotationSpeed = 5;

    public Animator Animator;

    private int _pathfindTick = 0;
    public int PathfindCooldownValue = 60;
    public int LineOfSightDistance = 10;
    public int ActivationDistance = 25;
    public float ResetTargetCooldown = .25f;

    private bool _isInLineOfSight = false;
    public LayerMask LineOfSightMask;

    private List<Vector3> _currentPath = new List<Vector3>();
    private int _indexOfPath = 0;
    private bool _isDonePathing = false;
    private PathfindingState _currentPathfindingState = PathfindingState.Inactive;
    private static Transform _staticTransform;

    private Vector3 _latestCorrectPos;//for networking
    private Vector3 _onUpdatePos;//networking
    private Quaternion _latestCorrectRot; //networking
    private Quaternion _onUpdateRot; //networking
    private float _lerpFraction;//networking

    public enum PathfindingState {
        Inactive = 0,
        Active = 1,
        Attacking = 2
    }

    void Start() {
        if (photonView.isMine) {
            UpdatePlayerList();
            HealthValue = HealthStartingValue;
            InvokeRepeating("SetTarget", 1, ResetTargetCooldown);//Set it to find a new target 
            DelegateHolder.OnPlayerHasConnected += PlayerConnectionChange;
            DelegateHolder.OnPlayerHasDisconnected += PlayerConnectionChange;
            CreateNeededSubobjects();
            InvokeRepeating("UpdatePlayerList", .5f, 1);
            StartCoroutine(justWaitThenStop(1));
        }
    }

	public abstract void CreateNeededSubobjects();

    public void UpdatePlayerList() {
        PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void PlayerConnectionChange() {
        Debug.Log("Player has connected!");
        InvokeRepeating("UpdatePlayerList", .5f, 1);
        StartCoroutine(justWaitThenStop(1));
    }

    public IEnumerator justWaitThenStop(int time) {
        yield return new WaitForSeconds(time);
        CancelInvoke("UpdatePlayerList");
    }


	void Update(){
        if (photonView.isMine) {
            Vector2 enemyPosition = transform.position;
            Vector2 playerPosition = PlayerList[0].transform.position;
            float angle = Mathf.Atan2(playerPosition.y - enemyPosition.y, playerPosition.x - enemyPosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle)), rotationSpeed * Time.deltaTime);
        }
	}
	
	void FixedUpdate() {
        if (photonView.isMine && Target != null) {
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            
            if (distance < ActivationDistance) { // good luck m8
                _isInLineOfSight = InLineOfSight(Target);
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
        else if(!photonView.isMine) {
            SyncedMovement();
        }

        if (HealthValue <= 0) {
            //Debug.Log("Enemy Health < 0 \nDestroying");
            if (photonView.isMine) {
                photonView.RPC("IncreaseXP", PhotonTargets.All, 20);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
		LowerCooldowns();
        UpdateAnimations();
    }

    [RPC]
    public void IncreaseXP(int amt) {
        PlayerStats.PlayerScore += amt;
    } 

	public abstract void LowerCooldowns();

    public void UpdateAnimations() {
        Animator.SetInteger("State", (int)_currentPathfindingState);
    }

    public void SetTarget() {
        _staticTransform = transform;
        if (PlayerList != null && PlayerList.Count != 0) {//not null and at least one player
            if (PlayerList.Count > 1) { //more than one player. sort list
                try {
                    PlayerList.Sort(ComparePlayerDistances);
                }
                catch (MissingReferenceException e) {
                    print(e.Message);
                    UpdatePlayerList();
                    Target = null;
                }
            }
            Target = PlayerList[0];//always select first index
        }
        else {
            Target = null;
        }
    }

    private static int ComparePlayerDistances(GameObject player1, GameObject player2) {
        if (player1 == null || player2 == null) {
            return 1;
        }
        float dist1 = Vector3.Distance(_staticTransform.position, player1.transform.position);
        float dist2 = Vector3.Distance(_staticTransform.position, player2.transform.position);
        return Mathf.RoundToInt(dist1 - dist2);

    }

    public bool InLineOfSight(GameObject target) {
       RaycastHit2D x = Physics2D.Linecast(transform.position, target.transform.position, LineOfSightMask.value);
       return x.collider == target.collider2D;
    }

    public void PathFind() {
        if (_pathfindTick >= PathfindCooldownValue) {//recreate the path every x number of seconds where x is the tick / 60
            _currentPath = AStar.findABPath(transform.position, Target.transform.position);
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
            transform.Translate(Speed * Mathf.Sin(angle), Speed * Mathf.Cos(angle), 0, Space.World);

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

	public void OnEnemyAttacked(float damageTaken){
		HealthValue -= damageTaken;
        photonView.RPC("OnEnemyAttackedRPC", PhotonTargets.Others, damageTaken);
	}

    [RPC]
    public void OnEnemyAttackedRPC(float damageTaken) {
        HealthValue -= damageTaken;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            short ste = (short)_currentPathfindingState;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref ste);
            
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            _onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            _latestCorrectRot = rot;
            _onUpdateRot = transform.rotation;
            _lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()

            short ste = 0;
            stream.Serialize(ref ste);
            _currentPathfindingState = (PathfindingState)ste;


        }
    }

    public void SyncedMovement() {
        _lerpFraction = _lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _lerpFraction);    // set our pos between A and B
        transform.rotation = Quaternion.Slerp(_onUpdateRot, _latestCorrectRot, _lerpFraction);
    }
}
