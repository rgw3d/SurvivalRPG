using UnityEngine;
using System.Collections;

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

    private CardinalDirection _playerDirection = CardinalDirection.front;

    private float _lastSynchronizationTime = 0f;
    private float _syncDelay = 0f;
    private float _syncTime = 0f;
    private Vector2 _syncStartPosition;
    private Vector2 _syncEndPosition;
    

	// Use this for initialization
	void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = FrontSprite;
        _syncStartPosition = transform.position;
        _syncEndPosition = transform.position;
	}

    private enum CardinalDirection {
        front,
        back,
        left,
        right
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		movementSpeed = PlayerStats.MovementSpeed;
        if (Input.GetKey(KeyCode.E)) { //just a test of the ability to work
            DelegateHolder.TriggerPlayerStatChange(StatType.Score, 1f);   
        }
        if (photonView.isMine) {
            if (GameControl.ChatState == GameControl.ChattingState.ChatClosedButShowing || GameControl.ChatState == GameControl.ChattingState.NoUsername) { 
                playerMovement();
                playerSprite();
                playerAttack();
            }   
        }
        else {
            SyncedMovement();
        }

	}


    public void playerMovement() {
        if (Input.GetKey(UpKey)) {
            rigidbody2D.AddForce(Vector2.up * movementSpeed);
            _playerDirection = CardinalDirection.back;
        }
        if (Input.GetKey(DownKey)) {
            rigidbody2D.AddForce(Vector2.up * -1 * movementSpeed);
            _playerDirection = CardinalDirection.front;
        }
        if (Input.GetKey(LeftKey)) {
			rigidbody2D.AddForce(Vector2.right * -1 * movementSpeed);
            _playerDirection = CardinalDirection.left;

        }
        if (Input.GetKey(RightKey)) {
			rigidbody2D.AddForce(Vector2.right * movementSpeed);
            _playerDirection = CardinalDirection.right;
        }
        
    }

    public void playerSprite() {
        if (_playerDirection == CardinalDirection.back) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = BackAttack;
            else
                _spriteRenderer.sprite = BackSprite;
        }
        if (_playerDirection == CardinalDirection.front) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = FrontAttack;
            else
                _spriteRenderer.sprite = FrontSprite;
        }
        if (_playerDirection == CardinalDirection.left) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = LeftAttack;
            else
                _spriteRenderer.sprite = LeftSprite;
        }
        if (_playerDirection == CardinalDirection.right) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = RightAttack;
            else
                _spriteRenderer.sprite = RightSprite;
        }
    }

    public void playerAttack() {
       
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(rigidbody2D.position);
        }
        else {
            _syncEndPosition = (Vector2)stream.ReceiveNext();
            _syncStartPosition = rigidbody2D.position;

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;

        }
    }

    private void SyncedMovement() {
        _syncTime += Time.deltaTime;
        rigidbody2D.position = Vector2.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
    }

}
