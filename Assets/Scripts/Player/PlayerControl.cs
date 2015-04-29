using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PlayerControl : Photon.MonoBehaviour{

    public Sprite FrontSprite;
    public Sprite BackSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;
    public Sprite FrontAttack;
    public Sprite BackAttack;
    public Sprite LeftAttack;
    public Sprite RightAttack;
    private Sprite _currentSprite;
	private SpriteRenderer _spriteRenderer;

	public float movementSpeed;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AttackKey;

<<<<<<< Updated upstream
=======
    private bool isAttacking = false;
	private bool hasLockedDirection = false;

>>>>>>> Stashed changes
    private CardinalDirection _playerDirection = CardinalDirection.front;
    private PlayerState _playerState = PlayerState.standing;

    public int AttackCooldownValue = 30;
    private int _attackCooldown = 0;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float lerpFraction;
    

	// Use this for initialization
	void Start () {
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;

        movementSpeed = PlayerStats.MovementSpeed;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = FrontSprite;
	
	}

    private enum CardinalDirection {
        front = 3,
        back = 1,
        left = 4,
        right = 2
    }
    private enum PlayerState {
        attacking,
        walking,
        standing,
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (photonView.isMine) {
            if (GameControl.ChatState == GameControl.ChattingState.ChatClosedButShowing 
                || GameControl.ChatState == GameControl.ChattingState.NoUsername) {

                    if (_playerState != PlayerState.attacking) //only update movement if not attacking
                        playerMovement();
                    playerSprite();
            }
            if (Input.GetKey(KeyCode.E)) { //just a test of the ability to work
                DelegateHolder.TriggerPlayerStatChange(StatType.Score, 1f);
            }
            playerAttack();
        }
        else {
            SyncedMovement();
        }

	}


    public void playerMovement() {
        if (Input.GetKey(UpKey)) {
            rigidbody2D.AddForce(Vector2.up * movementSpeed);
            _playerDirection = CardinalDirection.back;
            _playerState = PlayerState.walking;
        }
        if (Input.GetKey(DownKey)) {
            rigidbody2D.AddForce(Vector2.up * -1 * movementSpeed);
            _playerDirection = CardinalDirection.front;
            _playerState = PlayerState.walking;
        }
        if (Input.GetKey(LeftKey)) {
			rigidbody2D.AddForce(Vector2.right * -1 * movementSpeed);
            _playerDirection = CardinalDirection.left;
            _playerState = PlayerState.walking;

        }
        if (Input.GetKey(RightKey)) {
			rigidbody2D.AddForce(Vector2.right * movementSpeed);
            _playerDirection = CardinalDirection.right;
            _playerState = PlayerState.walking;
        }
        
    }

    public void playerSprite() {
        if (_playerDirection == CardinalDirection.back) {
            if(_playerState == PlayerState.attacking)
                _spriteRenderer.sprite = BackAttack;
            else
                _spriteRenderer.sprite = BackSprite;
        }
        if (_playerDirection == CardinalDirection.front) {
            if (_playerState == PlayerState.attacking)
                _spriteRenderer.sprite = FrontAttack;
            else
                _spriteRenderer.sprite = FrontSprite;
        }
        if (_playerDirection == CardinalDirection.left) {
            if (_playerState == PlayerState.attacking)
                _spriteRenderer.sprite = LeftAttack;
            else
                _spriteRenderer.sprite = LeftSprite;
        }
        if (_playerDirection == CardinalDirection.right) {
            if (_playerState == PlayerState.attacking)
                _spriteRenderer.sprite = RightAttack;
            else
                _spriteRenderer.sprite = RightSprite;
        }
    }

    public void playerAttack() {
<<<<<<< Updated upstream
        if (_attackCooldown == 0 && Input.GetKeyDown(AttackKey) && _playerState != PlayerState.attacking) {
            DelegateHolder.TriggerPlayerAttack((int)_playerDirection, true);
            _attackCooldown = AttackCooldownValue;
            _playerState = PlayerState.attacking;
        }
        else if(_attackCooldown > 0){
            _attackCooldown--;
        }
        
        if (_attackCooldown == 0 && _playerState == PlayerState.attacking) {
            _playerState = PlayerState.standing;
            DelegateHolder.TriggerPlayerAttack((int)_playerDirection, false);
        }
=======

		if(Input.GetKey(AttackKey)){

			CardinalDirection lockedCurrentDirection = 0;
			if(!hasLockedDirection){
				lockedCurrentDirection = _playerDirection;
				hasLockedDirection = true;
			}
			if(_playerDirection == lockedCurrentDirection){
				isAttacking = true;
				DelegateHolder.TriggerPlayerAttack((int)_playerDirection, true);
			}
			else{
				isAttacking = false;
				hasLockedDirection = false;
				DelegateHolder.TriggerPlayerAttack(1, false);
				DelegateHolder.TriggerPlayerAttack(2, false);
				DelegateHolder.TriggerPlayerAttack(3, false);
				DelegateHolder.TriggerPlayerAttack(4, false);
			}
		}
		else{
			isAttacking = false;
			DelegateHolder.TriggerPlayerAttack(1, false);
			DelegateHolder.TriggerPlayerAttack(2, false);
			DelegateHolder.TriggerPlayerAttack(3, false);
			DelegateHolder.TriggerPlayerAttack(4, false);
		}
>>>>>>> Stashed changes
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

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;          // this sample doesn't smooth rotation
        }
    }

    private void SyncedMovement() {
        // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead.

        lerpFraction = lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, lerpFraction);    // set our pos between A and B
    }

}
